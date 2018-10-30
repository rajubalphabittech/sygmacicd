using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

namespace atm.services.models.payroll
{
    public class FormsQueryRequest
    {
        public string FormId { get; set; }
        public int SygmaCenterNo { get; set; }
        public string RouteNo { get; set; }
        public DateTime WeekEnding { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int StatusId { get; set; }
        public int FormTypeId { get; set; }
        public bool ActualsUpdated { get; set; }
        public string EmployeeString { get; set; }
        public string TractorString { get; set; }
        public string TrailorString { get; set; }
        public string UserName { get; set; }
    }

    public class FormCriteria
    {
        public List<FormStatus> Status { get; set; }
        public List<FormType> FormType { get; set; }
        public List<SygmaCenter> Centers { get; set; }
    }

    public class SygmaCenter
    {
        public int SygmaCenterNo { get; set; }
        public string CenterDisplay { get; set; }
    }

    public class FormType
    {
        public int FormTypeId { get; set; }
        public string FormTypeDescription { get; set; }
        public bool Editable { get; set; }
    }

    public class FormStatus
    {
        public int StatusId { get; set; }
        public string StatusDescription { get; set; }
    }


}