using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using atm.services.models;

namespace atm.Models
{
	public class PayRateViewModel
	{
		public SelectList CenterSelectList { get; set; }
		public SelectList PayScaleSelectList { get; set; }
	}
}