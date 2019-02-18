using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Windows;
using DevExpress.Mvvm;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Logic.Messages;
using FinancialAnalysis.Models.Administration;
using Utilities;

namespace FinancialAnalysis.Logic
{
    public class UserManager
    {
        #region Constructor

        private UserManager()
        {
            UserRights = LoadUserRightsFromDB();
            Users = LoadUsersFromDB();
        }

        #endregion Constructor

        #region Properties

        public static UserManager Instance { get; } = new UserManager();
        public List<UserRight> UserRights { get; }

        #endregion Properties

        #region Methods

        public SvenTechCollection<User> Users { get; set; }

        private SvenTechCollection<User> LoadUsersFromDB()
        {
            var allUsers = new SvenTechCollection<User>();
            return DataContext.Instance.Users.GetAll().ToSvenTechCollection();
        }

        public User NewUser()
        {
            var newUser = new User();

            foreach (var item in UserRights)
                newUser.UserRightUserMappings.Add(new UserRightUserMapping(0, item.UserRightId, false)
                { User = newUser, UserRight = item });

            return newUser;
        }

        public bool DeleteUser(User user)
        {
            if (user == null || user.UserId == 0) return false;

            DataContext.Instance.UserRightUserMappings.Delete(user.UserId);
            DataContext.Instance.Users.Delete(user.UserId);
            Users.Remove(user);

            return true;
        }

        public User InsertOrUpdateUser(User user)
        {
            if (user == null) throw new NullReferenceException("User is null!");

            var IsNewUser = user.UserId == 0;

            if (!IsNewUser)
            {
                if (!string.IsNullOrEmpty(user.Password)) DataContext.Instance.Users.UpdatePassword(user);
            }
            else
            {
                if (string.IsNullOrEmpty(user.Password)) throw new ArgumentException("Password is not set!");
            }

            DataContext.Instance.Users.UpdateOrInsert(user);

            if (IsNewUser)
                foreach (var item in user.UserRightUserMappings)
                    item.RefUserId = user.UserId;

            DataContext.Instance.UserRightUserMappings.UpdateOrInsert(user.UserRightUserMappings);
            RefreshUsers();

            return user;
        }

        public bool IsUserRightGranted(int userId, Permission permission)
        {
            var user = Users.SingleOrDefault(x => x.UserId == userId);
            if (user == null)
                return false;
            return IsUserRightGranted(user, permission);
        }

        public bool IsUserRightGranted(User user, Permission permission)
        {
            foreach (var right in user.UserRights.Keys)
                if (right.Permission == permission)
                    return user.UserRights[right];
            return false;
        }

        public SvenTechCollection<UserRightUserMappingFlatStructure> GetUserRightUserMappingFlatStructure(User user)
        {
            var UserRightUserMappingFlatStructure = new SvenTechCollection<UserRightUserMappingFlatStructure>();
            foreach (var item in user.UserRightUserMappings)
                UserRightUserMappingFlatStructure.Add(new UserRightUserMappingFlatStructure(item));
            return UserRightUserMappingFlatStructure;
        }

        public List<UserRightUserMapping> ConvertUserRightUserMappingFlatStructureToNormal(
            SvenTechCollection<UserRightUserMappingFlatStructure> UserRightUserMappingFlatStructure)
        {
            var userRightUserMappings = new List<UserRightUserMapping>();

            foreach (var item in UserRightUserMappingFlatStructure)
                userRightUserMappings.Add(new UserRightUserMapping(item.RefUserId, item.RefUserRightId,
                    item.IsGranted));

            return userRightUserMappings;
        }

        public void RefreshUsers()
        {
            Users = LoadUsersFromDB();
        }

        /// <summary>
        ///     Returns null if no user is found
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public User GetUserByNameAndPassword(string name, string password)
        {
            password = Encryption.ComputeHash(password, new SHA256CryptoServiceProvider(),
                new byte[]
                {
                    0x6c, 0xa6, 0x27, 0x0d, 0x62, 0xd4, 0x80, 0xc7, 0x50, 0xc9, 0x93, 0xef, 0xfb, 0x64, 0x90, 0x16,
                    0x7d, 0xc7, 0x1d, 0x6f, 0xb0, 0xe3, 0x80, 0xdc, 0x73
                });
            var user = DataContext.Instance.Users.GetUserByNameAndPassword(name, password);

            return user;
        }

        private List<UserRight> LoadUserRightsFromDB()
        {
            return DataContext.Instance.UserRights.GetAll();
        }

        public void GrantPermission(User user, Permission permission)
        {
            var right = Instance.UserRights.Single(x => x.Permission == permission);
            var tempUserRightUserMapping =
                user.UserRightUserMappings.SingleOrDefault(x => x.RefUserRightId == right.UserRightId);
            if (tempUserRightUserMapping != null)
            {
                tempUserRightUserMapping.IsGranted = true;
                DataContext.Instance.UserRightUserMappings.UpdateOrInsert(tempUserRightUserMapping);
            }
            else
            {
                DataContext.Instance.UserRightUserMappings.UpdateOrInsert(
                    new UserRightUserMapping(user.UserId, right.UserRightId, true));
            }
        }

        public void RevokePermission(User user, Permission permission)
        {
            var right = Instance.UserRights.Single(x => x.Permission == permission);
            var tempUserRightUserMapping =
                user.UserRightUserMappings.SingleOrDefault(x => x.RefUserRightId == right.UserRightId);
            if (tempUserRightUserMapping != null)
            {
                tempUserRightUserMapping.IsGranted = false;
                DataContext.Instance.UserRightUserMappings.UpdateOrInsert(tempUserRightUserMapping);
            }
            else
            {
                DataContext.Instance.UserRightUserMappings.UpdateOrInsert(
                    new UserRightUserMapping(user.UserId, right.UserRightId, false));
            }
        }

        #endregion Methods
    }
}