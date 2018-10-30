using System.Collections.Generic;
using System.Threading.Tasks;
using atm.services.models;
using atm.services.models.payroll;

namespace atm.services
{
    public interface IDriverService
    {
        Task<List<Driver>> GetDriversAsync();
        Task<List<DriverHelper>> GetDriverHelpersAsync();
        Task<List<PayrollForm>> GetFormsAsync(FormsQueryRequest request);
        Task<FormCriteria> GetFormCriteriaAsync(string userName);
    }
}