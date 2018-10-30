using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace atm.services.contracts
{
    public interface IPayScaleRate
    {
        int RateTypeId { get; set; }
        string RateTypeDescription { get; set; }
        decimal? Rate { get; set; }
        bool IsEnabled { get; set; }
        bool IncludeInBase { get; set; }
        bool ProgressionRateApplies { get; set; }
        bool IncludeInGuaranteedPay { get; set; }
    }
}
