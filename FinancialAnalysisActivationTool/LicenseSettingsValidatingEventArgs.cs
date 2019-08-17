using License;
using System;

namespace FinancialAnalysisActivationTool
{
    public class LicenseSettingsValidatingEventArgs:EventArgs
    {
        public LicenseEntity License { get; set; }
        public bool CancelGenerating { get; set; }
    }
}
