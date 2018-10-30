using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using TechTalk.SpecFlow;
using System.Data;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http;
using atm.services.models;
using atm.services;
using FluentAssertions;

namespace atm.web.tests.common
{
    public class DbHelper
    {
        //Change user name as nessecary here
        public static string userName = "jkop0253";
        private static readonly string ApiUrl = "http://localhost:52555/api/";
        private static readonly SqlConnection sqlConnection = new SqlConnection(
                                      "server=ms084devsql01\\dev2014;" +
                                      "Trusted_Connection=true;" +
                                      "Integrated Security=true;" +
                                      "database=ATM; " +
                                      "connection timeout=30");
        public static readonly Random rand = new Random();

        public static void CreatePayrollForm()
        {
            //Weekending date will be the next Saturday
            var weekendingDate = DateTime.Now;
            weekendingDate = (DateTime.Now.AddDays(6 - (int)DateTime.Now.DayOfWeek));
            var departDate = DateTime.Now.AddDays(1).ToShortDateString();
            var routeNumber = TestDataHelper.GenerateRandomRouteNumber();

            using (SqlConnection sqlConnection = new SqlConnection(
                                       "server=ms084devsql01\\dev2014;" +
                                       "Trusted_Connection=true;" +
                                       "Integrated Security=true;" +
                                       "database=ATM; " +
                                       "connection timeout=30"))
            {

                try
                {
                    sqlConnection.Open();
                    SqlCommand cmdCreateVoucher = new SqlCommand("up_p_createFormForTesting", sqlConnection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmdCreateVoucher.Parameters.AddWithValue("@formTypeId", 0);
                    cmdCreateVoucher.Parameters.AddWithValue("@userName", userName);
                    cmdCreateVoucher.Parameters.AddWithValue("@sygmaCenterNo", "22");
                    cmdCreateVoucher.Parameters.AddWithValue("@routeNo", routeNumber);
                    cmdCreateVoucher.Parameters.AddWithValue("@routeDepartDate", departDate);
                    cmdCreateVoucher.Parameters.AddWithValue("@weekendingDate", weekendingDate);
                    cmdCreateVoucher.Parameters.AddWithValue("@cubes", 5);
                    cmdCreateVoucher.Parameters.AddWithValue("@stops", 5);
                    cmdCreateVoucher.Parameters.AddWithValue("@cases", 5);
                    cmdCreateVoucher.Parameters.AddWithValue("@pounds", 5);

                    var dbResponse = cmdCreateVoucher.Parameters.Add("@return_value", SqlDbType.Int);
                    dbResponse.Direction = ParameterDirection.ReturnValue;
                    cmdCreateVoucher.ExecuteNonQuery();

                    var payrollFormIdreturned = Convert.ToInt32(dbResponse.SqlValue.ToString());
                    ScenarioContext.Current["PayrollFormId"] = payrollFormIdreturned;
                }

                catch (Exception ex)
                {
                    Console.WriteLine("Error was thrown while trying to create a test payroll form.");
                    Console.WriteLine(ex);
                    throw (ex);
                }
                finally
                {
                    sqlConnection.Close();
                }
            }

        }

        public static void DeletePayrollForm()
        {
            //Only try to delete the payroll form if we created one
            if (ScenarioContext.Current["PayrollFormId"] != null)
            {
                using (SqlConnection sqlConnection = new SqlConnection(
                                     "server=ms084devsql01\\dev2014;" +
                                     "Trusted_Connection=true;" +
                                     "Integrated Security=true;" +
                                     "database=ATM; " +
                                     "connection timeout=30"))
                {

                    try
                    {
                        sqlConnection.Open();
                        SqlCommand cmdDeleteVoucher = new SqlCommand("up_p_deleteForms", sqlConnection)
                        {
                            CommandType = CommandType.StoredProcedure
                        };
                        cmdDeleteVoucher.Parameters.AddWithValue("@formIds", ScenarioContext.Current["PayrollFormId"].ToString());
                        cmdDeleteVoucher.Parameters.AddWithValue("@userName", userName);

                        var dbResponse = cmdDeleteVoucher.Parameters.Add("@ReturnVal", SqlDbType.Int);
                        dbResponse.Direction = ParameterDirection.ReturnValue;
                        cmdDeleteVoucher.ExecuteNonQuery();
                    }

                    catch (Exception ex)
                    {
                        Console.WriteLine("Error was thrown while trying to delete a test voucher.");
                        Console.WriteLine(ex);
                        throw (ex);
                    }
                    finally
                    {
                        sqlConnection.Close();
                    }
                }
            }
        }

        public static async Task<Route> GetByRouteIdAsync(string routeId)
        {
            var url = ApiUrl + "Routes/" + routeId.ToString();

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                return JsonConvert.DeserializeObject<Route>(response.Content.ReadAsStringAsync().Result);
            }
        }

        /// <summary>
        /// Querries the API to retrieve stops for given api. Then choses a random stop number that won't be a duplicate.
        /// </summary>
        /// <param name="routeId"></param>
        /// <returns></returns>
        public static int FindNonDuplicateTargetStopNumberForGivenRouteId(string routeId)
        {

            List<string> stopNumbers = GetStopNumbersFromAPIForRouteId(routeId);
            Random rnd = new Random();
            int randomEndingStopIndex = rnd.Next(1, (stopNumbers.Count() - 1));
            int randomEndingStopNumber = Int32.Parse(stopNumbers[randomEndingStopIndex]);
            //Retrieve the stop before the one we selected
            int randomBeginningStopIndex = randomEndingStopIndex - 1;
            int randomBeginningStopNumber = Int32.Parse(stopNumbers[randomBeginningStopIndex]);
            //Pick a random value betweeen these 2 stop numbers and return it
            var targetRouteNumber = rnd.Next(randomBeginningStopNumber + 1, randomEndingStopNumber - 1);
            //Make sure this number isn't a duplicate of existing stop numbers
            if (stopNumbers.Contains(targetRouteNumber.ToString()))
            {
                //Take the last stop number and add 1 to it
                targetRouteNumber = Int32.Parse(stopNumbers.ElementAt(stopNumbers.Count() - 1)) + 1;
            }

            return targetRouteNumber;
        }

        public static async Task<IEnumerable<Route>> SearchAsync(SearchRoute criteria)
        {
            var url = ApiUrl + $"Routes/search?DeepSearch={criteria.DeepSearch}";
            if (criteria.CenterNumber.HasValue && criteria.CenterNumber > 0) url = url + $"&CenterNumber={criteria.CenterNumber}";
            if (criteria.FilterStartDate.HasValue) url = url + $"&FilterStartDate={criteria.FilterStartDate.Value}";
            if (criteria.FilterEndDate.HasValue) url = url + $"&FilterEndDate={criteria.FilterEndDate.Value}";
            if (!string.IsNullOrEmpty(criteria.RouteName)) url = url + $"&RouteName={criteria.RouteName}";
            if (!string.IsNullOrEmpty(criteria.RouteNumber)) url = url + $"&RouteNumber={criteria.RouteNumber}";
            if (criteria.NearRouteId != 0) url = url + $"&NearRouteId={criteria.NearRouteId}";
            if (criteria.NearRoutePlanId != 0) url = url + $"&NearRoutePlanId={criteria.NearRoutePlanId}";
            if (criteria.BillTo != 0) url = url + $"&BillTo={criteria.BillTo}";
            if (criteria.ShipTo != 0) url = url + $"&ShipTo={criteria.ShipTo}";

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                return JsonConvert.DeserializeObject<IEnumerable<Route>>(response.Content.ReadAsStringAsync().Result);
            }
        }

        /// <summary>
        /// Will find the first route which has a stop eligible for moving and save off the route and stop detals to the ScenerioContext 
        /// </summary>
        /// <param name="centerNumber"></param>
        /// <param name="filterStartDate"></param>
        /// <param name="filterEndDate"></param>
        public static void FindRandomRouteAndStopNumberWhichHasAnOrderAndNotDispatched(int centerNumber, DateTime filterStartDate, DateTime filterEndDate)
        {
            var searchCriteria = new SearchRoute()
            {
                CenterNumber = centerNumber,
                FilterStartDate = filterStartDate.AddHours(12.0), //Setting time to noon to match UI behavior
                FilterEndDate = filterEndDate.AddHours(12.0), //Setting time to noon to match UI behavior
                DeepSearch = true
            };
            var routes = SearchAsync(searchCriteria).Result;
            var Routes_WithNonRemovedStopsWithOrders = routes.Where(r =>
                r.RouteStops.Any(s => s.RoutePlanModificationTypeId != 1 && s.RoutePlanModificationTypeId != 4) && // give you non-removed stops
                r.RouteStops.Any(s => s.OrderId != 0 && s.StopNumber != 0 && s.Weight > 0)); // give you with Order
            //Gets routes that have not been dispatched
            var Routes_WithNonRemovedStopWithOrders_NotDispatched = Routes_WithNonRemovedStopsWithOrders.Where(route => route.RouteStops.Any(s => s.TelogisArrivalDeliveryDateTime.HasValue) == false);
            Routes_WithNonRemovedStopWithOrders_NotDispatched.Count().Should().BeGreaterOrEqualTo(1, "should have found a route with at least 1 stop that's eligible for a stop move");
            //Randomly select one of the routes
            var selectedRoute = Routes_WithNonRemovedStopWithOrders_NotDispatched.ElementAt(rand.Next(Routes_WithNonRemovedStopWithOrders_NotDispatched.Count() - 1));
            var selectedRoute_WithOnlyNonRemovedStopsAndWithOrders = selectedRoute.RouteStops.Where(r => r.OrderId != 0 && //Give you stops with an order
            (r.RoutePlanModificationTypeId != 1 && r.RoutePlanModificationTypeId != 4 && r.Weight > 0)); //Give you non-removed stops
            var selectedStop = selectedRoute_WithOnlyNonRemovedStopsAndWithOrders.First(); //Select the first stop from the list
            //Save off details of the selectedStop for use later
            ScenarioContext.Current["SourceRouteDispatchDay"] = (selectedStop.AdjustedDeliveryDateTime);
            ScenarioContext.Current["SourceRouteNumber"] = selectedStop.RouteNumber;
            ScenarioContext.Current["SourceRouteId"] = selectedStop.RouteId;
            ScenarioContext.Current["SourceStopNumber"] = selectedStop.StopNumber;
            ScenarioContext.Current["SourceStopInitialTotalWeight"] = selectedStop.Weight;
            ScenarioContext.Current["SourceStopInitialTotalCubes"] = selectedStop.Cubes;
            ScenarioContext.Current["SourceStopInitialTotalCases"] = selectedStop.Cases;
        }

        public static Tuple<int, string> FindRandomRouteWhichHasOrders(int centerNumber, DateTime filterStartDate, DateTime filterEndDate, int? routeIdToExclude = null)
        {
            var searchCriteria = new SearchRoute()
            {
                CenterNumber = centerNumber,
                FilterStartDate = filterStartDate.AddHours(12.0), //Setting time to noon to match UI behavior
                FilterEndDate = filterEndDate.AddHours(12.0), //Setting time to noon to match UI behavior
                DeepSearch = false
            };
            var routes = SearchAsync(searchCriteria).Result;
            if (routeIdToExclude != null)
            {
                //Remove the route we want to exclude from the possible list of routes
                routes = routes.Where(route => route.RouteId != routeIdToExclude);
            }
            var routes_WithOrders = routes.Where(route => route.NumberOfOrders > 1); //Returns routes where more than 1 order exists
            routes_WithOrders.Count().Should().BeGreaterOrEqualTo(1, $"should have found at least 1 route which contained at 1+ orders for center number {centerNumber}. Please try changing centers and rerunning test.");
            var selectedRoute = routes_WithOrders.ElementAt(rand.Next(routes_WithOrders.Count() - 1)); //Randomly selects one of the routes with orders
            return Tuple.Create(selectedRoute.RouteId, selectedRoute.RouteNumber);
        }

        public static Tuple<int, string> FindRandomRouteWithOrdersAndIsDispatched(int centerNumber, DateTime filterStartDate, DateTime filterEndDate)
        {
            var searchCriteria = new SearchRoute()
            {
                CenterNumber = centerNumber,
                FilterStartDate = filterStartDate.AddHours(12.0), //Setting time to noon to match UI behavior
                FilterEndDate = filterEndDate.AddHours(12.0), //Setting time to noon to match UI behavior
                DeepSearch = true
            };
            var routes = SearchAsync(searchCriteria).Result;
            var routes_WithOrders = routes.Where(route => route.NumberOfOrders > 1); //Returns routes where more than 1 order exists
            routes_WithOrders.Count().Should().BeGreaterOrEqualTo(1, $"should have found at least 1 route which contained at 1+ orders for center number {centerNumber}. Please try changing centers and rerunning test.");
            var route_WithOrders_AndDispatched = routes_WithOrders.Where(route => (route.RouteStops.OrderBy(s => s.StopNumber).FirstOrDefault().TelogisArrivalDeliveryDateTime.HasValue) || (route.RouteStops.Any(s => s.TelogisArrivalDeliveryDateTime.HasValue)));
            var selectedRoute = route_WithOrders_AndDispatched.ElementAt(rand.Next(route_WithOrders_AndDispatched.Count() - 1)); //Randomly selects one of the routes
            return Tuple.Create(selectedRoute.RouteId, selectedRoute.RouteNumber);
        }

        public static void FindAnEligibleTargetRouteNumber(int centerNumber, DateTime filterStartDate, DateTime filterEndDate, int sourceRouteId)
        {
            var searchCriteria = new SearchRoute()
            {
                CenterNumber = centerNumber,
                FilterStartDate = filterStartDate, //Setting time to noon to match UI behavior
                FilterEndDate = filterEndDate, //Setting time to noon to match UI behavior
                DeepSearch = false
            };
            var routes = SearchAsync(searchCriteria).Result;
            //Finds all routes excluding the given source route id
            var routes_ExcludingSourceRoute = routes.Where(r => r.RouteId != sourceRouteId); // give you all routes not including the source route id
            var routes_ExcludingSourceRoute_WithOrders = routes.Where(r => r.HasOrders.ToString() == "RouteHasSomeOrders" || r.HasOrders.ToString() == "RouteHasAllOrders"); // give you routes with orders
            //Gets routes that have not been dispatched
            var routes_ExcludingSourceRoute_WithOrders_AndNonDispatched = routes_ExcludingSourceRoute_WithOrders.Where(r => r.HasDispatched == false); // give you routes with at least 1 stop
            routes_ExcludingSourceRoute_WithOrders_AndNonDispatched.Count().Should().BeGreaterOrEqualTo(1, "should have found at least 1 eligible source route");
            var sourceRoute = routes_ExcludingSourceRoute_WithOrders_AndNonDispatched.ElementAt(rand.Next(routes_ExcludingSourceRoute_WithOrders_AndNonDispatched.Count() - 1));
            ScenarioContext.Current["TargetRouteId"] = sourceRoute.RouteId;
            ScenarioContext.Current["TargetRouteNumber"] = sourceRoute.RouteNumber;
        }

        public static List<string> GetStopNumbersFromAPIForRouteId(string routeId)
        {
            //Retrieve details for the given routeId from the API
            var route = GetByRouteIdAsync(routeId).Result;
            return route.RouteStops.Select(s => s.StopNumber.ToString()).ToList();
        }

        /// <summary>
        /// Returns a list of all stop numbers which do not have a removed stop status
        /// </summary>
        /// <param name="routeId"></param>
        /// <returns></returns>
        public static List<string> GetAllStopNumberExcludingRemovedStopFromAPIForRouteId(string routeId)
        {
            //Retrieve details for the given routeId from the API
            var route = GetByRouteIdAsync(routeId).Result;
            var nonRemovedStops = route.RouteStops.Where(stop => stop.RoutePlanModificationTypeId != 1 && stop.StopNumber != 0); //gets non removed stops and excludes stop 0
            return nonRemovedStops.Select(s => s.StopNumber.ToString()).ToList();
        }

        public static void FindValidBillToShipToValues(int centerNumber, DateTime filterStartDate, DateTime filterEndDate)
        {
            var searchCriteria = new SearchRoute()
            {
                CenterNumber = centerNumber,
                FilterStartDate = filterStartDate.AddHours(12.0), //Setting time to noon to match UI behavior
                FilterEndDate = filterEndDate.AddHours(12.0), //Setting time to noon to match UI behavior
                DeepSearch = true
            };
            var routes = SearchAsync(searchCriteria).Result;
            var targetRoute = routes.Where(route => route.RouteStops.Count() > 4).First(); //get routes with at least 4 stops
            var targetStop = targetRoute.RouteStops.ElementAt(3); //get stop at position 3

            ScenarioContext.Current["ExpectedRouteNumber"] = targetRoute.RouteNumber; //Save off the route number for use later
            ScenarioContext.Current["ValidBillTo"] = Convert.ToInt32(targetStop.BillTo);
            ScenarioContext.Current["ValidShipTo"] = Convert.ToInt32(targetStop.ShipTo);
        }

        public static List<string> GetExpectedRoutesWithGivenBillToAndShipTo(int centerNumber, DateTime filterStartDate, DateTime filterEndDate, int billTo, int shipTo)
        {
            var searchCriteria = new SearchRoute()
            {
                CenterNumber = centerNumber,
                FilterStartDate = filterStartDate.AddHours(12.0), //Setting time to noon to match UI behavior
                FilterEndDate = filterEndDate.AddHours(12.0), //Setting time to noon to match UI behavior
                BillTo = billTo,
                ShipTo = shipTo,
                DeepSearch = false
            };
            var routes = SearchAsync(searchCriteria).Result;
            return routes.Select(route => route.RouteNumber).ToList();
        }

        public static string GenerateOrderClassificationForRouteId(string routeId)
        {
            var routeFromApi = GetByRouteIdAsync(routeId).Result;
            List<bool> stopHasOrder = new List<bool>();
            foreach (var r in routeFromApi.RouteStops)
            {
                //Skip stop number 0, and also skip any manual removed or system removed stops
                if (r.RoutePlanModificationTypeId != 1 && r.RoutePlanModificationTypeId != 4 && r.StopNumber != 0)
                {
                    stopHasOrder.Add((r.OrderId != 0));
                }
            }
            string orderClassificationForRoute = "";

            //If there were no stops in the route
            if (stopHasOrder.Count == 0)
            {
                orderClassificationForRoute = "No Orders";
            }
            //If all stops have an order
            else if (stopHasOrder.All(stop => stop == true))
            {
                orderClassificationForRoute = "All Orders Only";
            }
            //If NO stops have an order
            else if (stopHasOrder.All(stop => stop == false))
            {
                orderClassificationForRoute = "No Orders";
            }
            else
            {
                orderClassificationForRoute = "Some Orders";
            }
            return orderClassificationForRoute;
        }

        public static IStop GetStopInformationForGivenRouteId(string routeId, string stopNumber)
        {
            var route = GetByRouteIdAsync(routeId).Result;
            return route.RouteStops.Where(s => s.StopNumber.ToString() == stopNumber).FirstOrDefault();
        }

        /// <summary>
        /// Will calculate the Total Cubes, Total Cases, and Total LBs by summing the capacity for the stops
        /// </summary>
        /// <param name="routeId"></param>
        /// <returns>TotalCubes,TotalCases,TotalLbs</returns>
        public static Tuple<decimal, decimal, decimal> CalculateExpectedCapacityTotalsForRouteId(string routeId)
        {
            decimal totalCubes = 0;
            decimal totalCases = 0;
            decimal totalLbs = 0;
            var route = GetByRouteIdAsync(routeId).Result;
            var routeStops = route.RouteStops;
            //Sum the Cubes, Cases, and Weight from all stops
            foreach (var stop in routeStops)
            {
                totalCases += stop.Cases;
                totalCubes += stop.Cubes;
                totalLbs += stop.Weight;
            }
            //Return the new capacity totals
            return Tuple.Create(totalCubes, totalCases, totalLbs);
        }

        public static Tuple<int, int> CalculateExpectedTotalOrdersVersusTotalStopsForGivenRouteId(string routeId)
        {
            var route = GetByRouteIdAsync(routeId).Result;
            var routeStops = route.RouteStops;
            var routeStopsWithOrder = routeStops.Where(stop => stop.OrderId > 0);
            return Tuple.Create(routeStopsWithOrder.Count(), route.NumberOfStops);
        }

        public static Tuple<int, string, int> FindRandomRouteAndStopNumberWhichHasRemovedStatus(int centerNumber, DateTime filterStartDate, DateTime filterEndDate)
        {
            var searchCriteria = new SearchRoute()
            {
                CenterNumber = centerNumber,
                FilterStartDate = filterStartDate.AddHours(12.0), //Setting time to noon to match UI behavior
                FilterEndDate = filterEndDate.AddHours(12.0), //Setting time to noon to match UI behavior
                DeepSearch = true
            };
            var routes = SearchAsync(searchCriteria).Result;
            var listOfRoutes_WithRemovedStops = routes.Where(r =>
                r.RouteStops.Any(s => s.RoutePlanModificationTypeId == 1 || s.RoutePlanModificationTypeId == 4)); // give you removed stops
            listOfRoutes_WithRemovedStops.Count().Should().BeGreaterOrEqualTo(1, "did not find any routes with at least 1 removed stop");
            var rand = new Random();
            //Randomly select one of the routes
            var selectedRoute = listOfRoutes_WithRemovedStops.ElementAt(rand.Next(listOfRoutes_WithRemovedStops.Count() - 1));

            var selectedRoute_ListOfRemovedStops = selectedRoute.RouteStops.Where(stop => stop.RoutePlanModificationTypeId == 1 || stop.RoutePlanModificationTypeId == 4); //Give you only removed stops in selectedRoute
            //Randomly select a removed stop from the selectedRoute
            var selectedStop = selectedRoute_ListOfRemovedStops.ElementAt(rand.Next(0, selectedRoute_ListOfRemovedStops.Count() - 1));
            var routeId = selectedStop.RouteId;
            var routeNumber = selectedStop.RouteNumber;
            var stopNumber = Convert.ToInt32(selectedStop.StopNumber);
            return Tuple.Create(routeId, routeNumber, stopNumber);
        }
    }
}
