using Newtonsoft.Json;

namespace FinancialAnalysis.Models.CarPoolManagement
{
    [JsonObject(MemberSerialization.OptOut)]
    public class CarModelBodyMapping
    {
        public CarModelBodyMapping()
        {

        }

        public CarModelBodyMapping(int RefCarModelId, int RefCarBodyId)
        {
            this.RefCarBodyId = RefCarBodyId;
            this.RefCarModelId = RefCarModelId;
        }

        public int CarModelBodyMappingId { get; set; }
        public int RefCarModelId { get; set; }
        public int RefCarBodyId { get; set; }
    }
}
