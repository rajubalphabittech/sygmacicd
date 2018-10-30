using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using atm.services;
using atm.services.models;

namespace atm.Models
{
    public class MovedStopPopupRouteStopViewModel
    {
        public MovedStopPopupRouteStopViewModel(Route route)
        {
            RouteNumber = route.RouteNumber;
            RouteId = route.RouteId;
            RouteName = route.RouteName;
            SygmaCenterNo = route.SygmaCenterNo;
            DispatchTime = route.AdjustedDispatchTime;
            TotalHours = route.TotalHours;

            Stops = new List<StopViewModel>();
            foreach (var item in route.RouteStops)
                Stops.Add(new StopViewModel(item));

            TotalAS400Miles = route.TotalAS400Miles;
            TotalATMMiles = route.TotalATMMiles;

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

            ScheduledDispatchTime = route.ScheduledDispatchTime;
            PlannedDispatchTime = route.PlannedDispatchTime;
            RouteHasStopAdditions = route.RouteHasStopAdditions;
            RouteHasStopRemovals = route.RouteHasStopRemovals;
            RouteHasStopTimeChanges = route.RouteHasStopTimeChanges;
            RouteWasModified = route.RouteWasModified;
            AverageLongitude = route.AverageLongitude;
            AverageLatitude = route.AverageLatitude;
            Proximity = route.Proximity;
            CompassHeading = route.CompassHeading;
            NearRouteName = route.NearRouteName;
            NearRouteNumber = route.NearRouteNumber;
            NearRouteId = route.NearRouteId;
            NearRoutePlanId = route.NearRoutePlanId;
            NearStopNumber = route.NearStopNumber;
            ClosestStopRoutePlanId = route.ClosestStopRoutePlanId;
            ClosestStopProximity = route.ClosestStopProximity;
            ClosestStopCompassHeading = route.ClosestStopCompassHeading;
            ClosestStop = route.ClosestStop;
            ConceptIds = route.ConceptIds;
            Concepts = route.Concepts;
        }

        public string RouteNumber { get; set; }
        public int RouteId { get; set; }
        public string RouteName { get; set; }
        public int SygmaCenterNo { get; set; }
        public DateTime RouteDate { get; set; }
        public decimal TotalHours { get; set; }
        public string Position { get; set; }
        public DateTime? DispatchTime { get; set; }
        public ColumnOptionViewModel StopColumnOption { get; set; }
        public ColumnOptionViewModel RouteColumnOption { get; set; }
        public List<StopViewModel> Stops { get; set; }

        //Additional data for _movedStopPopup
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
    }
}