using System.Collections.Generic;
using System.Data.Entity;
using System.Net.Http;
using System.Threading.Tasks;
using atm.services.models;
using Newtonsoft.Json;
using System.Linq;
using System.Data.SqlClient;

namespace atm.services
{
    public class LookUpService : AtmServiceToApiBase, ILookUpService
    {
        protected override string RouteUrl => "routefilters/";

        public LookUpService(HttpClient httpClient) : base()
        {
            _client = httpClient;
        }
        public LookUpService() : base()
        {
            _client = new HttpClient();
        }

        public async Task<ColumnOption> GetColumnOptionsForUserAndPage(string userName, string pageName)
        {
            var key = $"column.option.{userName}.{pageName}";
            if (CacheHandler.Get(key, out ColumnOption cachedObject))
            {
                return cachedObject;
            }

            using (var Db = new AtmContext())
            {
                var result = (await Db.Database.SqlQuery<string>("exec [up_p_getProfileCustom] @userName = {0}, @pageName = {1}", userName, pageName).ToListAsync()).FirstOrDefault();
                if (result == null) return null;
                var columnOption = JsonConvert.DeserializeObject<ColumnOption>(result);
                if (columnOption != null)
                {
                    if (columnOption.AvailableColumns == null)
                        columnOption.AvailableColumns = new List<AvailableColumn>();
                    if (columnOption.Columns == null)
                        columnOption.Columns = new List<Column>();
                    CacheHandler.Add(columnOption, key);
                }
                return columnOption;
            }
        }

        public async Task UpdateColumnOptionAsync(string userName, string pageName, string columnOption)
        {
            var key = $"column.option.{userName}.{pageName}";
            using (var Db = new AtmContext())
            {
                await Db.Database.ExecuteSqlCommandAsync(
                    "[up_p_saveProfileCustom] @userName, @pageName, @Columns",
                    new SqlParameter("@userName", userName),
                    new SqlParameter("@pageName", pageName),
                    new SqlParameter("@Columns", columnOption)
                );
                CacheHandler.Remove(key);
            }
        }

        public async Task<List<RouteFilter>> GetAllRouteFilters(int typeId)
        {
            var url = ApiUrl + "type/" + (typeId.ToString());
            var response = await _client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<List<RouteFilter>>(response.Content.ReadAsStringAsync().Result);
        }

        public async Task<List<ConceptType>> GetAllConcepts()
        {
            var key = $"concept.list.all";
            if (CacheHandler.Get(key, out List<ConceptType> cachedObject))
            {
                return cachedObject;
            }

            var url = BaseUrl + "Concepts";
            var response = await _client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var result = JsonConvert.DeserializeObject<List<ConceptType>>(response.Content.ReadAsStringAsync().Result);
            if (result != null) CacheHandler.Add(result, key);
            return result;
        }

        public async Task<List<ConceptType>> GetConceptsForCenter(int centerNumber)
        {
            var url = BaseUrl + "Concepts/Concept/" + centerNumber;
            var response = await _client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<List<ConceptType>>(response.Content.ReadAsStringAsync().Result);
        }

        public async Task<List<KeyValuePair<int, string>>> GetRoutesForCenterThisWeek(int centerNumber)
        {
            var url = BaseUrl + "Routes/Lookup?centerNumber=" + centerNumber;
            var response = await _client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<List<KeyValuePair<int, string>>>(response.Content.ReadAsStringAsync().Result);
        }

        public async Task<List<KeyValuePair<string, int>>> GetStopsForRoute(int routeId)
        {
            var url = BaseUrl + "Routes/Stop/Lookup?routeId=" + routeId;
            var response = await _client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<List<KeyValuePair<string, int>>>(response.Content.ReadAsStringAsync().Result);
        }
    }
}
