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
    public class NotificationService : AtmServiceToApiBase, INotificationService
    {
        protected override string RouteUrl => "notification/";

        public NotificationService(HttpClient httpClient) : base( )
        {
            _client = httpClient;
        }

        public NotificationService() : base( )
        {
            _client = new HttpClient();
        }

        public async Task SendAsync(List<Notification> notifications)
        {
            var url = ApiUrl + "/multiple";
            var response = await _client.PostAsync(url, new StringContent(
                JsonConvert.SerializeObject(
                    notifications
                    ), Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
        }
    }
}