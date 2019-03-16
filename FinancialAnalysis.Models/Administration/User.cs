using DevExpress.Mvvm;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Security.Cryptography;
using Utilities;

namespace FinancialAnalysis.Models.Administration
{
    /// <summary>
    /// Benutzer
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class User : BindableBase
    {
        #region Properties

        /// <summary>
        /// Id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Login-Name
        /// </summary>
        public string LoginUser { get; set; }

        /// <summary>
        /// Passwort
        /// </summary>
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
                    _Password = Encryption.ComputeHash(value, new SHA256CryptoServiceProvider(),
                        new byte[]
                        {
                            0x6c, 0xa6, 0x27, 0x0d, 0x62, 0xd4, 0x80, 0xc7, 0x50, 0xc9, 0x93, 0xef, 0xfb, 0x64, 0x90,
                            0x16, 0x7d, 0xc7, 0x1d, 0x6f, 0xb0, 0xe3, 0x80, 0xdc, 0x73
                        });
                }
                else
                {
                    _Password = "";
                }
            }
        }

        /// <summary>
        /// Daten des Benutzerbild
        /// </summary>
        public byte[] Picture { get; set; }

        /// <summary>
        /// Mailadresse
        /// </summary>
        public string Mail { get; set; }

        /// <summary>
        /// Gibt an, ob das Konto aktiviert ist
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Gibt an, ob der Benutzer Administrator ist
        /// </summary>
        public bool IsAdministrator { get; set; } = false;

        /// <summary>
        /// Kürzel
        /// </summary>
        public string Contraction { get; set; }

        /// <summary>
        /// Ausgabe: Vorname Nachname
        /// </summary>
        public string Name => Firstname + " " + Lastname;

        /// <summary>
        /// Vorname
        /// </summary>
        public string Firstname
        {
            get => _Firstname;
            set
            {
                _Firstname = value;
                RaisePropertyChanged("Name");
            }
        }

        /// <summary>
        /// Nachname
        /// </summary>
        public string Lastname
        {
            get => _Lastname;
            set
            {
                _Lastname = value;
                RaisePropertyChanged("Name");
            }
        }

        /// <summary>
        /// Zugeordnete Rechte
        /// </summary>
        public Dictionary<UserRight, bool> UserRights { get; set; } = new Dictionary<UserRight, bool>();

        /// <summary>
        /// Benutzer-Rechte-Zuordnung
        /// </summary>
        public List<UserRightUserMapping> UserRightUserMappings { get; set; } = new List<UserRightUserMapping>();

        /// <summary>
        /// Initialien
        /// </summary>
        public string Initials
        {
            get
            {
                if (!string.IsNullOrEmpty(Firstname) && !string.IsNullOrEmpty(Lastname))
                {
                    return Firstname[0] + Lastname[0].ToString();
                }

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