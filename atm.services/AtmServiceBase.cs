using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Net.Http;
using System.Web;
using Newtonsoft.Json;
using StackExchange.Profiling;
using SygmaFramework;

namespace atm.services
{
    public abstract class AtmServiceToApiBase : IDisposable
    {
        protected AtmServiceToApiBase()
        {
        }

        protected virtual string BaseUrl => ConfigurationManager.AppSettings["APIUrl"];
        protected abstract string RouteUrl { get; }
        protected string ApiUrl => BaseUrl + RouteUrl;
        protected HttpClient _client;

        public virtual void Dispose()
        {
            if (_client != null) _client.Dispose();
        }
    }
}