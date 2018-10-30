using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using atm.services.models;

namespace atm.web.tests.common
{
    public class TestDataHelper
    {

        public static string GenerateRandomRouteNumber()
        {
            Random rnd = new Random();
            //Generate the first 3 digits of the route number
            var routeNumberValue = rnd.Next(100, 999).ToString();
            //Generate the int value representing a random value between a-z
            var charNumber = rnd.Next(0, 26); // Zero to 25
            routeNumberValue += (char)('a' + charNumber);
            return routeNumberValue;
        }

        public static ListDictionary GetDepartDayNumbers()
        {
            ListDictionary departDayNumbers = new ListDictionary
            {
                { "Sunday", "100" },
                { "Monday", "200" },
                { "Tuesday", "300" },
                { "Wednesday", "400" },
                { "Thursday", "500" },
                { "Friday", "600" },
                { "Saturday", "700" }
            };
            return departDayNumbers;
        }

        public static string GetFormattedDateRangeTextForValue(string value)
        {
            //Used to add or subtract days from today
            var daysAwayFromToday = 0;
            var today = DateTime.Now;
            switch (value)
            {
                case "Today":
                    daysAwayFromToday = 0;
                    break;
                case "Yesterday":
                    daysAwayFromToday = -1;
                    break;
                case "Tomorrow":
                    daysAwayFromToday = 1;
                    break;
                default:
                    //Value doesn't need formatting so return it
                    return value;
            }
            var targetDayOfWeek = today.AddDays(daysAwayFromToday).DayOfWeek;
            //Get the depart day number cooresponding to targetDayOfWeek
            var departDayNumber = GetDepartDayNumbers()[targetDayOfWeek.ToString()];
            return $"{value} - {departDayNumber}s";
        }

        public static void SaveExpectedValuesForRouteId(int routeId)
        {
            var routeData = DbHelper.GetByRouteIdAsync(routeId.ToString()).Result;
            Dictionary<string, Dictionary<string, string>> selectedRoutesData = new Dictionary<string, Dictionary<string, string>>();
            //If there are already selected routes
            if (ScenarioContext.Current.ContainsKey("SelectedRoutes"))
            {
                //Then get the SelectedRoutes to make sure we don't overwrite them
                ScenarioContext.Current.TryGetValue("SelectedRoutes", out selectedRoutesData);
            }
            //Create a dictionary containing all the info we need for this route
            Dictionary<string, string> currentRouteDate = new Dictionary<string, string>();
            currentRouteDate.Add("RouteNumber", routeData.RouteNumber);
            currentRouteDate.Add("RouteId", routeData.RouteId.ToString());
            currentRouteDate.Add("NumberOfStops", routeData.NumberOfStops.ToString());
            currentRouteDate.Add("AdjustedDispatchTime", routeData.AdjustedDispatchTime.ToString());
            currentRouteDate.Add("NumberOfOrders", routeData.NumberOfOrders.ToString());
            //Add the data for this current route to the existing list of selected routes
            selectedRoutesData.Add(routeId.ToString(), currentRouteDate);
            //Save the selectedRoutesData to the ScenarioContext
            ScenarioContext.Current["SelectedRoutes"] = selectedRoutesData;
            //Save off the id of the last selected route id for quick access later
            ScenarioContext.Current["LastSelectedRouteId"] = routeId;
        }
    }
}
