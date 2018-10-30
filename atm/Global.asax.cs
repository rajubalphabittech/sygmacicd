using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using atm.Helpers;
using SygmaFramework;
using StackExchange.Profiling;
using StackExchange.Profiling.Storage;
using StackExchange.Profiling.Mvc;
using StackExchange.Profiling.EntityFramework6;

namespace atm
{


    public class Global : HttpApplication
    {
        protected bool ForceSecureConnection
        {
            get
            {
                if (_forceSecureConnection.HasValue) return _forceSecureConnection.Value;

                _forceSecureConnection = Boolean.Parse(WebConfigurationManager.AppSettings["ForceSSL"] ?? "false");
                return _forceSecureConnection.Value;
            }
        }
        private static bool? _forceSecureConnection;

        public static string ConnectionString => "FullUri=file::memory:?cache=shared";
        public static bool DisableProfilingResults { get; set; }
        public const char DIR_SEPERATOR = '\\';
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            InitProfilerSettings();
            InitializeMPNetConnection();
        }

        private void InitializeMPNetConnection()
        {
            try
            {
                // Create and set the logon information (note comment in web.config -- here would be the place to
                // decrypt/unhash the user/password from the config file).
                NetworkCredential ourCredentials = new NetworkCredential(ConfigurationManager.AppSettings["MPUser"],
                    ConfigurationManager.AppSettings["MPPass"]);
                IWebProxy proxy = Common.GetProxy(80);

                //RenderServiceSoap renderService = new RenderServiceSoap();
                //renderService.Proxy = proxy;
                //renderService.Credentials = ourCredentials;
                //XMLConfig config = new XMLConfig();
                //string prodOrDev = config.GetAppSetting("Environment", "common", "indicator");
                //Application["RenderService"] = renderService;
            }
            catch (Exception exp)
            {
                string msg = exp.Message;
            }
        }

        private void InitProfilerSettings()
        {
            // A powerful feature of the MiniProfiler is the ability to share links to results with other developers.
            // by default, however, long-term result caching is done in HttpRuntime.Cache, which is very volatile.
            // 
            // Let's rig up serialization of our profiler results to a database, so they survive app restarts.
            MiniProfiler.Configure(new MiniProfilerOptions
            {
                // Sets up the route to use for MiniProfiler resources:
                // Here, ~/profiler is used for things like /profiler/mini-profiler-includes.js)
                RouteBasePath = "~/profiler",

                // Setting up a MultiStorage provider. This will store results in the MemoryCacheStorage (normally the default) and in SqlLite as well.
                Storage = new MultiStorageProvider(new MemoryCacheStorage(new TimeSpan(1, 0, 0))),

                // Different RDBMS have different ways of declaring sql parameters - SQLite can understand inline sql parameters just fine.
                // By default, sql parameters will be displayed.
                //SqlFormatter = new StackExchange.Profiling.SqlFormatters.InlineFormatter(),

                // These settings are optional and all have defaults, any matching setting specified in .RenderIncludes() will
                // override the application-wide defaults specified here, for example if you had both:
                //    PopupRenderPosition = RenderPosition.Right;
                //    and in the page:
                //    @MiniProfiler.Current.RenderIncludes(position: RenderPosition.Left)
                // ...then the position would be on the left on that page, and on the right (the application default) for anywhere that doesn't
                // specified position in the .RenderIncludes() call.
                PopupRenderPosition = RenderPosition.Right,  // defaults to left
                PopupMaxTracesToShow = 10,                   // defaults to 15

                // ResultsAuthorize (optional - open to all by default):
                // because profiler results can contain sensitive data (e.g. sql queries with parameter values displayed), we
                // can define a function that will authorize clients to see the JSON or full page results.
                // we use it on http://stackoverflow.com to check that the request cookies belong to a valid developer.
                ResultsAuthorize = request =>
                {
                    // you may implement this if you need to restrict visibility of profiling on a per request basis

                    // for example, for this specific path, we'll only allow profiling if a query parameter is set
                    if ("/Home/ResultsAuthorization".Equals(request.Url.LocalPath, StringComparison.OrdinalIgnoreCase))
                    {
                        return (request.Url.Query).IndexOf("isauthorized", StringComparison.OrdinalIgnoreCase) >= 0;
                    }

                    // all other paths can check our global switch
                    return !DisableProfilingResults && IsUserAllowedToSeeMiniProfilerUI(request);
                },

                // ResultsListAuthorize (optional - open to all by default)
                // the list of all sessions in the store is restricted by default, you must return true to allow it
                ResultsListAuthorize = request =>
                {
                    // you may implement this if you need to restrict visibility of profiling lists on a per request basis 
                    return IsUserAllowedToSeeMiniProfilerUI(request); // all requests are legit in our happy world
                },

                // Stack trace settings
                StackMaxLength = 256, // default is 120 characters

                // (Optional) You can disable "Connection Open()", "Connection Close()" (and async variant) tracking.
                // (defaults to true, and connection opening/closing is tracked)
                TrackConnectionOpenClose = true
            }
            // Optional settings to control the stack trace output in the details pane
            .ExcludeType("SessionFactory")  // Ignore any class with the name of SessionFactory)
            .ExcludeAssembly("NHibernate")  // Ignore any assembly named NHibernate
            .ExcludeMethod("Flush")         // Ignore any method with the name of Flush
            .AddViewProfiling()              // Add MVC view profiling
            );

            MiniProfilerEF6.Initialize();
        }

        void Application_Error(Object sender, EventArgs e)
        {
            Exception LastError = Server.GetLastError();
            List<string> parms = new List<string>();

            try
            {
                parms.Add(string.Format("Client IP: {0}", Request.ServerVariables["REMOTE_ADDR"].ToString()));
                parms.Add(string.Format("User: {0}", Request.ServerVariables["LOGON_USER"].ToString()));
                parms.Add(string.Format("QueryString: {0}", Server.UrlDecode(Request.QueryString.ToString())));
                parms.Add(string.Concat("Client IP: ", Request.UserHostAddress));
                parms.Add(string.Concat("Url: ", Request.Url));
                string referrer = (Request.UrlReferrer != null) ? Request.UrlReferrer.ToString() : "N/A";
                parms.Add(string.Concat("Referrer: ", referrer));
                parms.Add(string.Concat("Is Secure: ", Request.IsSecureConnection));
                parms.Add(string.Format("Browser: {0} {1}", Request.Browser.Browser, Request.Browser.Version));
                parms.Add(string.Concat("Is Crawler?: ", Request.Browser.Crawler));
                parms.Add(string.Concat("OS: ", Request.Browser.Platform));

                if (Request.Form.AllKeys.Length > 0)
                {
                    foreach (string key in Request.Form.AllKeys)
                    {
                        if (key != null)
                            if (!key.StartsWith("__"))
                                parms.Add(string.Format("\t{0}: {1}", key, Request.Form[key]));
                    }
                }
                else
                    parms.Add("\tNo Post Back");

            }
            finally
            {
                //create the ErrorHandler object, then Report error
                ErrorHandler Handler = new ErrorHandler(LastError);
                Handler.ReportError(string.Format("{0}", Request.ServerVariables["SCRIPT_NAME"]), parms.ToArray());
            }
        }

        void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started
        }

        void Session_End(object sender, EventArgs e)
        {
            // Code that runs when a session ends. 
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer 
            // or SQLServer, the event is not raised.
        }

        void Application_BeginRequest(Object sender, EventArgs e)
        {
            if (Application["AppPath"] == null) Application.Add("AppPath", GetAppPath());

            MiniProfiler.StartNew();

            if (!ForceSecureConnection) return;
            if (Context.Request.IsSecureConnection) return;

            // This is an insecure connection, so redirect to the secure version
            Response.Redirect(SecureUrlRewriter.CreateSecureUri(Context.Request.Url));
        }

        void Application_EndRequest()
        {
            MiniProfiler.Current?.Stop();
        }

        private bool IsUserAllowedToSeeMiniProfilerUI(HttpRequest httpRequest)
        { 
            string[] allowedUserNames = (WebConfigurationManager.AppSettings["ProfilerUser"]?.Replace(" ", "").ToUpperInvariant().Split(','));
            if (allowedUserNames == null) return false;

            // Implement your own logic for who 
            // should be able to access ~/mini-profiler-resources/results
            var principal = httpRequest.RequestContext.HttpContext.User;
            var user = principal.Identity.Name;
            var names = user.Split(DIR_SEPERATOR);
            var userName =  names.LastOrDefault();

            return allowedUserNames.Contains(userName.ToUpperInvariant());
        }

        private string GetAppPath()
        {
            string appPath = Request.ApplicationPath.Trim();

            if (!appPath.EndsWith("/"))
                appPath += "/";

            return appPath;
        }
    }
}