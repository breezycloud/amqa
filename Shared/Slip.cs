using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlfalahApp.Shared
{
    public class Slip
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Dept { get; set; }
        public decimal Basic { get; set; }
        public decimal Allowance { get; set; }
        public decimal Rent { get; set; }
        public decimal Transport { get; set; }
        public decimal Tax { get; set; }

        private string _fullName;

        public string FullName
        {
            get { return _fullName; }
            set { _fullName = $"{FirstName} {LastName}"; }
        }
        public DateTime PayDate { get; set; }
        public decimal OtherEarning { get; set; }
        public decimal Iou { get; set; }
        public decimal Loans { get; set; }
        public decimal OtherDeduction { get; set; }
        public string Note { get; set; }

    }
}
