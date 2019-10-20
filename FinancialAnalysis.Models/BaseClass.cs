using DevExpress.Mvvm;
using FinancialAnalysis.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models
{
    public class BaseClass : BindableBase, IDeleted, IEditable, IAuditable, IDeletable, IVisible
    {
        public bool IsDeleted { get; set; } = false;
        public bool IsDeletable { get; set; } = true;
        public bool IsEditable { get; set; } = true;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime UpdatedOn { get; set; } = DateTime.Now;
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public bool IsVisible { get; set; } = true;
    }
}
