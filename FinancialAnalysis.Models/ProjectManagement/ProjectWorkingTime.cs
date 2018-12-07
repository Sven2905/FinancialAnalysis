﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.ProjectManagement
{
    public class ProjectWorkingTime
    {
        public int ProjectWorkingTimeId { get; set; }
        public int RefEmployeeId { get; set; }
        public virtual Employee Employee { get; set; }
        public int RefProjectId { get; set; }
        public virtual Project Project { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
