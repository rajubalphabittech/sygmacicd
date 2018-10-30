using System;
using System.Data;

namespace atm.services.models.payroll
{
    public class PayrollForm
    {
        public int FormId { get; set; }
        public int StatusId { get; set; }
        public string StatusDescription { get; set; }
        public int FormTypeId { get; set; }
        public string FormTypeDescription { get; set; }
        public int SygmaCenterNo { get; set; }
        public string CenterDescription { get; set; }
        public string RouteNo { get; set; }
        public DateTime RouteDepartDate { get; set; }
        public DateTime FiscalWeekEnding { get; set; }
        public int Cases { get; set; }
        public int Cubes { get; set; }
        public int Stops { get; set; }
        public decimal Miles { get; set; }
        public int Pounds { get; set; }
        public bool ActualsUpdated { get; set; }
        public string Employee1 { get; set; }
        public string Employee2 { get; set; }
        public string TrailerSygmaId { get; set; }
        public string VehicleName { get; set; }
        public int? LastUpdatedBy { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public int AddedBy { get; set; }
        public DateTime? AddedDate { get; set; }
        public string DriverId { get; set; }
        public string DriverHelperId { get; set; }
    }
}