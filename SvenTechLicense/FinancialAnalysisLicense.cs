using License;
using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Licenses
{
    public class FinancialAnalysisLicense : LicenseEntity
    {
        [DisplayName("Time Management")]
        [Category("License Options")]
        [XmlElement("TimeManagement")]
        [ShowInLicenseInfo(true, "Time Management", ShowInLicenseInfoAttribute.FormatType.String)]
        public bool TimeManagement { get; set; }

        [DisplayName("Sales Management")]
        [Category("License Options")]
        [XmlElement("SalesManagement")]
        [ShowInLicenseInfo(true, "Sales Management", ShowInLicenseInfoAttribute.FormatType.String)]
        public bool SalesManagement { get; set; }


        [DisplayName("Warehouse Management")]
        [Category("License Options")]
        [XmlElement("WarehouseManagement")]
        [ShowInLicenseInfo(true, "Warehouse Management", ShowInLicenseInfoAttribute.FormatType.String)]
        public bool WarehouseManagement { get; set; }

        [DisplayName("Expiry Date")]
        [Category("License Options")]
        [XmlElement("ExpiryDate")]
        [ShowInLicenseInfo(true, "ExpiryDate", ShowInLicenseInfoAttribute.FormatType.Date)]
        public DateTime ExpiryDate { get; set; } = DateTime.Now;

        public FinancialAnalysisLicense()
        {
            this.AppName = "FinancialAnalysis";
        }

        public override LicenseStatus DoExtraValidation(out string validationMsg)
        {
            LicenseStatus _licStatus = LicenseStatus.UNDEFINED;
            validationMsg = string.Empty;

            switch (this.Type)
            {
                case LicenseTypes.Single:
                    //For Single License, check whether UID is matched
                    if (this.UID == LicenseHandler.GenerateUID(this.AppName))
                    {
                        _licStatus = LicenseStatus.VALID;
                    }
                    else
                    {
                        validationMsg = "The license is NOT for this copy!";
                        _licStatus = LicenseStatus.INVALID;
                    }
                    break;
                case LicenseTypes.Volume:
                    //No UID checking for Volume License
                    _licStatus = LicenseStatus.VALID;
                    break;
                default:
                    validationMsg = "Invalid license";
                    _licStatus = LicenseStatus.INVALID;
                    break;
            }

            return _licStatus;
        }
    }
}

