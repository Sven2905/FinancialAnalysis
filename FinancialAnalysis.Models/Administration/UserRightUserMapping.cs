﻿using DevExpress.Mvvm;
using Newtonsoft.Json;

namespace FinancialAnalysis.Models.Administration
{
    /// <summary>
    /// Benutzerrechtzuordnung
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class UserRightUserMapping : BaseClass
    {
        public UserRightUserMapping()
        {
        }

        public UserRightUserMapping(int RefUserId, int RefUserRightId, bool IsGranted, int UserRightUserMappingId)
        {
            this.RefUserId = RefUserId;
            this.RefUserRightId = RefUserRightId;
            this.IsGranted = IsGranted;
            this.UserRightUserMappingId = UserRightUserMappingId;
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
        /// Referenz-Id Benutzerrecht
        /// </summary>
        public int RefUserRightId { get; set; }
    }
}