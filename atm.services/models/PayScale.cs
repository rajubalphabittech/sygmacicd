using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace atm.services.models
{
	public class PayScale
	{
		public int PayScaleId { get; set; }
		public string PayScaleDesignator { get; set; }
		public string PayScaleDescription { get; set; }
		public string PayScaleDisplay { get; set; }
		public int DisplayOrder { get; set; }
	}
}
