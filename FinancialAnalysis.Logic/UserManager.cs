using FinancialAnalysis.Models.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

        }

        #endregion Constructor

        #region Properties

        public static UserManager Instance { get; } = new UserManager();
        public List<UserRight> UserRightList { get; private set; }

        #endregion Properties

        #region Methods

        public SvenTechCollection<User> UserList { get; set; }

        private SvenTechCollection<User> LoadUsersFromDB()
        {
            var allUsers = new SvenTechCollection<User>();
            return Users.GetAll().ToSvenTechCollection();
        }

        public User NewUser()
        {
            var newUser = new User();

            foreach (var item in UserRightList)
            {
                newUser.UserRightUserMappings.Add(new UserRightUserMapping(0, item.UserRightId, false));
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

            var IsNewUser = user.UserId == 0;

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
                Users.Insert(user);
            }
            else
            {
                Users.Update(user);
            }

            if (IsNewUser)
            {
                foreach (var item in user.UserRightUserMappings)
                {
                    item.RefUserId = user.UserId;
                }
            }

            foreach (var userRightMapping in user.UserRightUserMappings)
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
            var user = UserList.SingleOrDefault(x => x.UserId == userId);
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
            var UserRightUserMappingFlatStructure = new SvenTechCollection<UserRightUserMappingFlatStructure>();
            foreach (var item in user.UserRightUserMappings)
            {
                UserRightUserMappingFlatStructure.Add(new UserRightUserMappingFlatStructure(user, UserManager.Instance.UserRightList.Single(x => x.UserRightId == item.RefUserRightId)));
            }

            return UserRightUserMappingFlatStructure;
        }

        public List<UserRightUserMapping> ConvertUserRightUserMappingFlatStructureToNormal(
            SvenTechCollection<UserRightUserMappingFlatStructure> UserRightUserMappingFlatStructure)
        {
            var userRightUserMappings = new List<UserRightUserMapping>();

            foreach (var item in UserRightUserMappingFlatStructure)
            {
                userRightUserMappings.Add(new UserRightUserMapping(item.RefUserId, item.RefUserRightId,
                    item.IsGranted));
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
            Configuration.GetKey(username, password);
            if (string.IsNullOrEmpty(Configuration.WebApiKey))
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
            var right = Instance.UserRightList.Single(x => x.Permission == permission);
            var tempUserRightUserMapping =
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
                    new UserRightUserMapping(user.UserId, right.UserRightId, true));
            }
        }

        public void RevokePermission(User user, Permission permission)
        {
            var right = Instance.UserRightList.Single(x => x.Permission == permission);
            var tempUserRightUserMapping =
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
                    new UserRightUserMapping(user.UserId, right.UserRightId, false));
            }
        }

        #endregion Methods
    }
}