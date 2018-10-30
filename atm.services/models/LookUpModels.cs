using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace atm.services.models
{
    public class ConceptType
    {
        public byte ConceptId { get; set; }
        public int SygmaCenterNo { get; set; }

        public ConceptType(string concept)
        {
            Concept = concept.Trim();
        }

        public string Concept { get; }
    }

    public class RouteFilter
    {
        public int RouteFilterId { get; set; }
        public string Text { get; set; }
        public string Value { get; set; }
        public int DisplayOrder { get; set; }
        public bool Selected { get; set; }
        public RouteFilterTypes RouteFilterTypeId { get; set; }
        public byte IsActive { get; set; }
        public DateTime RecordCreatedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
    }

    public class UpdateColumnOption
    {
        public string UserName { get;  set; }
        public string PageName { get; set; }

    }

    public class ColumnOption
    {
        //Actually the Selected Columns list
        public virtual List<Column> Columns { get; set; }
        public virtual List<AvailableColumn> AvailableColumns { get; set; }
    }

    public class Column
    {
        [JsonProperty("ID")]
        public string ID { get; set; }
        [JsonProperty("Width")]
        public string Width { get; set; }
        [JsonProperty("ColumnIdentifier")]
        public string ColumnIdentifier { get; set; }
        [JsonProperty("Visible")]
        public string Visible { get; set; }
        [JsonProperty("DisplayOrder")]
        public string DisplayOrder { get; set; }
    }

    public class AvailableColumn
    {
        public string ID { get; set; }
        public string ColumnIdentifier { get; set; }
    }
}
