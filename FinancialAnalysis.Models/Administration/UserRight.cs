using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.Administration
{
    public class UserRight : ViewModelBase
    {
        public UserRight()
        {

        }

        public UserRight(Permission Permission, string Name, int ParentCategory = 0, string Description = "")
        {
            this.Permission = Permission;
            this.Name = Name;
            this.Description = Description;
            this.ParentCategory = ParentCategory;
        }

        public int UserRightId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Permission Permission { get; set; }
        public int ParentCategory { get; set; }
    }
}
