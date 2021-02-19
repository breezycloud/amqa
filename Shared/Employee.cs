using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlfalahApp.Shared
{
    public class Employee
    {
        public Employee() => PaySlips = new List<PaySlip>();

        public Guid Id { get; set; }
        public string EName { get; set; }
        public string Gender { get; set; }
        public string Dept { get; set; }
        public string PhoneNumber1 { get; set; }
        public string PhoneNumber2 { get; set; }
        public bool Status { get; set; }
        public decimal Basic { get; set; }
        public decimal Allowance { get; set; }
        public decimal Rent { get; set; }
        public decimal Transport { get; set; }
        public decimal Tax { get; set; }
     
        private List<PaySlip> PaySlips { get; set; }
    }
}
