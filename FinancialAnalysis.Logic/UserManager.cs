using DevExpress.Mvvm;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Logic.Messages;
using FinancialAnalysis.Models.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace FinancialAnalysis.Logic
{
    public class UserManager : ViewModelBase
    {
        #region Properties

        public static UserManager Instance { get; } = new UserManager();
        public List<UserRight> UserRights { get; }

        #endregion Properties

        #region Constructor

        private UserManager()
        {
            UserRights = LoadUserRightsFromDB();
            Users = LoadUsersFromDB();
        }

        #endregion Constructor

        #region Methods

        public SvenTechCollection<User> Users { get; set; }

        private SvenTechCollection<User> LoadUsersFromDB()
        {
            SvenTechCollection<User> allUsers = new SvenTechCollection<User>();
            try
            {
                allUsers = DataLayer.Instance.Users.GetAll().ToSvenTechCollection();
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
            }

            return allUsers;
        }

        public User NewUser()
        {
            User newUser = new User();

            foreach (var item in UserRights)
            {
                newUser.UserRightUserMappings.Add(new UserRightUserMapping(0, item.UserRightId, false) { User = newUser, UserRight = item });
            }

            return newUser;
        }

        public bool DeleteUser(User user)
        {
            if (user == null || user.UserId == 0)
            {
                return false;
            }

            try
            {
                DataLayer.Instance.Users.Delete(user.UserId);
                Users.Remove(user);
            }
            catch (Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
                return false;
            }

            return true;
        }

        public User InsertOrUpdateUser(User user)
        {
            if (user == null)
            {
                throw new NullReferenceException("User is null!");
            }

            try
            {
                if (user.UserId != 0)
                {
                    if (!string.IsNullOrEmpty(user.Password))
                    {
                        DataLayer.Instance.Users.UpdatePassword(user);
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(user.Password))
                    {
                        throw new ArgumentException("Password is not set!");
                    }
                }
                user = DataLayer.Instance.Users.UpdateOrInsert(user);
                DataLayer.Instance.UserRightUserMappings.UpdateOrInsert(user.UserRightUserMappings);
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
            }
            return user;
        }

        public bool IsUserRightGranted(int userId, Permission permission)
        {
            var user = Users.SingleOrDefault(x => x.UserId == userId);
            if (user == null)
            {
                return false;
            }
            else
            {
                return IsUserRightGranted(user, permission);
            }
        }

        public bool IsUserRightGranted(User user, Permission permission)
        {
            foreach (var right in user.UserRights.Keys)
            {
                if (right.Permission == permission)
                {
                    return user.UserRights[right];
                }
            }
            return false;
        }

        public SvenTechCollection<UserRightUserMappingFlatStructure> GetUserRightUserMappingFlatStructure(User user)
        {
            SvenTechCollection<UserRightUserMappingFlatStructure> UserRightUserMappingFlatStructure = new SvenTechCollection<UserRightUserMappingFlatStructure>();
            foreach (var item in user.UserRightUserMappings)
            {
                UserRightUserMappingFlatStructure.Add(new UserRightUserMappingFlatStructure(item));
            }
            return UserRightUserMappingFlatStructure;
        }

        public void RefreshUsers()
        {
            LoadUsersFromDB();
        }

        /// <summary>
        /// Returns null if no user is found
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public User GetUserByNameAndPassword(string name, string password)
        {
            password = Encryption.ComputeHash(password, new SHA256CryptoServiceProvider(), new byte[] { 0x6c, 0xa6, 0x27, 0x0d, 0x62, 0xd4, 0x80, 0xc7, 0x50, 0xc9, 0x93, 0xef, 0xfb, 0x64, 0x90, 0x16, 0x7d, 0xc7, 0x1d, 0x6f, 0xb0, 0xe3, 0x80, 0xdc, 0x73 });
            User user = DataLayer.Instance.Users.GetUserByNameAndPassword(name, password);

            return user;
        }

        private List<UserRight> LoadUserRightsFromDB()
        {
            return DataLayer.Instance.UserRights.GetAll();
        }

        public void GrantPermission(User user, Permission permission)
        {
            var right = Instance.UserRights.Single(x => x.Permission == permission);
            var tempUserRightUserMapping = user.UserRightUserMappings.SingleOrDefault(x => x.RefUserRightId == right.UserRightId);
            if (tempUserRightUserMapping != null)
            {
                tempUserRightUserMapping.IsGranted = true;
                DataLayer.Instance.UserRightUserMappings.UpdateOrInsert(tempUserRightUserMapping);
            }
            else
            {
                DataLayer.Instance.UserRightUserMappings.UpdateOrInsert(new UserRightUserMapping(user.UserId, right.UserRightId, true));
            }
        }

        public void RevokePermission(User user, Permission permission)
        {
            var right = Instance.UserRights.Single(x => x.Permission == permission);
            var tempUserRightUserMapping = user.UserRightUserMappings.SingleOrDefault(x => x.RefUserRightId == right.UserRightId);
            if (tempUserRightUserMapping != null)
            {
                tempUserRightUserMapping.IsGranted = false;
                DataLayer.Instance.UserRightUserMappings.UpdateOrInsert(tempUserRightUserMapping);
            }
            else
            {
                DataLayer.Instance.UserRightUserMappings.UpdateOrInsert(new UserRightUserMapping(user.UserId, right.UserRightId, false));
            }
        }

        #endregion Methods
    }
}
