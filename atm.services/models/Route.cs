using System;
using System.Collections.Generic;

namespace atm.services.models
{
    public class Route
    {
        public int RouteId { get; set; }
        public string RouteName { get; set; }
        public int SygmaCenterNo { get; set; }
        public string RouteNumber { get; set; }
        public int NumberOfStops { get; set; }
        public decimal TotalHours { get; set; }
        public int TotalAS400Miles { get; set; }
        public int TotalATMMiles { get; set; }

        public decimal TotalWeight { get; set; }
        public decimal TotalWeightFreezer { get; set; }
        public decimal TotalWeightCooler { get; set; }
        public decimal TotalWeightDry { get; set; }
        public decimal TotalCubes { get; set; }
        public decimal TotalCubesFreezer { get; set; }
        public decimal TotalCubesCooler { get; set; }
        public decimal TotalCubesDry { get; set; }
        public decimal TotalCases { get; set; }
        public decimal TotalCasesFreezer { get; set; }
        public decimal TotalCasesCooler { get; set; }
        public decimal TotalCasesDry { get; set; }

        public DateTime? AdjustedDispatchTime { get; set; }
        public DateTime? ScheduledDispatchTime { get; set; }
        public DateTime? PlannedDispatchTime { get; set; }

        public bool RouteHasStopAdditions { get; set; }
        public bool RouteHasStopRemovals { get; set; }
        public bool RouteHasStopTimeChanges { get; set; }
        public bool RouteWasModified { get; set; }
        public double AverageLongitude { get; set; }
        public double AverageLatitude { get; set; }
        public double Proximity { get; set; }
        public string CompassHeading { get; set; }
        public string NearRouteName { get; set; }
        public string NearRouteNumber { get; set; }
        public int NearRouteId { get; set; }
        public int NearRoutePlanId { get; set; }
        public int NearStopNumber { get; set; }
        public int ClosestStopRoutePlanId { get; set; }
        public double ClosestStopProximity { get; set; }
        public string ClosestStopCompassHeading { get; set; }
        public Stop ClosestStop { get; set; }
        public IEnumerable<int> ConceptIds { get; set; }
        public IEnumerable<string> Concepts { get; set; }
        public int IsEarlyOrLate { get; set; }
        public RouteOrders HasOrders { get; set; }
        public int NumberOfOrders { get; set; }
        public bool HasDispatched { get; set; }

        public string PrimaryDriverName { get; set; }
        public string SecondaryDriverName { get; set; }
        public string HelperName { get; set; }
        public int TruckId { get; set; }
        public int TrailerId { get; set; }

        public virtual IEnumerable<Stop> RouteStops { get; set; }
    }

    public class UpdateRouteWithStops
    {
        /// <summary>
        /// SygmaCenterNo of the UpdateRouteWithStops
        /// </summary>
        public int SygmaCenterNo { get; set; }

        /// <summary>
        /// RouteNumber of the UpdateRouteWithStops
        /// </summary>
        public string RouteNumber { get; set; }

        /// <summary>
        /// RouteNumber of the UpdateRouteWithStops
        /// </summary>
        public int RouteId { get; set; }

        /// <summary>
        /// RouteName of the UpdateRouteWithStops
        /// </summary>
        public string RouteName { get; set; }

        /// <summary>
        /// RouteStops of the Route
        /// </summary>
        public IEnumerable<UpdateStop> RouteStops { get; set; }
    }

    public class UpdateStop
    {
        /// <summary>
        /// RoutePlanId of the UpdateStop record
        /// </summary>
        public int RoutePlanId { get; set; }

        /// <summary>
        /// BillTo of the UpdateStop
        /// </summary>
        public decimal BillTo { get; set; }

        /// <summary>
        /// ShipTo of the UpdateStop
        /// </summary>
        public decimal ShipTo { get; set; }

        /// <summary>
        /// ScheduledDeliveryDateTime of the UpdateStop
        /// </summary>
        public DateTime? ScheduledDeliveryDateTime { get; set; }

        /// <summary>
        /// AdjustedDeliveryDateTime of the UpdateStop
        /// </summary>
        public DateTime? AdjustedDeliveryDateTime { get; set; }

        /// <summary>
        /// EarlyLate of the UpdateStop
        /// </summary>
        public string EarlyLate { get; set; }

        /// <summary>
        /// RoutePlanModificationTypeId of the UpdateStop
        /// </summary>
        public int RoutePlanModificationTypeId { get; set; }

        /// <summary>
        /// RouteNumber of the UpdateStop
        /// </summary>
        public string RouteNumber { get; set; }

        /// <summary>
        /// RouteNumber of the UpdateStop
        /// </summary>
        public int RouteId { get; set; }

        /// <summary>
        /// AdjustedRouteNumber of the UpdateStop
        /// </summary>
        public string AdjustedRouteNumber { get; set; }

        /// <summary>
        /// StopNumber of the UpdateStop
        /// </summary>
        public int StopNumber { get; set; }

        /// <summary>
        /// AdjustedStopNumber of the UpdateStop
        /// </summary>
        public int AdjustedStopNumber { get; set; }

        /// <summary>
        /// Comment of the UpdateStop
        /// </summary>
        public string Comment { get; set; }

        public bool IncludesStopAdjustedDateTimeChanges { get; set; }
        public bool IncludesStopScheduledDateTimeChanges { get; set; }
        public bool IncludesStopChanges { get; set; }
    }

    /// <summary>
    /// SearchRoute 
    /// </summary>
    public class SearchRoute
    {
        /// <summary>
        /// RouteNumber of the SearchRoute
        /// </summary>
        public string RouteNumber { get; set; }

        /// <summary>
        /// RouteName of the SearchRoute
        /// </summary>
        public string RouteName { get; set; }

        /// <summary>
        /// CenterNumber of the SearchAlert
        /// </summary>
        public int? CenterNumber { get; set; }

        /// <summary>
        /// BillTo of the UpdateStop
        /// </summary>
        public int BillTo { get; set; }

        /// <summary>
        /// ShipTo of the UpdateStop
        /// </summary>
        public int ShipTo { get; set; }

        /// <summary>
        /// FilterStartDate of the SearchRoute
        /// </summary>
        public Nullable<DateTime> FilterStartDate { get; set; }

        /// <summary>
        /// FilterEndDate of the SearchRoute
        /// </summary>
        public Nullable<DateTime> FilterEndDate { get; set; }

        /// <summary>
        /// StartOffsetMinutes of the SearchRoute for late/early
        /// </summary>
        public double StartOffsetMinutes { get; set; }

        /// <summary>
        /// EndOffsetMinutes of the SearchRoute for late/early
        /// </summary>
        public double EndOffsetMinutes { get; set; }

        /// <summary>
        /// NearRouteId of the SearchRoute
        /// </summary>
        public int NearRouteId { get; set; }

        /// <summary>
        /// NearRoutePlanId of the SearchRoute
        /// </summary>
        public int NearRoutePlanId { get; set; }

        public bool DeepSearch { get; set; }
    }

    public class MoveStopModel : IMoveStopModel
    {
        public int CenterNumber { get; set; }
        public string SourceRouteNumber { get; set; }
        public int SourceStopNumber { get; set; }
        public int SourceRoutePlanId { get; set; }
        public int SourceRouteId { get; set; }

        public string DestinationRouteNumber { get; set; }
        public int DestinationStopNumber { get; set; }
        public DateTime DestinationDeliveryDateTime { get; set; }
        public int DestinationRouteId { get; set; }
        public string StopModificationComment { get; set; }
        public string CreatedBy { get; set; }
    }
}