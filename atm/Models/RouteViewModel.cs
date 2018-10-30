using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using atm.services.models;
using Newtonsoft.Json;

namespace atm.Models
{
    public class RouteViewModel
    {
        //private static Random rand = new Random();

        public RouteViewModel(Route route)
        {
            RouteId = route.RouteId;
            RouteNumber = route.RouteNumber.Trim();
            RouteName = route.RouteName.Trim();
            SygmaCenterNo = route.SygmaCenterNo;
            NumberOfStops = route.NumberOfStops;
            PlannedDispatchTime = route.PlannedDispatchTime;
            ScheduledDispatchTime = route.ScheduledDispatchTime;
            AdjustedDispatchTime = route.AdjustedDispatchTime;
            RouteHasStopAdditions = route.RouteHasStopAdditions;
            RouteHasStopRemovals = route.RouteHasStopRemovals;
            RouteHasStopTimeChanges = route.RouteHasStopTimeChanges;
            RouteWasModified = route.RouteWasModified;
            Proximity = route.Proximity;
            CompassHeading = route.CompassHeading;
            NearRouteId = route.NearRouteId;
            NearRouteName = route.NearRouteName;
            NearRouteNumber = route.NearRouteNumber;
            ConceptIds = route.ConceptIds;
            Concepts = route.Concepts;

            TotalHours = route.TotalHours;
            TotalWeight = route.TotalWeight;
            TotalWeightFreezer = route.TotalWeightFreezer;
            TotalWeightCooler = route.TotalWeightCooler;
            TotalWeightDry = route.TotalWeightDry;

            TotalCubes = route.TotalCubes;
            TotalCubesFreezer = route.TotalCubesFreezer;
            TotalCubesCooler = route.TotalCubesCooler;
            TotalCubesDry = route.TotalCubesDry;

            TotalCases = route.TotalCases;
            TotalCasesFreezer = route.TotalCasesFreezer;
            TotalCasesCooler = route.TotalCasesCooler;
            TotalCasesDry = route.TotalCasesDry;

            TotalAS400Miles = route.TotalAS400Miles;
            TotalATMMiles = route.TotalATMMiles;

            NearRoutePlanId = route.NearRoutePlanId;
            NearStopNumber = route.NearStopNumber;
            ClosestStop = route.ClosestStop != null ? new StopViewModel(route.ClosestStop) : null;
            ClosestStopCompassHeading = route.ClosestStopCompassHeading;
            ClosestStopProximity = route.ClosestStopProximity;
            ClosestStopRoutePlanId = route.ClosestStopRoutePlanId;
            IsEarlyOrLate = route.IsEarlyOrLate;
            HasOrders = route.HasOrders;
            NumberOfOrders = route.NumberOfOrders;
            PrimaryDriverName = route.PrimaryDriverName;
            SecondaryDriverName = route.SecondaryDriverName;
            HelperName = route.HelperName;
            TruckId = route.TruckId;
            TrailerId = route.TrailerId;
            HasDispatched = route.HasDispatched;
        }
        public IEnumerable<int> ConceptIds { get; set; }
        public IEnumerable<string> Concepts { get; set; }
        public int RouteId { get; set; }
        public string RouteNumber { get; set; }
        public string RouteName { get; set; }
        public int SygmaCenterNo { get; set; }
        public int NumberOfStops { get; set; }
        public int TotalAS400Miles { get; set; }
        public int TotalATMMiles { get; set; }
        public decimal TotalHours { get; set; }
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
        public DateTime? PlannedDispatchTime { get; set; }
        public DateTime? ScheduledDispatchTime { get; set; }
        public DateTime? AdjustedDispatchTime { get; set; }
        public bool RouteHasStopAdditions { get; set; }
        public bool RouteHasStopRemovals { get; set; }
        public bool RouteHasStopTimeChanges { get; set; }
        public bool RouteWasModified { get; set; }
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
        public int IsEarlyOrLate { get; set; }
        public RouteOrders HasOrders { get; set; }
        public int NumberOfOrders { get; set; }
        public StopViewModel ClosestStop { get; set; }
        public string PrimaryDriverName { get; set; }
        public string SecondaryDriverName { get; set; }
        public string HelperName { get; set; }
        public int TruckId { get; set; }
        public int TrailerId { get; set; }
        public bool HasDispatched { get; set; }
    }

    public class RouteForMapModel
    {
        [JsonProperty("number")] public string Number { get; set; }
        [JsonProperty("routeId")] public string RouteId { get; set; }
        [JsonProperty("stops")] public StopLocationForMapModel[] Stops { get; set; }

        public RouteForMapModel(Route route)
        {
            RouteId = route.RouteId.ToString();
            Number = route.RouteNumber.Trim();

            Stops = route.RouteStops.Where(s => s.StopNumber != 999 && s.StopNumber != 0 && s.RoutePlanModificationTypeId != 1 && s.RoutePlanModificationTypeId != 4)
                           .OrderBy(s => s.StopNumber)
                           .Select(s => new StopLocationForMapModel(s))
                           .ToArray();
        }
    }

    public class StopLocationForMapModel
    {
        public StopLocationForMapModel(Stop stop)
        {
            Id = stop.RouteNumber.Trim();
            Label = $"{stop.StopNumber} - {stop.CustomerName.Trim()}";
            var street = $"{stop.Address1.Trim()} {stop.Address2.Trim()}";
            Address = $"{street.Trim()}, {stop.City.Trim()}, {stop.State.Trim()} {stop.Zip.Trim()}";
            Lat = stop.Latitude;
            Lon = stop.Longitude;
            StopNumber = stop.StopNumber.ToString();
            AdjustedDeliveryDateTime = stop.AdjustedDeliveryDateTime;
            ScheduledDeliveryDateTime = stop.ScheduledDeliveryDateTime;
            PlannedDeliveryDateTime = stop.PlannedDeliveryDateTime;
            ShipTo = stop.ShipTo.ToString("000");
            BillTo = stop.BillTo.ToString("0000");

            Weight = stop.Weight;
            WeightCooler = stop.WeightCooler;
            WeightDry = stop.WeightDry;
            WeightFreezer = stop.WeightFreezer;
            Cubes = stop.Cubes;
            CubesCooler = stop.CubesCooler;
            CubesDry = stop.CubesDry;
            CubesFreezer = stop.CubesFreezer;
            Cases = stop.Cases;
            CasesCooler = stop.CasesCooler;
            CasesDry = stop.CasesDry;
            CasesFreezer = stop.CasesFreezer;
        }

        // add capacity, delivery times
        [JsonProperty("adjustedDeliveryDateTime")] public DateTime? AdjustedDeliveryDateTime { get; set; }
        [JsonProperty("scheduledDeliveryDateTime")] public DateTime? ScheduledDeliveryDateTime { get; set; }
        [JsonProperty("plannedDeliveryDateTime")] public DateTime? PlannedDeliveryDateTime { get; set; }
        [JsonProperty("shipTo")] public string ShipTo { get; set; }
        [JsonProperty("billTo")] public string BillTo { get; set; }

        [JsonProperty("weight")] public decimal Weight { get; set; }
        [JsonProperty("weightFreezer")] public decimal WeightFreezer { get; set; }
        [JsonProperty("weightCooler")] public decimal WeightCooler { get; set; }
        [JsonProperty("weightDry")] public decimal WeightDry { get; set; }
        [JsonProperty("cubes")] public decimal Cubes { get; set; }
        [JsonProperty("cubesFreezer")] public decimal CubesFreezer { get; set; }
        [JsonProperty("cubesCooler")] public decimal CubesCooler { get; set; }
        [JsonProperty("cubesDry")] public decimal CubesDry { get; set; }
        [JsonProperty("cases")] public decimal Cases { get; set; }
        [JsonProperty("casesFreezer")] public decimal CasesFreezer { get; set; }
        [JsonProperty("casesCooler")] public decimal CasesCooler { get; set; }
        [JsonProperty("casesDry")] public decimal CasesDry { get; set; }

        [JsonProperty("id")] public string Id { get; set; }
        [JsonProperty("label")] public string Label { get; set; }
        [JsonProperty("address")] public string Address { get; set; }
        [JsonProperty("lat")] public double Lat { get; set; }
        [JsonProperty("lng")] public double Lon { get; set; }
        [JsonProperty("stop")] public string StopNumber { get; set; }
    }

    public class RouteOverlayModel
    {
        public string Position { get; set; }
        public RouteManagerViewModel PageModel { get; set; }
    }

    public interface ISortable
    {
        RouteSortFields SortField { get; set; }
        SortDirections SortDirection { get; set; }
    }

    public interface IPageable
    {
        int PageIndex { get; set; }
    }

    /// <summary>
    /// SearchRoute 
    /// </summary>
    public class SearchRouteSortableViewModel : atm.services.models.SearchRoute, ISortable
    {
        public SearchRouteSortableViewModel()
        {
            SortField = RouteSortFields.RouteNumber;
        }

        public RouteSortFields SortField { get; set; }
        public SortDirections SortDirection { get; set; }
        public string CurrentSortParam { get; set; }
        public string SortParam { get; set; }
    }

    public class SearchRouteSortablePageableViewModel : SearchRouteSortableViewModel, IPageable
    {
        public int PageIndex { get; set; }
    }
}