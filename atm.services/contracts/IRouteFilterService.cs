using System.Collections.Generic;
using System.Threading.Tasks;
using atm.services.models;

namespace atm.services
{
    public interface IRouteFilterService
    {
        Task<List<RouteFilter>> GetAllValuesForFilter(int routeFilterType);
        Task<List<ConceptType>> GetAllConcepts();
        Task<List<ConceptType>> GetConceptsForCenter(int SygmaCenterNumber);
    }
}