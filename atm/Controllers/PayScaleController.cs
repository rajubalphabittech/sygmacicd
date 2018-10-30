using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using atm.helpers;
using atm.Helpers;
using atm.Models;
using atm.services;

namespace atm.Controllers
{
	public class PayScaleController : AtmControllerBase
	{
		public ICenterService CenterService { get; private set; }
		public IPayScaleService PayScaleService { get; private set; }

		public PayScaleController(
				IAuthorizationService authorizationService,
				ICenterService centerService,
				IPayScaleService payScaleScaleService) : base(authorizationService)
		{
			CenterService = centerService;
			PayScaleService = payScaleScaleService;
		}

		[AtmAuthorize(Path = SecuredFeatures.PAYSCALE_READONLY)]
		public async Task<ActionResult> ReadOnly()
		{
			ViewBag.Title = "View Pay Rates";
			var payRateViewModel = new PayRateViewModel
			{
				CenterSelectList = new SelectList(await CenterService.GetAll(UserName), "SygmaCenterNo", "Center"),
				PayScaleSelectList = new SelectList((await PayScaleService.GetAll()).OrderBy(ps => ps.DisplayOrder), "PayScaleId", "PayScaleDisplay")
			};
			return View(payRateViewModel);
		}

		[AtmAuthorize(Path = SecuredFeatures.PAYSCALE_READONLY)]
		public async Task<ActionResult> Detail(int sygmaCenterNo, int payScaleId)
		{
			var payScaleRatesViewModel = new PayScaleRateListViewModel(await PayScaleService.GetRatesForCenterAndPayScale(sygmaCenterNo, payScaleId));
			return PartialView("_detail", payScaleRatesViewModel);
		}
	}
}