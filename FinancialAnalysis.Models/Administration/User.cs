using DevExpress.Mvvm.DataAnnotations;
using System.ComponentModel;
using System.Security.Cryptography;
using Utilities;
using DevExpress.Mvvm;
using System.Collections.Generic;

namespace FinancialAnalysis.Models.Administration
{
    public class User : ViewModelBase
    {
        #region Properties
        public int UserId { get; set; }
        public string LoginUser { get; set; }
        public string Password
        {
            get
            {
                if (_Password != string.Empty)
                {
                    return _Password;
                }
                return "";
            }
            set
            {
                if (value != "")
                {
                    _Password = Encryption.ComputeHash(value, new SHA256CryptoServiceProvider(), new byte[] { 0x6c, 0xa6, 0x27, 0x0d, 0x62, 0xd4, 0x80, 0xc7, 0x50, 0xc9, 0x93, 0xef, 0xfb, 0x64, 0x90, 0x16, 0x7d, 0xc7, 0x1d, 0x6f, 0xb0, 0xe3, 0x80, 0xdc, 0x73 });
                }
            }
        }
        public byte[] Picture { get; set; }
        public string Mail { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsAdministrator { get; set; } = false;
        public string Contraction { get; set; }
        public string Name { get { return Firstname + " " + Lastname; } }
        public string Firstname
        {
            get { return _Firstname; }
            set { _Firstname = value; RaisePropertyChanged("Name"); }
        }
        public string Lastname
        {
            get { return _Lastname; }
            set { _Lastname = value; RaisePropertyChanged("Name"); }
        }
        public Dictionary<UserRight, bool> UserRights { get; set; } = new Dictionary<UserRight, bool>();

        /// <summary>
        /// Kürzel
        /// </summary>
        public string Initials
        {
            get
            {
                if (!string.IsNullOrEmpty(Firstname) && !string.IsNullOrEmpty(Lastname))
                {
                    return Firstname[0].ToString() + Lastname[0].ToString();
                }
                else
                {
                    return "";
                }
            }
        }
        #endregion Properties

        #region Fields
        private string _Firstname;
        private string _Lastname;
        private string _Password = string.Empty;
        #endregion Fields

        #region Methods
        public bool IsUserRightGranted(Permission permission)
        {
            foreach (var right in UserRights.Keys)
            {
                if (right.Permission == permission)
                {
                    return UserRights[right];
                }
            }
            return false;
        }
        #endregion Methods
    }
}
