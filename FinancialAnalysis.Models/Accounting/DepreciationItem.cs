using DevExpress.Mvvm;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.Accounting
{
    [JsonObject(MemberSerialization.OptOut)]
    public class DepreciationItem : BindableBase
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Fields

        private int _Years;
        private decimal _InitialValue;
        private decimal _AssetValue;
        private DepreciationType _DepreciationType;

        #endregion Fields

        #region Properties

        public int DepreciationItemId { get; set; }
        public string Name { get; set; }

        public int Years
        {
            get { return _Years; }
            set { _Years = value; OnPropertyChanged(nameof(Years)); }
        }

        public decimal InitialValue
        {
            get { return _InitialValue; }
            set { _InitialValue = value; OnPropertyChanged(nameof(InitialValue)); }
        }

        public decimal AssetValue
        {
            get { return _AssetValue; }
            set { _AssetValue = value; OnPropertyChanged(nameof(AssetValue)); }
        }

        public DepreciationType DepreciationType
        {
            get { return _DepreciationType; }
            set { _DepreciationType = value; OnPropertyChanged(nameof(DepreciationType)); }
        }

        public int StartYear { get; set; }
        public bool IsDepreciated { get; set; }

        #endregion Properties
    }
}
