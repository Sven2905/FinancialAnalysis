using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.Administration
{
    public class UserRight
    {
        public UserRight()
        {

        }

        public UserRight(Permission Permission, string Name, string Description = "")
        {
            this.Permission = Permission;
            this.Name = Name;
            this.Description = Description;
        }

        public int UserRightId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Permission Permission { get; set; }
    }
}
