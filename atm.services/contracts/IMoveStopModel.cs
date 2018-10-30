using System;

namespace atm.services
{
    public interface IMoveStopModel
    {
        int CenterNumber { get; set; }
        DateTime DestinationDeliveryDateTime { get; set; }
        int DestinationRouteId { get; set; }
        string DestinationRouteNumber { get; set; }
        int DestinationStopNumber { get; set; }
        int SourceRouteId { get; set; }
        string SourceRouteNumber { get; set; }
        int SourceRoutePlanId { get; set; }
        int SourceStopNumber { get; set; }
        string StopModificationComment { get; set; }
        string CreatedBy { get; set; }
    }
}