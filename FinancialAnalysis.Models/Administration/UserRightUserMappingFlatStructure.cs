using DevExpress.Mvvm;
using Newtonsoft.Json;
using System.Linq;

namespace FinancialAnalysis.Models.Administration
{
    /// <summary>
    /// Benutzerrechtzuordnung flache Struktur für TreeList
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class UserRightUserMappingFlatStructure : BindableBase
    {
        public UserRightUserMappingFlatStructure(User user, UserRight userRight, int userRightMappingId = 0)
        {
            if (userRightMappingId != 0)
            {
                UserRightUserMappingId = userRightMappingId;
            }

            RefUserId = user.UserId;
            IsGranted = user.UserRights.Single(x => x.UserRightId == userRight.UserRightId).IsGranted;
            RefUserRightId = userRight.UserRightId;
            ParentCategory = userRight.ParentCategory;
            Name = userRight.Name;
            Description = userRight.Description;
            HierachicalId = (int)userRight.Permission;
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