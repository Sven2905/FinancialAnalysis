namespace FinancialAnalysis.Models.Administration
{
    /// <summary>
    /// Benutzerrechtzuordnung
    /// </summary>
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

        /// <summary>
        /// Id
        /// </summary>
        public int UserRightUserMappingId { get; set; }

        /// <summary>
        /// Gibt an, ob der Benutzer berechtigt ist
        /// </summary>
        public bool IsGranted { get; set; }

        /// <summary>
        /// Referenz-Id des Benutzers
        /// </summary>
        public int RefUserId { get; set; }

        /// <summary>
        /// Benutzer
        /// </summary>
        public User User { get; set; } = new User();

        /// <summary>
        /// Referenz-Id Benutzerrecht
        /// </summary>
        public int RefUserRightId { get; set; }

        /// <summary>
        /// Benutzerrecht
        /// </summary>
        public UserRight UserRight { get; set; } = new UserRight();
    }
}