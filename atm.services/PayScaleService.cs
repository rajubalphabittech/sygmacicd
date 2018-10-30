using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using atm.services.models;

namespace atm.services
{
    public class PayScaleService : IPayScaleService
    {
        public PayScaleService() 
        {
        }

        public async Task<List<PayScale>> GetAll()
        {
            using (var Db = new AtmContext())
            {
                return await Db.Database
                .SqlQuery<PayScale>("exec [up_p_getPayScales]"
                ).ToListAsync();
            }
        }

        public async Task<List<PayScaleRate>> GetRatesForCenterAndPayScale(int centerNo, int payScale)
        {
            using (var Db = new AtmContext())
            {
                var result = await Db.Database
                .SqlQuery<PayScaleRate>("exec [up_p_getCenterPayScaleRates] @sygmaCenterNo = {0}, @payScaleId = {1}",
                centerNo,
                payScale
                ).ToListAsync();
                return result;
            }
        }
    }
}