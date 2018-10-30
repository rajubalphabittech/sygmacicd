using System.Collections.Generic;
using atm.services.models;
using System.Linq;
using PagedList;

namespace atm.Models
{
    public class RouteListViewModel : List<RouteViewModel>
    {
        public RouteListViewModel(IEnumerable<Route> routes)
        {
            foreach (var route in routes)
            {
                Add(new RouteViewModel(route));
            }
        }

        public string Position { get; set; }
        public RouteSortFields SortField { get; set; }
        public SortDirections SortDirection { get; set; }
        public ColumnOptionViewModel RouteColumnOption { get; set; }
        public bool StopProximitySearch { get; set; }
    }

    public class RoutePageableListViewModel : List<RouteViewModel>
    {
        public RoutePageableListViewModel(IEnumerable<Route> routes)
        {
            foreach (var route in routes)
            {
                Add(new RouteViewModel(route));
            }
        }

        public IPagedList<RouteViewModel> PagedRoutes { get; set; }
        public string Position { get; set; }
        public RouteSortFields SortField { get; set; }
        public SortDirections SortDirection { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}