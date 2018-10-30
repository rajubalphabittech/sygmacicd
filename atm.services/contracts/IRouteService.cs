using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using atm.services.models;

namespace atm.services
{
	public interface IRouteService
	{
        Task<IEnumerable<Route>> SearchAsync(SearchRoute criteria);
        Task<Route> GetByRouteIdAsync(int routeId);
        Task<Route> GetByRouteIdAndCenterNumberAndRouteNumberAsync(int routeId, int centerNumber = 0, string routeNumber = "");
        Task UpdateRouteAsync(string routeNumber, int centerNumber, UpdateRouteWithStops route);
        Task MoveStopAsync(IMoveStopModel model);
        Task<IEnumerable<Route>> GetRoutesWithStops(string routeNumber, string routeName, string centerNumber, DateTime filterStartDate, DateTime filterEndDate, string startOffsetMinutes, string endOffsetMinutes, bool deepSearch);
        Task<IEnumerable<Stop>> GetStopsByRoutePlanIdAsync(int routePlanId);
    }
}