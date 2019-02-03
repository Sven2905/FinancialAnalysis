namespace FinancialAnalysis.Models.Administration
{
    public class UserRightUserMapping
    {
        public UserRightUserMapping()
        {
        }

        public UserRightUserMapping(int RefUserId, int RefUserRightId, bool IsGranted)
        {
            this.RefUserId = RefUserId;
            this.RefUserRightId = RefUserRightId;
            this.IsGranted = IsGranted;
        }

        public int UserRightUserMappingId { get; set; }
        public bool IsGranted { get; set; }
        public int RefUserId { get; set; }
        public User User { get; set; } = new User();
        public int RefUserRightId { get; set; }
        public UserRight UserRight { get; set; } = new UserRight();
    }
}