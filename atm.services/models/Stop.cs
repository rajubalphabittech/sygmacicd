using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace atm.services.models
{
	public class Stop : IStop
	{
        /// <summary>
        /// RouteStopId of the RouteStop record
        /// </summary>
        public int RoutePlanId { get; set; }

        /// <summary>
        /// SygmaCenterNo of the RouteStop record
        /// </summary>
        public int SygmaCenterNo { get; set; }

        /// <summary>
        /// RouteNumber of the RouteStop
        /// </summary>
        public string RouteNumber { get; set; }

        /// <summary>
        /// RouteId of the RouteStop
        /// </summary>
        public int RouteId { get; set; }

        /// <summary>
        /// RouteName of the RouteStop
        /// </summary>
        public string RouteName { get; set; }

        /// <summary>
        /// BillTo of the RouteStop
        /// </summary>
        public decimal BillTo { get; set; }

        /// <summary>
        /// ShipTo of the RouteStop
        /// </summary>
        public decimal ShipTo { get; set; }

        /// <summary>
        /// StopNumber of the RouteStop
        /// </summary>
        public long? StopNumber { get; set; }

        /// <summary>
        /// Actual arrival date of the RouteStop as retrieved by Telogis
        /// </summary>
        public DateTime? TelogisArrivalDeliveryDateTime { get; set; }

        /// <summary>
        /// PlannedDeliveryDateTime of the RouteStop
        /// </summary>
        public DateTime? PlannedDeliveryDateTime { get; set; }

        /// <summary>
        /// ScheduledDeliveryDateTime of the RouteStop
        /// </summary>
        public DateTime? ScheduledDeliveryDateTime { get; set; }

        /// <summary>
        /// AdjustedDeliveryDateTime of the RouteStop
        /// </summary>
        public DateTime? AdjustedDeliveryDateTime { get; set; }

        /// <summary>
        /// EffectiveDate of the RouteStop
        /// </summary>
        public DateTime EffectiveDate { get; set; }

        /// <summary>
        /// RouteStopTypeId of the RouteStop
        /// </summary>
        public int RouteStopTypeId { get; set; }

        /// <summary>
        /// IsActive of the RouteStop
        /// </summary>
        public byte? IsActive { get; set; }

        /// <summary>
        /// RecordCreatedDate of the RouteStop
        /// </summary>
        public DateTime? RecordCreatedDate { get; set; }

        /// <summary>
        /// LastUpdatedDate of the RouteStop
        /// </summary>
        public DateTime? LastUpdatedDate { get; set; }

        /// <summary>
        /// RecordCreatedDate of the RouteStop
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// EmailAddress of the RouteStop
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Phone of the RouteStop
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// RecordCreatedDate of the RouteStop
        /// </summary>
        public string Address1 { get; set; }

        /// <summary>
        /// Address2 of the RouteStop
        /// </summary>
        public string Address2 { get; set; }

        /// <summary>
        /// City of the RouteStop
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// State of the RouteStop
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Zip of the RouteStop
        /// </summary>
        public string Zip { get; set; }

        /// <summary>
        /// Longitude of the RouteStop's address
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// Latitude of the RouteStop's address
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// Weight of the RouteStop
        /// </summary>
        public decimal Weight { get; set; }

        /// <summary>
        /// WeightFreezer of the RouteStop
        /// </summary>
        public decimal WeightFreezer { get; set; }

        /// <summary>
        /// WeightCooler of the RouteStop
        /// </summary>
        public decimal WeightCooler { get; set; }

        /// <summary>
        /// WeightDry of the RouteStop
        /// </summary>
        public decimal WeightDry { get; set; }

        /// <summary>
        /// Cubes of the RouteStop
        /// </summary>
        public decimal Cubes { get; set; }

        /// <summary>
        /// CubesFreezer of the Route
        /// </summary>
        public decimal CubesFreezer { get; set; }

        /// <summary>
        /// CubesCooler of the Route
        /// </summary>
        public decimal CubesCooler { get; set; }

        /// <summary>
        /// CubesDry of the Route
        /// </summary>
        public decimal CubesDry { get; set; }

        /// <summary>
        /// Cases of the Route
        /// </summary>
        public decimal Cases { get; set; }

        /// <summary>
        /// CasesFreezer of the Route
        /// </summary>
        public decimal CasesFreezer { get; set; }

        /// <summary>
        /// CasesCooler of the Route
        /// </summary>
        public decimal CasesCooler { get; set; }

        /// <summary>
        /// CasesDry of the Route
        /// </summary>
        public decimal CasesDry { get; set; }

        /// <summary>
        /// ConceptId of the RouteStop's address
        /// </summary>
        public int ConceptId { get; set; }

        /// <summary>
        /// Concept of the RouteStop
        /// </summary>
        public string Concept { get; set; }

        /// <summary>
        /// ConceptCode of the RouteStop
        /// </summary>
        public string ConceptCode { get; set; }

        /// <summary>
        /// OntimeCalculationId of the RouteStop's address
        /// </summary>
        public int OntimeCalculationId { get; set; }

        /// <summary>
        /// Comment of the RouteStop
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// LastCustomerCommunicationComment of the RouteStop
        /// </summary>
        public string LastCustomerCommunicationComment { get; set; }

        /// <summary>
        /// RoutePlanModificationTypeId of the RouteStop
        /// </summary>
        public byte RoutePlanModificationTypeId { get; set; }

        /// <summary>
        /// AddOnOrderCount of the RouteStop
        /// </summary>
        public int AddOnOrderCount { get; set; }

        /// <summary>
        /// AddOnOrderCount of the RouteStop
        /// </summary>
        public int AS400Miles { get; set; }

        /// <summary>
        /// AddOnOrderCount of the RouteStop
        /// </summary>
        public int ATMMiles { get; set; }

        /// <summary>
        /// Proximity of the Stop
        /// </summary>
        public double Proximity { get; set; }

        /// <summary>
        /// CompassHeading of the Stop to another stop
        /// </summary>
        public string CompassHeading { get; set; }

        /// <summary>
        /// The first Route Id a Stop was assigned to. Useful for tracking a stop's history
        /// </summary>
        public int OriginalRouteId { get; set; }

        public string TimeZone { get; set; }

        public string OrderStatus { get; set; }
        public int EnableStopMove { get; set; }
        public int OrderId { get; set; }
        public byte DayNumber { get; set; }

        public int IsEarlyOrLate { get; set; }
        public MoveStatuses MoveStatus { get; set; }

        /// <summary>
        /// CurrentDriverName of the RouteStop
        /// </summary>
        public string CurrentDriverName { get; set; }

        /// <summary>
        /// StopStatus of the RouteStop
        /// </summary>
        public string StopStatus { get; set; }
    }
}