using Ninject.Web.Common.WebHost;
using SygmaFramework;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(atm.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(atm.App_Start.NinjectWebCommon), "Stop")]

namespace atm.App_Start
{
	using System;
	using System.Web;

	using Microsoft.Web.Infrastructure.DynamicModuleHelper;

	using Ninject;
	using Ninject.Web.Common;
	using atm.services;

	public static class NinjectWebCommon
	{
		private static readonly Bootstrapper bootstrapper = new Bootstrapper();

		/// <summary>
		/// Starts the application
		/// </summary>
		public static void Start()
		{
			DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
			DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
			bootstrapper.Initialize(CreateKernel);
		}

		/// <summary>
		/// Stops the application.
		/// </summary>
		public static void Stop()
		{
			bootstrapper.ShutDown();
		}

		/// <summary>
		/// Creates the kernel that will manage your application.
		/// </summary>
		/// <returns>The created kernel.</returns>
		private static IKernel CreateKernel()
		{
			var kernel = new StandardKernel();
			try
			{
				kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
				kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

				RegisterServices(kernel);
				return kernel;
			}
			catch
			{
				kernel.Dispose();
				throw;
			}
		}

		/// <summary>
		/// Load your modules or register your services here!
		/// </summary>
		/// <param name="kernel">The kernel.</param>
		private static void RegisterServices(IKernel kernel)
		{
			kernel.Bind<IAuthorizationService>().To<AuthorizationService>();
			kernel.Bind<IMenuService>().To<MenuService>();
			kernel.Bind<IDriverService>().To<DriverService>();
			kernel.Bind<ICenterService>().To<CenterService>();
			kernel.Bind<IPayScaleService>().To<PayScaleService>();
			kernel.Bind<IRouteService>().To<RouteService>();
			kernel.Bind<ILookUpService>().To<LookUpService>();
            kernel.Bind<ICommentService>().To<CommentService>();
            kernel.Bind<INotificationService>().To<NotificationService>();
        }
	}
}
