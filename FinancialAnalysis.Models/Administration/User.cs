using System.Collections.Generic;
using System.Security.Cryptography;
using DevExpress.Mvvm;
using Utilities;

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
                if (_Password != string.Empty) return _Password;
                return "";
            }
            set
            {
                if (value != "")
                    _Password = Encryption.ComputeHash(value, new SHA256CryptoServiceProvider(),
                        new byte[]
                        {
                            0x6c, 0xa6, 0x27, 0x0d, 0x62, 0xd4, 0x80, 0xc7, 0x50, 0xc9, 0x93, 0xef, 0xfb, 0x64, 0x90,
                            0x16, 0x7d, 0xc7, 0x1d, 0x6f, 0xb0, 0xe3, 0x80, 0xdc, 0x73
                        });
                else
                    _Password = "";
            }
        }

        public byte[] Picture { get; set; }
        public string Mail { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsAdministrator { get; set; } = false;
        public string Contraction { get; set; }
        public string Name => Firstname + " " + Lastname;

        public string Firstname
        {
            get => _Firstname;
            set
            {
                _Firstname = value;
                RaisePropertyChanged("Name");
            }
        }

        public string Lastname
        {
            get => _Lastname;
            set
            {
                _Lastname = value;
                RaisePropertyChanged("Name");
            }
        }

        public Dictionary<UserRight, bool> UserRights { get; set; } = new Dictionary<UserRight, bool>();
        public List<UserRightUserMapping> UserRightUserMappings { get; set; } = new List<UserRightUserMapping>();

        /// <summary>
        ///     Kürzel
        /// </summary>
        public string Initials
        {
            get
            {
                if (!string.IsNullOrEmpty(Firstname) && !string.IsNullOrEmpty(Lastname))
                    return Firstname[0] + Lastname[0].ToString();
                return "";
            }
        }

        #endregion Properties

        #region Fields

        private string _Firstname;
        private string _Lastname;
        private string _Password = string.Empty;

        #endregion Fields
    }
}