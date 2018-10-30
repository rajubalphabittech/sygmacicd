using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using atm.services;
using atm.services.models;

namespace atm.Models
{
    public class RouteStopViewModel
    {
        public RouteStopViewModel(Route route)
        {
            RouteNumber = route.RouteNumber;
            RouteId = route.RouteId;
            RouteName = route.RouteName;
            SygmaCenterNo = route.SygmaCenterNo;
            DispatchTime = route.AdjustedDispatchTime;
            TotalHours = route.TotalHours;

            Stops = new List<StopViewModel>();
            foreach (var item in route.RouteStops)
                Stops.Add(new StopViewModel(item, route.HasDispatched));
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
        public List<StopViewModel> Stops { get; set; }
    }

    public class StopViewModel : IStop
    {
        public StopViewModel(IStop stop, bool routeHasDispatched = false)
        {
            RoutePlanId = stop.RoutePlanId;
            SygmaCenterNo = stop.SygmaCenterNo;
            RouteNumber = stop.RouteNumber;
            RouteId = stop.RouteId;
            RouteName = stop.RouteName;
            BillTo = stop.BillTo;
            ShipTo = stop.ShipTo;
            StopNumber = stop.StopNumber;
            TelogisArrivalDeliveryDateTime = stop.TelogisArrivalDeliveryDateTime;
            PlannedDeliveryDateTime = stop.PlannedDeliveryDateTime;
            AdjustedDeliveryDateTime = stop.AdjustedDeliveryDateTime;
            ScheduledDeliveryDateTime = stop.ScheduledDeliveryDateTime;
            EffectiveDate = stop.EffectiveDate;
            RouteStopTypeId = stop.RouteStopTypeId;
            IsActive = stop.IsActive;
            RecordCreatedDate = stop.RecordCreatedDate;
            LastUpdatedDate = stop.LastUpdatedDate;
            CustomerName = stop.CustomerName;
            EmailAddress = stop.EmailAddress;
            Phone = stop.Phone;
            Address1 = stop.Address1;
            Address2 = stop.Address2;
            City = stop.City;
            State = stop.State;
            Zip = stop.Zip;
            Longitude = stop.Longitude;
            Latitude = stop.Latitude;
            OntimeCalculationId = stop.OntimeCalculationId;
            Comment = stop.Comment;
            LastCustomerCommunicationComment = stop.LastCustomerCommunicationComment;
            RoutePlanModificationTypeId = stop.RoutePlanModificationTypeId;
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
            Concept = stop.Concept;
            ConceptCode = stop.ConceptCode;
            ConceptId = stop.ConceptId;
            AS400Miles = stop.AS400Miles;
            ATMMiles = stop.ATMMiles;
            AddOnOrderCount = stop.AddOnOrderCount;
            Proximity = stop.Proximity;
            CompassHeading = stop.CompassHeading;
            OriginalRouteId = stop.OriginalRouteId;
            TimeZone = stop.TimeZone;
            OrderStatus = stop.OrderStatus;
            OrderId = stop.OrderId;
            DayNumber = stop.DayNumber;
            EnableStopMove = stop.EnableStopMove;
            IsEarlyOrLate = stop.IsEarlyOrLate;
            MoveStatus = stop.MoveStatus;
            CurrentDriverName = stop.CurrentDriverName;
            StopStatus = stop.StopStatus;

            if (StopStatus == "Completed" || StopStatus == "In Transit")
                OverallStatus = StopStatus;
            else
            {
                if (routeHasDispatched)
                {
                    if (StopStatus == "" || StopStatus == "Unknown")
                        OverallStatus = "Dispatched";
                    else
                        OverallStatus = StopStatus;
                }
                else
                {
                    if (OrderStatus == "I") OverallStatus = "Invoiced";
                    if (OrderStatus == "P") OverallStatus = "Picking";
                    if (OrderStatus == "A") OverallStatus = "Incomplete";
                    if (OrderStatus == "K") OverallStatus = "Released";
                    if (OrderStatus == "C") OverallStatus = "Cancellation";
                    if (OrderStatus == "S") OverallStatus = "Selected";
                    if (OrderStatus == "E") OverallStatus = "Ready";
                }
            }

            if (stop.RoutePlanModificationTypeId == 1 || stop.RoutePlanModificationTypeId == 4 || stop.EnableStopMove == 0 || stop.OrderId == 0 || routeHasDispatched)
            {
                CanMove = false;
            }
            else
            {
                CanMove = true;
            }
        }

        public int RoutePlanId { get; set; }
        public int SygmaCenterNo { get; set; }
        public string RouteNumber { get; set; }
        public int RouteId { get; set; }
        public string RouteName { get; set; }
        public decimal BillTo { get; set; }
        public decimal ShipTo { get; set; }
        public long? StopNumber { get; set; }
        public DateTime? TelogisArrivalDeliveryDateTime { get; set; }
        public DateTime? PlannedDeliveryDateTime { get; set; }
        public DateTime? ScheduledDeliveryDateTime { get; set; }
        public DateTime? AdjustedDeliveryDateTime { get; set; }
        public DateTime EffectiveDate { get; set; }
        public int RouteStopTypeId { get; set; }
        public byte? IsActive { get; set; }
        public DateTime? RecordCreatedDate { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public string CustomerName { get; set; }
        public string EmailAddress { get; set; }
        public string Phone { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public int OntimeCalculationId { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string Comment { get; set; }
        public string LastCustomerCommunicationComment { get; set; }
        public byte RoutePlanModificationTypeId { get; set; }
        public decimal Weight { get; set; }
        public decimal WeightFreezer { get; set; }
        public decimal WeightCooler { get; set; }
        public decimal WeightDry { get; set; }
        public decimal Cubes { get; set; }
        public decimal CubesFreezer { get; set; }
        public decimal CubesCooler { get; set; }
        public decimal CubesDry { get; set; }
        public decimal Cases { get; set; }
        public decimal CasesFreezer { get; set; }
        public decimal CasesCooler { get; set; }
        public decimal CasesDry { get; set; }
        public int ConceptId { get; set; }
        public string Concept { get; set; }
        public string ConceptCode { get; set; }
        public int AddOnOrderCount { get; set; }
        public int AS400Miles { get; set; }
        public int ATMMiles { get; set; }
        public double Proximity { get; set; }
        public string CompassHeading { get; set; }
        public int OriginalRouteId { get; set; }
        public string TimeZone { get; set; }
        public string OrderStatus { get; set; }
        public int EnableStopMove { get; set; }
        public int OrderId { get; set; }
        public byte DayNumber { get; set; }
        public int IsEarlyOrLate { get; set; }
        public MoveStatuses MoveStatus { get; set; }
        public string CurrentDriverName { get; set; }
        public string StopStatus { get; set; }
        public string OverallStatus { get; set; }
        public bool CanMove { get; set; }
    }

    public class StopMoveViewModel
    {
        public int SourceRouteId { get; set; }
        public int SourceRoutePlanId { get; set; }
        public int DestinationRouteId { get; set; }
        public int SygmaCenterNo { get; set; }
        public IEnumerable<KeyValuePair<int, string>> RouteList { get; set; }
    }

    public class UpdateRouteWithStopsViewModel
    {
        public string RouteNumber { get; set; }
        public int RouteId { get; set; }
        public string RouteName { get; set; }
        public int SygmaCenterNo { get; set; }
        public DateTime RouteDate { get; set; }

        public List<UpdateStopViewModel> Stops { get; set; }
    }

    public class UpdateStopViewModel
    {
        public int RoutePlanId { get; set; }
        public int RouteId { get; set; }
        public decimal BillTo { get; set; }
        public decimal ShipTo { get; set; }
        public DateTime? ScheduledDeliveryDateTime { get; set; }
        public DateTime? AdjustedDeliveryDateTime { get; set; }
        public string EarlyLate { get; set; }
        public int RoutePlanModificationTypeId { get; set; }
        public string RouteNumber { get; set; }
        public string AdjustedRouteNumber { get; set; }
        public int StopNumber { get; set; }
        public int AdjustedStopNumber { get; set; }
        public string LastCustomerCommunicationComment { get; set; }
    }

    public class MoveStopViewModel : IMoveStopModel
    {
        public int CenterNumber { get; set; }
        public DateTime DestinationDeliveryDateTime { get; set; }
        public int DestinationRouteId { get; set; }
        public string DestinationRouteNumber { get; set; }
        public int DestinationStopNumber { get; set; }
        public int SourceRouteId { get; set; }
        public string SourceRouteNumber { get; set; }
        public int SourceRoutePlanId { get; set; }
        public int SourceStopNumber { get; set; }
        public string StopModificationComment { get; set; }
        public string CreatedBy { get; set; }
    }

}