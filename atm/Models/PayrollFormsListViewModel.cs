using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using atm.services.models;
using atm.services.models.payroll;

namespace atm.Models
{
    public class PayrollFormCriteria
    {
        public List<FormType> FormTypes { get; set; }
        public List<SygmaCenter> Centers { get; set; }
        public List<FormStatus> Status { get; set; }

        public int FormStatusId { get; set; }
        public DateTime WeekEnding { get; set; }
        public int CenterId { get; set; }
        public int FormTypeId { get; set; }
    }

    public class PayrollFormsListViewModel : List<PayrollFormViewModel>
    {
        public PayrollFormsListViewModel(IEnumerable<Driver> drivers, IEnumerable<DriverHelper> helpers, List<PayrollForm> forms)
        {
            Drivers = new List<DriverListItemModel>();
            Helpers = new List<DriverListItemModel>();

            foreach (var driver in drivers) Drivers.Add(new DriverListItemModel(driver));
            foreach (var helper in helpers) Helpers.Add(new DriverListItemModel(helper));

            foreach (var form in forms)
            {
                Add(new PayrollFormViewModel(form));
            }
        }

        public bool IsApprover { get; set; }
        public List<DriverListItemModel> Drivers { get; set; }
        public List<DriverListItemModel> Helpers { get; set; }
    }

    public class PayrollFormViewModel
    {
        public int Id { get; set; }
        public int StatusId { get; set; }
        public string StatusDescription { get; set; }
        public int FormTypeId { get; set; }
        public string FormTypeDescription { get; set; }
        public int SygmaCenterNo { get; set; }
        public string SygmaCenterDescription { get; set; }
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

        public PayrollFormViewModel(PayrollForm form)
        {
            Id = form.FormId;
            StatusId = form.StatusId;
            StatusDescription = form.StatusDescription;
            FormTypeId = form.FormTypeId;
            FormTypeDescription = form.FormTypeDescription;
            SygmaCenterNo = form.SygmaCenterNo;
            SygmaCenterDescription = form.CenterDescription;
            RouteNo = form.RouteNo;
            RouteDepartDate = form.RouteDepartDate;
            FiscalWeekEnding = form.FiscalWeekEnding;
            Cases = form.Cases;
            Cubes = form.Cubes;
            Stops = form.Stops;
            Miles = form.Miles;
            Pounds = form.Pounds;
            ActualsUpdated = form.ActualsUpdated;
            Employee1 = form.Employee1;
            Employee2 = form.Employee2;
            TrailerSygmaId = form.TrailerSygmaId;
            VehicleName = form.VehicleName;
            LastUpdatedBy = form.LastUpdatedBy;
            LastUpdatedDate = form.LastUpdatedDate;
            AddedBy = form.AddedBy;
            AddedDate = form.AddedDate;
            DriverId = form.DriverId;
            DriverHelperId = form.DriverHelperId;
        }
    }

    public class DriverListItemModel
    {

        public DriverListItemModel(Driver driver)
        {
            Id = driver.DriverId;
            Name = driver.DriverName;
            CenterNo = driver.SygmaCenterNo;
        }

        public int CenterNo { get; set; }

        public string Name { get; set; }

        public int Id { get; set; }
    }
}