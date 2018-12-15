using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace FinancialAnalysis.Models.Administration
{
    public class User
    {
        private string _Password = string.Empty;

        public int UserId { get; set; }
        public string LoginUser { get; set; }

        public string Password
        {
            get
            {
                if (_Password != string.Empty)
                {
                    return Encryption.DecryptText(_Password, @"LTF*'qnt6\$gtQM:?W7~");
                }
                return "";
            }
            set
            {
                if (value != "")
                    _Password = Encryption.EncryptText(value, @"LTF*'qnt6\$gtQM:?W7~");
            }
        }
        public byte[] Picture { get; set; }
        public string Mail { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        /// <summary>
        /// Kürzel
        /// </summary>
        public string Contraction { get; set; }
        public string Name { get { return Firstname + " " + Lastname; } }
    }
}
