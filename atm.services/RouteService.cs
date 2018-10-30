using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using atm.services.models;
using Newtonsoft.Json;
using StackExchange.Profiling;

namespace atm.services
{
    public class RouteService : AtmServiceToApiBase, IRouteService
    {
        protected override string RouteUrl => "routes/";

        public RouteService(HttpClient httpClient) : base()
        {
            _client = httpClient;
        }

        public RouteService() : base()
        {
            _client = new HttpClient();
        }

        public async Task<IEnumerable<Route>> SearchAsync(SearchRoute criteria)
        {
            var url = ApiUrl + $"search?DeepSearch={criteria.DeepSearch}";
            if (criteria.CenterNumber.HasValue && criteria.CenterNumber > 0) url = url + $"&CenterNumber={criteria.CenterNumber}";
            if (criteria.FilterStartDate.HasValue) url = url + $"&FilterStartDate={criteria.FilterStartDate.Value}";
            if (criteria.FilterEndDate.HasValue) url = url + $"&FilterEndDate={criteria.FilterEndDate.Value}";
            if (!string.IsNullOrEmpty(criteria.RouteName)) url = url + $"&RouteName={criteria.RouteName}";
            if (!string.IsNullOrEmpty(criteria.RouteNumber)) url = url + $"&RouteNumber={criteria.RouteNumber}";
            if (criteria.NearRouteId != 0) url = url + $"&NearRouteId={criteria.NearRouteId}";
            if (criteria.NearRoutePlanId != 0) url = url + $"&NearRoutePlanId={criteria.NearRoutePlanId}";
            if (criteria.BillTo != 0) url = url + $"&BillTo={criteria.BillTo}";
            if (criteria.ShipTo != 0) url = url + $"&ShipTo={criteria.ShipTo}";

            using (MiniProfiler.Current.Step("SearchAsync"))
            {
                var response = await _client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                MiniProfiler.Current.TryAddMiniProfilerResultsFromHeader(response);
                var result = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<IEnumerable<Route>>(result);
            }
        }

        public async Task<Route> GetByRouteIdAsync(int routeId)
        {
            var url = ApiUrl + routeId.ToString();
            var response = await _client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            MiniProfiler.Current.TryAddMiniProfilerResultsFromHeader(response);
            return JsonConvert.DeserializeObject<Route>(response.Content.ReadAsStringAsync().Result);
        }

        public async Task<Route> GetByRouteIdAndCenterNumberAndRouteNumberAsync(int routeId, int centerNumber = 0, string routeNumber = "")
        {
            var url = ApiUrl + routeId.ToString();
            if (centerNumber > 0)
            {
                url  = url + $"?centerNumber={centerNumber}";
                if (!string.IsNullOrEmpty(routeNumber))
                {
                    url = url + $"&routeNumber={routeNumber}";
                }
            } else
            {
                url = url + $"?routeNumber={routeNumber}";
            }
            var response = await _client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            MiniProfiler.Current.TryAddMiniProfilerResultsFromHeader(response);
            return JsonConvert.DeserializeObject<Route>(response.Content.ReadAsStringAsync().Result);
        }

        public async Task UpdateRouteAsync(string routeNumber, int centerNumber, UpdateRouteWithStops route)
        {
            var url = ApiUrl + "routenumber/" + routeNumber + "/center/" + centerNumber;
            var response = await _client.PostAsync(url, new StringContent(
                JsonConvert.SerializeObject(
                    route
                    ), Encoding.UTF8, "application/json"));

            response.EnsureSuccessStatusCode();
        }

        public async Task MoveStopAsync(IMoveStopModel model)
        {
            var url = ApiUrl + "movestop";
            var response = await _client.PostAsync(url, new StringContent(
                JsonConvert.SerializeObject(
                    model
                    ), Encoding.UTF8, "application/json"));

            response.EnsureSuccessStatusCode();
        }

        public async Task<IEnumerable<Route>> GetRoutesWithStops(string routeNumber, string routeName, string centerNumber, DateTime filterStartDate, DateTime filterEndDate, string startOffsetMinutes, string endOffsetMinutes, bool deepSearch)
        {
            routeNumber = routeNumber.Replace(" ", String.Empty);
            routeName = routeName.Replace(" ", String.Empty);
            startOffsetMinutes = startOffsetMinutes.Replace(" ", String.Empty);
            endOffsetMinutes = endOffsetMinutes.Replace(" ", String.Empty);

            //set up base URL with center number
            if (centerNumber == null || centerNumber == "")
            {
                return JsonConvert.DeserializeObject<List<Route>>("[]"); //no data for empty center number call
            }
            var url = ApiUrl + "search/?searchCriteria.centerNumber=" + centerNumber;

            if (routeNumber != null && routeNumber != "")
            {
                url += ("&searchCriteria.routeNumber=" + routeNumber);
            }

            if (routeName != null && routeName != "")
            {
                url += ("&searchCriteria.routeName=" + routeName);
            }

            url += ("&searchCriteria.filterStartDate=" + filterStartDate);
            url += ("&searchCriteria.filterEndDate=" + filterEndDate);

            if (startOffsetMinutes != null && startOffsetMinutes != "")
            {
                url += ("&searchCriteria.startOffsetMinutes=" + startOffsetMinutes);
            }

            if (endOffsetMinutes != null && endOffsetMinutes != "")
            {
                url += ("&searchCriteria.endOffsetMinutes=" + endOffsetMinutes);
            }

            url += "&searchCriteria.deepSearch=" + deepSearch;

            var response = await _client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            MiniProfiler.Current.TryAddMiniProfilerResultsFromHeader(response);
            return JsonConvert.DeserializeObject<IEnumerable<Route>>(response.Content.ReadAsStringAsync().Result);

        }

        public async Task<IEnumerable<Stop>> GetStopsByRoutePlanIdAsync(int routePlanId)
        {
            var url = ApiUrl + "stop/" + routePlanId;

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                MiniProfiler.Current.TryAddMiniProfilerResultsFromHeader(response);
                return JsonConvert.DeserializeObject<IEnumerable<Stop>>(response.Content.ReadAsStringAsync().Result);
            }
        }
    }
}