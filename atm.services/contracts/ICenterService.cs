using System.Collections.Generic;
using System.Threading.Tasks;
using atm.services.models;

namespace atm.services
{
	public interface ICenterService
	{
		Task<List<BasicCenter>> GetAll(string userName);
		Task<CenterLocation> GetLocationByNoAsync(int sygmaCenterNo);
	}
}