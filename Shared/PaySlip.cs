using System;
using System.Collections.Generic;

#nullable disable

namespace AlfalahApp.Shared
{
    public partial class PaySlip
    {
        public Guid Id { get; set; }
        public Guid Empid { get; set; }
        public DateTime Paydate { get; set; }
        public string AcdSession { get; set; }
        public string Term { get; set; }
        public decimal OtherEarning { get; set; }
        public decimal Iou { get; set; }
        public decimal Loans { get; set; }
        public decimal OtherDeduction { get; set; }
        public string Note { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
