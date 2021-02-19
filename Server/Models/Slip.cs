using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlfalahApp.Server.Models
{
    public class Slip
    {
        public string PayDate { get; set; }
        public string FullName { get; set; }       
        public string Dept { get; set; }
        public string Term { get; set; }

        public decimal Basic { get; set; }
        public decimal Allowance { get; set; }
        public decimal Rent { get; set; }
        public decimal Transport { get; set; }
        public decimal Tax { get; set; }
                
        public decimal OtherEarning { get; set; }
        public decimal Iou { get; set; }
        public decimal Loans { get; set; }
        public decimal OtherDeduction { get; set; }
        public string Note { get; set; }

    }
}
