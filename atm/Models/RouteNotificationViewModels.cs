using atm.services;
using atm.services.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace atm.Models
{
    public class RouteNotificationListViewModel : List<RouteNotificationViewModel>
    {
        public RouteNotificationListViewModel(IEnumerable<Route> routes)
        {
            foreach (var route in routes)
            {
                Add(new RouteNotificationViewModel(route));
            }
        }
    }

    public class RouteNotificationViewModel
    {
        public RouteNotificationViewModel(Route route)
        {
            RouteNumber = route.RouteNumber;
            RouteId = route.RouteId;
            RouteName = route.RouteName;
            SygmaCenterNo = route.SygmaCenterNo;
            NumberOfStops = route.NumberOfStops;
            PlannedDispatchTime = route.PlannedDispatchTime;
            ScheduledDispatchTime = route.ScheduledDispatchTime;
            AdjustedDispatchTime = route.AdjustedDispatchTime;
            ConceptIds = route.ConceptIds;
            Concepts = route.Concepts;

            Stops = new List<RouteNotificationStopViewModel>();
            foreach (var item in route.RouteStops)
                Stops.Add(new RouteNotificationStopViewModel(item));
        }

        public string RouteNumber { get; set; }
        public int RouteId { get; set; }
        public string RouteName { get; set; }
        public int SygmaCenterNo { get; set; }
        public DateTime? PlannedDispatchTime { get; set; }
        public DateTime? ScheduledDispatchTime { get; set; }
        public DateTime? AdjustedDispatchTime { get; set; }
        public IEnumerable<int> ConceptIds { get; set; }
        public IEnumerable<string> Concepts { get; set; }
        public int NumberOfStops { get; set; }
        public List<RouteNotificationStopViewModel> Stops { get; set; }
    }

    public class RouteNotificationStopViewModel
    {
        public RouteNotificationStopViewModel(IStop stop)
        {
            RoutePlanId = stop.RoutePlanId;
            SygmaCenterNo = stop.SygmaCenterNo;
            RouteNumber = stop.RouteNumber;
            RouteId = stop.RouteId;
            RouteName = stop.RouteName;
            BillTo = stop.BillTo;
            ShipTo = stop.ShipTo;
            StopNumber = stop.StopNumber;
            PlannedDeliveryDateTime = stop.PlannedDeliveryDateTime;
            AdjustedDeliveryDateTime = stop.AdjustedDeliveryDateTime;
            ScheduledDeliveryDateTime = stop.ScheduledDeliveryDateTime;
            IsActive = stop.IsActive;
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
            Concept = stop.Concept;
            ConceptCode = stop.ConceptCode;
            ConceptId = stop.ConceptId;
            OriginalRouteId = stop.OriginalRouteId;
            TimeZone = stop.TimeZone;
            OrderStatus = stop.OrderStatus;
            RoutePlanModificationTypeId = stop.RoutePlanModificationTypeId;
        }

        public int RoutePlanId { get; set; }
        public int SygmaCenterNo { get; set; }
        public string RouteNumber { get; set; }
        public int RouteId { get; set; }
        public string RouteName { get; set; }
        public decimal BillTo { get; set; }
        public decimal ShipTo { get; set; }
        public long? StopNumber { get; set; }
        public DateTime? PlannedDeliveryDateTime { get; set; }
        public DateTime? ScheduledDeliveryDateTime { get; set; }
        public DateTime? AdjustedDeliveryDateTime { get; set; }
        public byte? IsActive { get; set; }
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
        public int ConceptId { get; set; }
        public string Concept { get; set; }
        public string ConceptCode { get; set; }
        public int OriginalRouteId { get; set; }
        public string TimeZone { get; set; }
        public string OrderStatus { get; set; }
        public byte RoutePlanModificationTypeId { get; set; }
    }
}