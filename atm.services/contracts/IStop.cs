using atm.services.models;
using System;
using System.ComponentModel.DataAnnotations;

namespace atm.services
{
    public interface IStop : ILocation
    {
        /// <summary>
        /// RouteStopId of the RouteStop record
        /// </summary>
        int RoutePlanId { get; set; }

        /// <summary>
        /// SygmaCenterNo of the RouteStop record
        /// </summary>
        int SygmaCenterNo { get; set; }

        /// <summary>
        /// RouteNumber of the RouteStop
        /// </summary>
        string RouteNumber { get; set; }

        /// <summary>
        /// RouteId of the RouteStop
        /// </summary>
        int RouteId { get; set; }

        /// <summary>
        /// RouteName of the RouteStop
        /// </summary>
        string RouteName { get; set; }

        /// <summary>
        /// BillTo of the RouteStop
        /// </summary>
        decimal BillTo { get; set; }

        /// <summary>
        /// ShipTo of the RouteStop
        /// </summary>
        decimal ShipTo { get; set; }

        /// <summary>
        /// ShipTo of the RouteStop
        /// </summary>
        long? StopNumber { get; set; }

        /// <summary>
        /// Actual arrival date of the RouteStop as retrieved by Telogis
        /// </summary>
        DateTime? TelogisArrivalDeliveryDateTime { get; set; }

        /// <summary>
        /// PlannedDeliveryDateTime of the RouteStop
        /// </summary>
        DateTime? PlannedDeliveryDateTime { get; set; }

        /// <summary>
        /// ScheduledDeliveryDateTime of the RouteStop
        /// </summary>
        DateTime? ScheduledDeliveryDateTime { get; set; }

        /// <summary>
        /// AdjustedDeliveryDateTime of the RouteStop
        /// </summary>
        DateTime? AdjustedDeliveryDateTime { get; set; }

        /// <summary>
        /// EffectiveDate of the RouteStop
        /// </summary>
        DateTime EffectiveDate { get; set; }

        /// <summary>
        /// RouteStopTypeId of the RouteStop
        /// </summary>
        int RouteStopTypeId { get; set; }

        /// <summary>
        /// IsActive of the RouteStop
        /// </summary>
        byte? IsActive { get; set; }

        /// <summary>
        /// RecordCreatedDate of the RouteStop
        /// </summary>
        DateTime? RecordCreatedDate { get; set; }

        /// <summary>
        /// LastUpdatedDate of the RouteStop
        /// </summary>
        DateTime? LastUpdatedDate { get; set; }

        /// <summary>
        /// RecordCreatedDate of the RouteStop
        /// </summary>
        string CustomerName { get; set; }

        /// <summary>
        /// EmailAddress of the RouteStop
        /// </summary>
        string EmailAddress { get; set; }

        /// <summary>
        /// Phone of the RouteStop
        /// </summary>
        string Phone { get; set; }

        /// <summary>
        /// RecordCreatedDate of the RouteStop
        /// </summary>
        string Address1 { get; set; }

        /// <summary>
        /// Address2 of the RouteStop
        /// </summary>
        string Address2 { get; set; }

        /// <summary>
        /// City of the RouteStop
        /// </summary>
        string City { get; set; }

        /// <summary>
        /// State of the RouteStop
        /// </summary>
        string State { get; set; }

        /// <summary>
        /// Zip of the RouteStop
        /// </summary>
        string Zip { get; set; }

        /// <summary>
        /// Weight of the RouteStop
        /// </summary>
        decimal Weight { get; set; }

        /// <summary>
        /// WeightFreezer of the RouteStop
        /// </summary>
        decimal WeightFreezer { get; set; }

        /// <summary>
        /// WeightCooler of the RouteStop
        /// </summary>
        decimal WeightCooler { get; set; }

        /// <summary>
        /// WeightDry of the RouteStop
        /// </summary>
        decimal WeightDry { get; set; }

        /// <summary>
        /// Cubes of the RouteStop
        /// </summary>
        decimal Cubes { get; set; }

        /// <summary>
        /// CubesFreezer of the Route
        /// </summary>
        decimal CubesFreezer { get; set; }

        /// <summary>
        /// CubesCooler of the Route
        /// </summary>
        decimal CubesCooler { get; set; }

        /// <summary>
        /// CubesDry of the Route
        /// </summary>
        decimal CubesDry { get; set; }

        /// <summary>
        /// Cases of the Route
        /// </summary>
        decimal Cases { get; set; }

        /// <summary>
        /// CasesFreezer of the Route
        /// </summary>
        decimal CasesFreezer { get; set; }

        /// <summary>
        /// CasesCooler of the Route
        /// </summary>
        decimal CasesCooler { get; set; }

        /// <summary>
        /// CasesDry of the Route
        /// </summary>
        decimal CasesDry { get; set; }

        /// <summary>
        /// OntimeCalculationId of the RouteStop's address
        /// </summary>
        int ConceptId { get; set; }

        /// <summary>
        /// Concept of the RouteStop
        /// </summary>
        string Concept { get; set; }

        /// <summary>
        /// ConceptCode of the RouteStop
        /// </summary>
        string ConceptCode { get; set; }

        /// <summary>
        /// OntimeCalculationId of the RouteStop's address
        /// </summary>
        int OntimeCalculationId { get; set; }

        /// <summary>
        /// Comment of the RouteStop
        /// </summary>
        string Comment { get; set; }

        /// <summary>
        /// LastCustomerCommunicationComment of the RouteStop
        /// </summary>
        string LastCustomerCommunicationComment { get; set; }

        /// <summary>
        /// RoutePlanModificationTypeId of the RouteStop
        /// </summary>
        byte RoutePlanModificationTypeId { get; set; }

        /// <summary>
        /// AddOnOrderCount of the RouteStop
        /// </summary>
        int AddOnOrderCount { get; set; }

        /// <summary>
        /// AddOnOrderCount of the RouteStop
        /// </summary>
        int AS400Miles { get; set; }

        /// <summary>
        /// AddOnOrderCount of the RouteStop
        /// </summary>
        int ATMMiles { get; set; }

        /// <summary>
        /// Proximity of the Stop
        /// </summary>
        double Proximity { get; set; }

        /// <summary>
        /// CompassHeading of the Stop to another stop
        /// </summary>
        string CompassHeading { get; set; }

        /// <summary>
        /// The first Route Id a Stop was assigned to. Useful for tracking a stop's history
        /// </summary>
        int OriginalRouteId { get; set; }

        string TimeZone { get; set; }
        string OrderStatus { get; set; }
        int EnableStopMove { get; set; }
        int OrderId { get; set; }
        byte DayNumber { get; set; }
        int IsEarlyOrLate { get; set; }
        MoveStatuses MoveStatus { get; set; }

        /// <summary>
        /// CurrentDriverName of the RouteStop
        /// </summary>
        string CurrentDriverName { get; set; }

        /// <summary>
        /// StopStatus of the RouteStop
        /// </summary>
        string StopStatus { get; set; }
    }    
}