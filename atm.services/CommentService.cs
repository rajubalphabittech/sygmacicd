using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using atm.services.models;
using Newtonsoft.Json;

namespace atm.services
{
    public class CommentService : AtmServiceToApiBase, ICommentService
    {
        protected override string RouteUrl => "comments/";

        public CommentService(HttpClient httpClient) : base()
        {
            _client = httpClient;
        }

        public CommentService() : base()
        {
            _client = new HttpClient();
        }

        public async Task<IEnumerable<Comment>> GetCommentsByBillToShipTo(int billTo, int shipTo, int centerNumber)
        {
            var url = ApiUrl + $"search?CenterNumber={centerNumber.ToString()}&PrimaryRecordId={billTo.ToString()}-{shipTo.ToString()}";
            var response = await _client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<IEnumerable<Comment>>(response.Content.ReadAsStringAsync().Result);
        }

        public async Task<IEnumerable<Comment>> GetCommentsByRoutePlanId(int routePlanId, int centerNumber)
        {
            var url = ApiUrl + "search?CenterNumber=" + centerNumber.ToString() + "&SecondaryRecordId=" + routePlanId.ToString();
            var response = await _client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<IEnumerable<Comment>>(response.Content.ReadAsStringAsync().Result);
        }

        public async Task<IEnumerable<Comment>> GetCommentsByRoutePlanIds(int[] routePlanIds, int centerNumber)
        {
            var url = ApiUrl + "search?CenterNumber=" + centerNumber.ToString();
            foreach (int routePlanId in routePlanIds)
            {
                url = url + "&SecondaryRecordIds=" + routePlanId.ToString();
            }
            var response = await _client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<IEnumerable<Comment>>(response.Content.ReadAsStringAsync().Result);
        }

        public async Task UpdateCommentAsync(UpdateComment comment)
        {
            var url = ApiUrl + comment.CommentId.ToString();
            var response = await _client.PostAsync(url, new StringContent(
                JsonConvert.SerializeObject(comment), Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
        }

        public async Task AddCommentAsync(AddComment comment)
        {
            var url = ApiUrl;
            var response = await _client.PostAsync(url, new StringContent(
                JsonConvert.SerializeObject(comment), Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
        }

        public async Task<Comment> GetByCommentIdAsync(int routeId)
        {
            var url = ApiUrl + routeId.ToString();
            var response = await _client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<Comment>(response.Content.ReadAsStringAsync().Result);
        }
    }
}