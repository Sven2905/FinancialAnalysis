using DevExpress.Mvvm;

namespace FinancialAnalysis.Models.Administration
{
    public class UserRightUserMappingFlatStructure : ViewModelBase
    {
        public UserRightUserMappingFlatStructure(UserRightUserMapping userRightUserMapping)
        {
            this.UserRightUserMappingId = userRightUserMapping.UserRightUserMappingId;
            this.RefUserId = userRightUserMapping.RefUserId;
            this.IsGranted = userRightUserMapping.IsGranted;
            this.RefUserRightId = userRightUserMapping.RefUserRightId;
            this.ParentCategory = userRightUserMapping.UserRight.ParentCategory;
            this.Name = userRightUserMapping.UserRight.Name;
            this.Description = userRightUserMapping.UserRight.Description;
            this.HierachicalId = (int)userRightUserMapping.UserRight.Permission;
        }

        public int UserRightUserMappingId { get; set; }
        public bool IsGranted { get; set; }
        public int RefUserId { get; set; }
        public int RefUserRightId { get; set; }
        public int ParentCategory { get; set; }
        public int HierachicalId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
