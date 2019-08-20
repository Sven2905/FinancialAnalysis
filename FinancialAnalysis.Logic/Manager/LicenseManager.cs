using License;
using Licenses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinancialAnalysis.Logic.Manager
{
    public class LicenseManager
    {
        #region Constructor

        public LicenseManager()
        {

            AppName = "FinancialAnalysis";
            LicenseObjectType = typeof(Licenses.FinancialAnalysisLicense);
        }

        #endregion Constructor

        #region Fields

        byte[] _certPubicKeyData;

        #endregion Fields

        #region Properties

        public static LicenseManager Instance { get; } = new LicenseManager();
        //public string AppName { get; set; }
        public Type LicenseObjectType { get; set; }
        public string UID { get; set; }
        public string License { get; set; }
        public string LicenseBASE64String
        {
            get
            {
                return License.Trim();
            }
        }

        private string appName;

        public string AppName
        {
            get { return appName; }
            private set { appName = value; UID = LicenseHandler.GenerateUID(appName); }
        }

        #endregion Properties

        #region Methods

        private bool ValidateLicense()
        {
            if (string.IsNullOrWhiteSpace(License))
            {
                MessageBox.Show("Please input license", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            //Check the activation string
            LicenseStatus _licStatus = LicenseStatus.UNDEFINED;
            string _msg = string.Empty;
            LicenseEntity _lic = LicenseHandler.ParseLicenseFromBASE64String(LicenseObjectType, License.Trim(), _certPubicKeyData, out _licStatus, out _msg);
            switch (_licStatus)
            {
                case LicenseStatus.VALID:
                        MessageBox.Show(_msg, "License is valid", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;

                case LicenseStatus.CRACKED:
                case LicenseStatus.INVALID:
                case LicenseStatus.UNDEFINED:
                        MessageBox.Show(_msg, "License is INVALID", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return false;

                default:
                    return false;
            }

        }

        public bool Validation()
        {
            //Call license control to validate the license string
            if (ValidateLicense())
            {
                //If license if valid, save the license string into a local file
                File.WriteAllText(Path.Combine(Application.StartupPath, "license.lic"), LicenseManager.Instance.LicenseBASE64String);

                MessageBox.Show("Lizenz akzeptiert. Die Applikation wird neugestartet.", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Restart();
                return true;
            }
            return false;
        }

        public void Initialize()
        {
            //Initialize variables with default values
            FinancialAnalysisLicense _lic = null;
            string _msg = string.Empty;
            LicenseStatus _status = LicenseStatus.UNDEFINED;

            //Read public key from assembly
            Assembly _assembly = Assembly.GetExecutingAssembly();
            using (MemoryStream _mem = new MemoryStream())
            {
                _assembly.GetManifestResourceStream("FinancialAnalysis.Logic.LicenseVerify.cer").CopyTo(_mem);

                _certPubicKeyData = _mem.ToArray();
            }

            //Check if the XML license file exists
            if (File.Exists("license.lic"))
            {
                _lic = (FinancialAnalysisLicense)LicenseHandler.ParseLicenseFromBASE64String(
                    typeof(FinancialAnalysisLicense),
                    File.ReadAllText("license.lic"),
                    _certPubicKeyData,
                    out _status,
                    out _msg);
                Globals.ActiveLicense = _lic;
            }
            else
            {
                _status = LicenseStatus.INVALID;
                _msg = "Your copy of this application is not activated";
            }

            if (_status == LicenseStatus.VALID)
            {
                //TODO: If license is valid, you can do extra checking here
                //TODO: E.g., check license  if you have added expiry date property to your license entity
                //TODO: Also, you can set feature switch here based on the different properties you added to your license entity 

                //Here for demo, just show the license information and RETURN without additional checking       
                //licInfo.ShowLicenseInfo(_lic);

                return;
            }
        }

        #endregion Methods
    }
}
