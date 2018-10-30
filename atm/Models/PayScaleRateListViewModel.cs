using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using atm.services.contracts;

namespace atm.Models
{
	public class PayScaleRateListViewModel : List<PayScaleRateViewModel>
	{

		public PayScaleRateListViewModel(IEnumerable<IPayScaleRate> collection)
		{
			foreach (var item in collection)
			{
				Add(new PayScaleRateViewModel(item));
			}
		}
	}
}