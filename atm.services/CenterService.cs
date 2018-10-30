using System.Collections.Generic;
using System.Data.Entity;
using System.Net.Http;
using System.Threading.Tasks;
using atm.services.models;
using Newtonsoft.Json;

namespace atm.services
{
    public class CenterService : AtmServiceToApiBase, ICenterService
    {
        public CenterService(HttpClient httpClient) : base()
        {
            _client = httpClient;
        }
        public CenterService() : base()
        {
            _client = new HttpClient();
        }

        protected override string RouteUrl => "centers/";

        public async Task<List<BasicCenter>> GetAll(string userName)
        {
            var key = $"center.list.{userName}";
            if (CacheHandler.Get(key, out List<BasicCenter> cachedObject))
            {
                return cachedObject;
            }

            using (var Db = new AtmContext())
            {
                var result = await Db.Database
                .SqlQuery<BasicCenter>("exec [up_getCenters] @userName = {0}",
                    userName
                ).ToListAsync();
                CacheHandler.Add(result, key);
                return result;
            }
        }

        public async Task<CenterLocation> GetLocationByNoAsync(int sygmaCenterNo)
        {
            var key = $"center.{sygmaCenterNo}";
            if (CacheHandler.Get(key, out CenterLocation cachedObject))
            {
                return cachedObject;
            }

            var url = ApiUrl + "Number/" + sygmaCenterNo.ToString();
            var response = await _client.GetAsync(url);
            var result = JsonConvert.DeserializeObject<CenterLocation>(response.Content.ReadAsStringAsync().Result);
            CacheHandler.Add(result, key);
            return result;
        }
    }
}