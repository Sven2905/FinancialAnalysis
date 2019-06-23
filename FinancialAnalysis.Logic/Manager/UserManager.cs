using DevExpress.Mvvm;
using FinancialAnalysis.Logic.Messages;
using FinancialAnalysis.Models.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Timers;
using System.Windows;
using Utilities;
using WebApiWrapper;
using WebApiWrapper.Administration;

namespace FinancialAnalysis.Logic
{
    public class UserManager
    {
        #region Constructor

        private UserManager()
        {
            tokenTimer.Interval = timerInvervall;
            tokenTimer.Elapsed += TokenTimer_Elapsed;
        }

        private void TokenTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            WebApiConfiguration.GetKey(username, password); ;
        }

        #endregion Constructor

        #region Fields

        private const int timerInvervall = 1000 * 60 * 15; // every 15 Minutes
        private readonly Timer tokenTimer = new Timer();
        private string username;
        private string password;

        #endregion Fields

        #region Properties

        public static UserManager Instance { get; } = new UserManager();
        public List<UserRight> UserRightList { get; private set; }

        #endregion Properties

        #region Methods

        public SvenTechCollection<User> UserList { get; set; }

        private SvenTechCollection<User> LoadUsersFromDB()
        {
            return Users.GetAll().ToSvenTechCollection();
        }

        public User NewUser()
        {
            User newUser = new User();

            foreach (UserRight item in UserRightList)
            {
                newUser.UserRightUserMappings.Add(new UserRightUserMapping(0, item.UserRightId, false, 0));
            }

            return newUser;
        }

        public bool DeleteUser(User user)
        {
            if (user == null || user.UserId == 0)
            {
                return false;
            }

            UserRightUserMappings.Delete(user.UserId);
            Users.Delete(user.UserId);
            UserList.Remove(user);

            return true;
        }

        public User InsertOrUpdateUser(User user)
        {
            if (user == null)
            {
                throw new NullReferenceException("User is null!");
            }

            bool IsNewUser = user.UserId == 0;

            if (!IsNewUser)
            {
                if (!string.IsNullOrEmpty(user.Password))
                {
                    Users.UpdatePassword(user);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(user.Password))
                {
                    throw new ArgumentException("Password is not set!");
                }
            }

            if (user.UserId == 0)
            {
                user.UserId = Users.Insert(user);
            }
            else
            {
                Users.Update(user);
            }

            if (IsNewUser)
            {
                foreach (UserRightUserMapping item in user.UserRightUserMappings)
                {
                    item.RefUserId = user.UserId;
                }
            }

            foreach (UserRightUserMapping userRightMapping in user.UserRightUserMappings)
            {
                if (userRightMapping.UserRightUserMappingId == 0)
                {
                    UserRightUserMappings.Insert(userRightMapping);
                }
                else
                {
                    UserRightUserMappings.Update(userRightMapping);
                }
            }

            RefreshUsers();

            return user;
        }

        public bool IsUserRightGranted(int userId, Permission permission)
        {
            User user = UserList.SingleOrDefault(x => x.UserId == userId);
            if (user == null)
            {
                return false;
            }

            return IsUserRightGranted(user, permission);
        }

        public bool IsUserRightGranted(User user, Permission permission)
        {
            return user.UserRights.Single(x => x.Permission == permission).IsGranted;
        }

        public SvenTechCollection<UserRightUserMappingFlatStructure> GetUserRightUserMappingFlatStructure(User user)
        {
            SvenTechCollection<UserRightUserMappingFlatStructure> UserRightUserMappingFlatStructure = new SvenTechCollection<UserRightUserMappingFlatStructure>();
            if (user.UserRights.Count == 0)
            {
                user.UserRights = UserRights.GetAll();
            }
            foreach (UserRightUserMapping item in user.UserRightUserMappings)
            {
                UserRightUserMappingFlatStructure.Add(new UserRightUserMappingFlatStructure(user, Instance.UserRightList.Single(x => x.UserRightId == item.RefUserRightId), item.UserRightUserMappingId));
            }

            return UserRightUserMappingFlatStructure;
        }

        public List<UserRightUserMapping> ConvertUserRightUserMappingFlatStructureToNormal(
            SvenTechCollection<UserRightUserMappingFlatStructure> UserRightUserMappingFlatStructure)
        {
            List<UserRightUserMapping> userRightUserMappings = new List<UserRightUserMapping>();

            foreach (UserRightUserMappingFlatStructure item in UserRightUserMappingFlatStructure)
            {
                userRightUserMappings.Add(new UserRightUserMapping(item.RefUserId, item.RefUserRightId,
                    item.IsGranted, item.UserRightUserMappingId));
            }

            return userRightUserMappings;
        }

        public void RefreshUsers()
        {
            UserList = LoadUsersFromDB();
        }

        /// <summary>
        ///     Returns null if no user is found
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public User GetUserByNameAndPassword(string username, string password)
        {
            password = Encryption.ComputeHash(password, new SHA256CryptoServiceProvider(),
                new byte[]
                {
                    0x6c, 0xa6, 0x27, 0x0d, 0x62, 0xd4, 0x80, 0xc7, 0x50, 0xc9, 0x93, 0xef, 0xfb, 0x64, 0x90, 0x16,
                    0x7d, 0xc7, 0x1d, 0x6f, 0xb0, 0xe3, 0x80, 0xdc, 0x73
                });

            try
            {
                this.username = username;
                this.password = password;
                WebApiConfiguration.GetKey(username, password);
                tokenTimer.Enabled = true;
                tokenTimer.Start();
            }
            catch (Exception)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Verbindungsfehler", "Die WebApi ist nicht erreichbar, bitte Verbindungsparameter überprüfen.", MessageBoxImage.Error));
                Messenger.Default.Send(new OpenWebApiConfigurationWindow());
                return null;
            }
            if (string.IsNullOrEmpty(WebApiConfiguration.WebApiKey))
            {
                return null;
            }

            UserRightList = LoadUserRightsFromDB();
            UserList = LoadUsersFromDB();
            return UserList.Single(x => x.LoginUser == username);
        }

        private List<UserRight> LoadUserRightsFromDB()
        {
            return UserRights.GetAll().ToList();
        }

        public void GrantPermission(User user, Permission permission)
        {
            UserRight right = Instance.UserRightList.Single(x => x.Permission == permission);
            UserRightUserMapping tempUserRightUserMapping =
                user.UserRightUserMappings.SingleOrDefault(x => x.RefUserRightId == right.UserRightId);
            if (tempUserRightUserMapping != null)
            {
                tempUserRightUserMapping.IsGranted = true;
                if (tempUserRightUserMapping.UserRightUserMappingId == 0)
                {
                    UserRightUserMappings.Insert(tempUserRightUserMapping);
                }
                else
                {
                    UserRightUserMappings.Update(tempUserRightUserMapping);
                }
            }
            else
            {
                UserRightUserMappings.Insert(
                    new UserRightUserMapping(user.UserId, right.UserRightId, true, 0));
            }
        }

        public void RevokePermission(User user, Permission permission)
        {
            UserRight right = Instance.UserRightList.Single(x => x.Permission == permission);
            UserRightUserMapping tempUserRightUserMapping =
                user.UserRightUserMappings.SingleOrDefault(x => x.RefUserRightId == right.UserRightId);
            if (tempUserRightUserMapping != null)
            {
                tempUserRightUserMapping.IsGranted = false;
                if (tempUserRightUserMapping.UserRightUserMappingId == 0)
                {
                    UserRightUserMappings.Insert(tempUserRightUserMapping);
                }
                else
                {
                    UserRightUserMappings.Update(tempUserRightUserMapping);
                }
            }
            else
            {
                UserRightUserMappings.Insert(
                    new UserRightUserMapping(user.UserId, right.UserRightId, false, 0));
            }
        }

        #endregion Methods
    }
}