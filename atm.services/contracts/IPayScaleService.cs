using System.Collections.Generic;
using System.Threading.Tasks;
using atm.services.models;

namespace atm.services
{
	public interface IPayScaleService
	{
		Task<List<PayScale>> GetAll();
        Task<List<PayScaleRate>> GetRatesForCenterAndPayScale(int centerNo, int payScale);

    }
}