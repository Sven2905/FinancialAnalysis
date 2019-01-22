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
        #region Fields

        private readonly static UserManager instance = new UserManager();
        private SvenTechCollection<User> _Users;

        #endregion Fields

        #region Properties

        public static UserManager Instance => instance;
        public List<UserRight> UserRights
        {
            get
            {
                using (var db = new DataLayer())
                {
                    return db.UserRights.GetAll();
                }
            }
        }

        #endregion Properties

        #region Constructor

        private UserManager()
        {
            Users = LoadUsersFromDB();
        }

        #endregion Constructor

        #region Methods

        public SvenTechCollection<User> Users
        {
            get { return _Users; }
            set { _Users = value; }
        }

        private SvenTechCollection<User> LoadUsersFromDB()
        {
            SvenTechCollection<User> allUsers = new SvenTechCollection<User>();
            try
            {
                using (var db = new DataLayer())
                {
                    allUsers = db.Users.GetAll().ToSvenTechCollection();
                }
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
                using (var db = new DataLayer())
                {
                    db.Users.Delete(user.UserId);
                }
                _Users.Remove(user);

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
                        using (var db = new DataLayer())
                        {
                            db.Users.UpdatePassword(user);
                        }
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(user.Password))
                    {
                        throw new ArgumentException("Password is not set!");
                    }
                }
                using (var db = new DataLayer())
                {
                    user = db.Users.UpdateOrInsert(user);
                    db.UserRightUserMappings.UpdateOrInsert(user.UserRightUserMappings);
                }

                _Users.Add(user);
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

        /// <summary>
        /// Returns null if no user is found
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public User GetUserByNameAndPassword(string name, string password)
        {
            User user;
            password = Encryption.ComputeHash(password, new SHA256CryptoServiceProvider(), new byte[] { 0x6c, 0xa6, 0x27, 0x0d, 0x62, 0xd4, 0x80, 0xc7, 0x50, 0xc9, 0x93, 0xef, 0xfb, 0x64, 0x90, 0x16, 0x7d, 0xc7, 0x1d, 0x6f, 0xb0, 0xe3, 0x80, 0xdc, 0x73 });

            using (var db = new DataLayer())
            {
                user = db.Users.GetUserByNameAndPassword(name, password);
            }

            return user;
        }

        #endregion Methods
    }
}
