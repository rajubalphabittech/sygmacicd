using atm.services.contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace atm.services.models
{

    public class PayScaleRate : IPayScaleRate
    {
        public int RateTypeId { get; set; }
        public string RateTypeDescription { get; set; }
        public decimal? Rate { get; set; }
        public bool IsEnabled { get; set; }
        public bool IncludeInBase { get; set; }
        public bool ProgressionRateApplies { get; set; }
        public bool IncludeInGuaranteedPay { get; set; }
    }
}
