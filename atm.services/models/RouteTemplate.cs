using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace atm.services.models
{
	//[Table("routes")]
	public class Route
	{
		//[Key]
        //public int RouteId { get; set; }
		//[NotMapped]
        public string RouteName { get; set; }
		public int SygmaCenterNo { get; set; }
		public string RouteNumber { get; set; }
		public DateTime RouteDate { get; set; }

		public virtual List<Stop> Stops { get; set; }
	}
}