using DevExpress.Mvvm;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Utilities;

namespace FinancialAnalysis.Models.Accounting
{
    /// <summary>
    /// Aufteilungsanteile auf Kostenstelle
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class FixedCostAllocation : BindableBase
    {
        /// <summary>
        /// Id
        /// </summary>
        public int FixedCostAllocationId { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        public List<FixedCostAllocationDetail> FixedCostAllocationDetails { get; set; } = new List<FixedCostAllocationDetail>();

        public string DisplayAllDetails
        {
            get
            {
                return Name + " (" + DisplayCostCenters + " mit " + DisplayShares +")";
            }
        }

        public double[] Shares
        {
            get
            {
                if (FixedCostAllocationDetails?.Count > 0)
                {
                    return FixedCostAllocationDetails.Select(x => x.Shares).ToArray();
                }
                else
                {
                    return new double[0];
                }
            }
        }

        public string DisplayShares
        {
            get
            {
                if (FixedCostAllocationDetails?.Count > 0)
                {
                    string result = string.Empty;
                    foreach (var share in Shares)
                    {
                        result += share + ":";
                    }
                    return result.Remove(result.Length - 1, 1);
                }
                else
                {
                    return "1";
                }
            }
        }

        public string DisplayCostCenters
        {
            get
            {
                if (FixedCostAllocationDetails?.Count > 0)
                {
                    string result = string.Empty;
                    foreach (var detail in FixedCostAllocationDetails)
                    {
                        result += detail.CostCenter.Name + ":";
                    }
                    return result.Remove(result.Length - 1, 1);
                }
                else
                {
                    return "-";
                }
            }
        }
    }
}