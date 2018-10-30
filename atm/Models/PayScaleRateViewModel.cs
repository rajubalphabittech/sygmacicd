using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using atm.services.contracts;

namespace atm.Models
{
	public class PayScaleRateViewModel : IPayScaleRate
	{
		public PayScaleRateViewModel(IPayScaleRate payScaleRate)
		{
			RateTypeId = payScaleRate.RateTypeId;
			RateTypeDescription = payScaleRate.RateTypeDescription;
			Rate = payScaleRate.Rate;
			IsEnabled = payScaleRate.IsEnabled;
			IncludeInBase = payScaleRate.IncludeInBase;
			ProgressionRateApplies = payScaleRate.ProgressionRateApplies;
		}

		public int RateTypeId { get; set; }

		[DisplayName("Rate Type")]
		public string RateTypeDescription { get; set; }

		[DisplayName("Rate")]
		[DisplayFormat(DataFormatString = "{0:C}")]
		public decimal? Rate { get; set; }

		[DisplayName("Enabled")]
		public bool IsEnabled { get; set; }

		[DisplayName("Base")]
		public bool IncludeInBase { get; set; }

		[DisplayName("Apply Prog. Rate")]
		public bool ProgressionRateApplies { get; set; }

		[DisplayName("Guaranteed Pay")]
		public bool IncludeInGuaranteedPay { get; set; }
	}
}