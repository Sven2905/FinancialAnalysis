using DevExpress.Mvvm;

namespace FinancialAnalysis.Models.Administration
{
    /// <summary>
    /// Benutzerrechtzuordnung flache Struktur für TreeList
    /// </summary>
    public class UserRightUserMappingFlatStructure : ViewModelBase
    {
        public UserRightUserMappingFlatStructure(UserRightUserMapping userRightUserMapping)
        {
            UserRightUserMappingId = userRightUserMapping.UserRightUserMappingId;
            RefUserId = userRightUserMapping.RefUserId;
            IsGranted = userRightUserMapping.IsGranted;
            RefUserRightId = userRightUserMapping.RefUserRightId;
            ParentCategory = userRightUserMapping.UserRight.ParentCategory;
            Name = userRightUserMapping.UserRight.Name;
            Description = userRightUserMapping.UserRight.Description;
            HierachicalId = (int)userRightUserMapping.UserRight.Permission;
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