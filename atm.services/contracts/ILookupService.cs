using System.Collections.Generic;
using System.Threading.Tasks;
using atm.services.models;

namespace atm.services
{
    public interface ILookUpService
    {
        Task<ColumnOption> GetColumnOptionsForUserAndPage(string userName, string pageName);
        Task UpdateColumnOptionAsync(string userName, string pageName, string columnOption);
        Task<List<RouteFilter>> GetAllRouteFilters(int typeId);
        Task<List<ConceptType>> GetAllConcepts();
        Task<List<ConceptType>> GetConceptsForCenter(int centerNumber);
        Task<List<KeyValuePair<int, string>>> GetRoutesForCenterThisWeek(int centerNumber);
        Task<List<KeyValuePair<string, int>>> GetStopsForRoute(int routeId);
    }
}