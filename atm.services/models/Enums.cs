using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace atm.services.models
{
    public enum MoveStatuses : int
    {
        [Display(Name = "None")] None = 0,
        [Display(Name = "Pending")] Pending = 1,
        [Display(Name = "Pre-Processing")] PreProcessing = 2,
        [Display(Name = "Processing")] Processing = 3,
        [Display(Name = "Post-Processing")] PostProcessing = 4,
        [Display(Name = "Complete")] Complete = 5,
        [Display(Name = "Error")] Error = 6,
        [Display(Name = "Cancelled")] Cancelled = 7
    }

    public enum RouteOrders : int
    {
        RouteHasNoOrders = 1,
        RouteHasSomeOrders = 2,
        RouteHasAllOrders = 3
    }

    public enum RoutePlanModificationTypes : int
    {
        None = 0,
        SystemRemoved = 1,
        SystemAdded = 2,
        TimeChanged = 3,
        ManualRemoved = 4,
        ManualAdded = 5
    }

    public enum RouteFilterTypes : byte
    {
        RouteStatus = 1,
        RouteNumbers = 2,
        DateRange = 3
    }

    public enum SortDirections : int
    {
        Ascending = 1,
        Descending = 2
    }

    public enum RouteSortFields : int
    {
        RouteNumber = 1,
        RouteName = 2,
        DispatchTime = 3,
        NumberOfStops = 4,
        Duration = 5,
        Weight = 6,
        Cubes = 7,
        Cases = 8,
        Proximity = 9
    }

    public enum ColumnTypes : int
    {
        Route = 1,
        Stop = 2
    }

    public enum RouteColumns : int
    {
        [Display(Name = "Route #", GroupName = "Main")] RouteNumber = 1,
        [Display(Name = "Route Name", GroupName = "Main")] RouteName = 2,
        [Display(Name = "Stops", GroupName = "Main")] NumberOfStops = 3,
        [Display(Name = "Concepts", GroupName = "Main")] Concepts = 4,

        [Display(Name = "Scheduled Dispatch Time", GroupName = "Delivery")] ScheduledDispatch = 6,
        [Display(Name = "Adjusted Dispatch Time", GroupName = "Delivery")] AdjustedDispatch = 7,

        [Display(Name = "LBs", GroupName = "Capacity")] WeightTotal = 11,
        [Display(Name = "Freezer LBs", GroupName = "Capacity")] WeightFreezer = 12,
        [Display(Name = "Cooler LBs", GroupName = "Capacity")] WeightCooler = 13,
        [Display(Name = "Dry LBs", GroupName = "Capacity")] WeightDry = 14,

        [Display(Name = "Cubes", GroupName = "Capacity")] CubesTotal = 16,
        [Display(Name = "Freezer Cubes", GroupName = "Capacity")] CubesFreezer = 17,
        [Display(Name = "Cooler Cubes", GroupName = "Capacity")] CubesCooler = 18,
        [Display(Name = "Dry Cubes", GroupName = "Capacity")] CubesDry = 19,

        [Display(Name = "Cases", GroupName = "Capacity")] CasesTotal = 21,
        [Display(Name = "Freezer Cases", GroupName = "Capacity")] CasesFreezer = 22,
        [Display(Name = "Cooler Cases", GroupName = "Capacity")] CasesCooler = 23,
        [Display(Name = "Dry Cases", GroupName = "Capacity")] CasesDry = 24,

        [Display(Name = "Primary Driver", GroupName = "Driver")] PrimaryDriverName = 30,
        [Display(Name = "Secondary Driver", GroupName = "Driver")] SecondaryDriverName = 31,
        [Display(Name = "Team Driver/Helper", GroupName = "Driver")] HelperName = 32,
        [Display(Name = "Truck Id", GroupName = "Driver")] TruckId = 33,
        [Display(Name = "Trailer Id", GroupName = "Driver")] TrailerId = 34
    }

    public enum StopColumns : int
    {
        [Display(Name = "Stop", GroupName = "Main")] StopNumber = 1,
        [Display(Name = "Status", GroupName = "Main")] OverallStatus = 2,
        [Display(Name = "Bill To", GroupName = "Main")] BillTo = 3,
        [Display(Name = "Ship To", GroupName = "Main")] ShipTo = 4,
        [Display(GroupName = "Main")] Customer = 5,

        [Display(GroupName = "Main")] Concept = 6,
        [Display(GroupName = "Address")] Street = 7,
        [Display(GroupName = "Address")] City = 8,
        [Display(GroupName = "Address")] State = 9,
        [Display(GroupName = "Address")] Zip = 10,

        [Display(GroupName = "Contact")] Phone = 11,
        [Display(GroupName = "Contact")] Email = 12,
        
        [Display(Name = "LBs", GroupName = "Capacity")] WeightTotal = 16,
        [Display(Name = "Freezer LBs", GroupName = "Capacity")] WeightFreezer = 17,
        [Display(Name = "Cooler LBs", GroupName = "Capacity")] WeightCooler = 18,
        [Display(Name = "Dry LBs", GroupName = "Capacity")] WeightDry = 19,

        [Display(Name = "Cubes", GroupName = "Capacity")] CubesTotal = 21,
        [Display(Name = "Freezer Cubes", GroupName = "Capacity")] CubesFreezer = 22,
        [Display(Name = "Cooler Cubes", GroupName = "Capacity")] CubesCooler = 23,
        [Display(Name = "Dry Cubes", GroupName = "Capacity")] CubesDry = 24,

        [Display(Name = "Cases", GroupName = "Capacity")] CasesTotal = 26,
        [Display(Name = "Freezer Cases", GroupName = "Capacity")] CasesFreezer = 27,
        [Display(Name = "Cooler Cases", GroupName = "Capacity")] CasesCooler = 28,
        [Display(Name = "Dry Cases", GroupName = "Capacity")] CasesDry = 29,

        [Display(Name = "Planned Delivery", GroupName = "Delivery")] PlannedDelivery = 31,
        [Display(Name = "Scheduled Delivery", GroupName = "Delivery")] ScheduledDelivery = 32,
        [Display(Name = "Scheduled Offset Hours / Cascade", GroupName = "Delivery")] ScheduledOffset = 33,
        [Display(Name = "Adjusted Delivery", GroupName = "Delivery")] AdjustedDelivery = 34,
        [Display(Name = "Adjusted Offset Hours / Cascade", GroupName = "Delivery")] AdjustedOffset = 35,
        [Display(Name = "Actual Arrival Time", GroupName = "Delivery")] ArrivalDelivery = 36,
        [Display(Name = "Order Status", GroupName = "Capacity")] OrderStatus = 37,
        [Display(Name = "Stop Status", GroupName = "Delivery")] StopStatus = 38,

        [Display(Name = "Current Driver Name", GroupName = "Driver")] CurrentDriverName = 41,


        [Display(GroupName = "Main")] Comment = 51
    }
}
