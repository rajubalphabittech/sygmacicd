using atm.services.models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using atm.helpers;
using System.Configuration;

namespace atm.Models
{
    public class RouteTrackerViewModel
    {
        public SelectList CenterSelectList { get; set; }
        [Display(Name = "Show Modified Routes Only")]
        public bool ShowModifiedRoutesOnly { get; set; }
        [Display(Name = "Cascade Updates")]
        public bool CascadeUpdates { get; set; }
        [Display(Name = "Route Status")]
        public MultiSelectList FilterByDispatchDay { get; set; }
        public SelectList DateRangeList { get; set; }
        public SelectList ModifiedTypeSelectList { get; set; }
        public SelectList ConceptTypeList { get; set; }
    }

    public class RouteTrackerColumnOptionViewModel
    {
        public ColumnOptionViewModel RouteColumnOption { get; set; }
        public ColumnOptionViewModel StopColumnOption { get; set; }
    }

    public class ColumnOptionViewModel
    {
        public ColumnOptionViewModel(ColumnOption columnOption, ColumnTypes type)
        {
            Columns = new List<ColumnViewModel>();
            AvailableColumns = new List<AvailableColumnViewModel>();

            if (columnOption == null)
            {
                if (type == ColumnTypes.Route)
                {
                    string[] defaultRouteColumns = ConfigurationManager.AppSettings["DefaultRouteColumns"]?.Replace(" ", "").ToUpperInvariant().Split(',');
                    foreach (var e in Enum.GetValues(typeof(RouteColumns)).Cast<RouteColumns>().OrderBy(c => c).Select((value, i) => new { i, value }))
                    {
                        var value = e.value;
                        var index = e.i;
                        //if (value.GetGroupName().ToUpperInvariant() == "DEFAULT")
                        if (defaultRouteColumns.Any(c => c == ((int)value).ToString()))
                        {
                            Columns.Add(new ColumnViewModel(new Column { ID = ((int)value).ToString(), ColumnIdentifier = value.GetDisplayName().ToString(), Visible = "true", Width = "0", DisplayOrder = (index + 1).ToString() }));
                        }
                        else
                        {
                            AvailableColumns.Add(new AvailableColumnViewModel(new AvailableColumn { ID = ((int)value).ToString(), ColumnIdentifier = value.GetDisplayName().ToString() }));
                        }
                    }
                }
                else
                {
                    string[] defaultStopColumns = ConfigurationManager.AppSettings["DefaultStopColumns"]?.Replace(" ", "").ToUpperInvariant().Split(',');
                    foreach (var e in Enum.GetValues(typeof(StopColumns)).Cast<StopColumns>().OrderBy(c => c).Select((value, i) => new { i, value }))
                    {
                        var value = e.value;
                        var index = e.i;
                        //if (value.GetGroupName().ToUpperInvariant() == "DEFAULT")
                        if (defaultStopColumns.Any(c => c == ((int)value).ToString()))
                        {
                            Columns.Add(new ColumnViewModel(new Column { ID = ((int)value).ToString(), ColumnIdentifier = value.GetDisplayName().ToString(), Visible = "true", Width = "0", DisplayOrder = (index + 1).ToString() }));
                        }
                        else
                        {
                            AvailableColumns.Add(new AvailableColumnViewModel(new AvailableColumn { ID = ((int)value).ToString(), ColumnIdentifier = value.GetDisplayName().ToString() }));
                        }
                    }
                }
            }
            else
            {
                if (type == ColumnTypes.Route)
                {
                    string[] defaultRouteColumns = ConfigurationManager.AppSettings["DefaultRouteColumns"]?.Replace(" ", "").ToUpperInvariant().Split(',');
                    var newColumns = Enum.GetValues(typeof(RouteColumns))
                        .Cast<RouteColumns>()
                        .Select(e => e.GetDisplayName())
                        .Except(
                            columnOption.Columns.Select(c => c.ColumnIdentifier).Union(columnOption.AvailableColumns.Select(c => c.ColumnIdentifier))
                        );

                    foreach (var c in newColumns)
                    {
                        var newEnum = Enum.GetValues(typeof(RouteColumns)).Cast<RouteColumns>().First(e => e.GetDisplayName() == c);
                        //if (newEnum.GetGroupName().ToUpperInvariant() == "DEFAULT")
                        if (defaultRouteColumns.Any(co => co == ((int)newEnum).ToString()))
                        {
                            Columns.Add(new ColumnViewModel(new Column { ID = ((int)newEnum).ToString(), ColumnIdentifier = newEnum.GetDisplayName().ToString(), Visible = "true", Width = "0", DisplayOrder = (columnOption.Columns.Count()).ToString() }));
                        }
                        else
                        {
                            AvailableColumns.Add(new AvailableColumnViewModel(new AvailableColumn { ID = ((int)newEnum).ToString(), ColumnIdentifier = newEnum.GetDisplayName().ToString() }));
                        }
                    }
                }
                else
                {
                    string[] defaultStopColumns = ConfigurationManager.AppSettings["DefaultStopColumns"]?.Replace(" ", "").ToUpperInvariant().Split(',');
                    var newColumns = Enum.GetValues(typeof(StopColumns))
                       .Cast<StopColumns>()
                       .Select(e => e.GetDisplayName())
                       .Except(
                           columnOption.Columns.Select(c => c.ColumnIdentifier).Union(columnOption.AvailableColumns.Select(c => c.ColumnIdentifier))
                       );

                    foreach (var c in newColumns)
                    {
                        var newEnum = Enum.GetValues(typeof(StopColumns)).Cast<StopColumns>().First(e => e.GetDisplayName() == c);
                        //if (newEnum.GetGroupName().ToUpperInvariant() == "DEFAULT")
                        if (defaultStopColumns.Any(co => co == ((int)newEnum).ToString()))
                        {
                            Columns.Add(new ColumnViewModel(new Column { ID = ((int)newEnum).ToString(), ColumnIdentifier = newEnum.GetDisplayName().ToString(), Visible = "true", Width = "0", DisplayOrder = (columnOption.Columns.Count()).ToString() }));
                        }
                        else
                        {
                            AvailableColumns.Add(new AvailableColumnViewModel(new AvailableColumn { ID = ((int)newEnum).ToString(), ColumnIdentifier = newEnum.GetDisplayName().ToString() }));
                        }
                    }
                }

                if (columnOption.Columns != null)
                {
                    foreach (var c in columnOption.Columns)
                    {
                        Columns.Add(new ColumnViewModel(c));
                    }
                }
                if (columnOption.AvailableColumns != null)
                {
                    foreach (var c in columnOption.AvailableColumns)
                    {
                        AvailableColumns.Add(new AvailableColumnViewModel(c));
                    }
                }
            }
        }

        public virtual List<ColumnViewModel> Columns { get; set; }
        public virtual List<AvailableColumnViewModel> AvailableColumns { get; set; }
    }

    public class ColumnViewModel
    {
        public ColumnViewModel(Column column)
        {
            ID = column.ID;
            Width = column.Width;
            ColumnIdentifier = column.ColumnIdentifier;
            Visible = column.Visible;
            DisplayOrder = column.DisplayOrder;
        }

        public string ID { get; set; }
        public string Width { get; set; }
        public string ColumnIdentifier { get; set; }
        public string Visible { get; set; }
        public string DisplayOrder { get; set; }
    }

    public class AvailableColumnViewModel
    {
        public AvailableColumnViewModel(AvailableColumn aColumn)
        {
            ID = aColumn.ID;
            ColumnIdentifier = aColumn.ColumnIdentifier;
        }
        public string ID { get; set; }
        public string ColumnIdentifier { get; set; }
    }


    public class UpdateRouteTrackerColumnOptionViewModel
    {
        public UpdateColumnOptionViewModel RouteColumnOption { get; set; }
        public UpdateColumnOptionViewModel StopColumnOption { get; set; }
    }

    public class UpdateColumnOptionViewModel
    {
        public virtual List<UpdateColumnViewModel> Columns { get; set; }
        public virtual List<UpdateAvailableColumnViewModel> AvailableColumns { get; set; }
    }

    public class UpdateColumnViewModel
    {
        public string ID { get; set; }
        public string Width { get; set; }
        public string ColumnIdentifier { get; set; }
        public string Visible { get; set; }
        public string DisplayOrder { get; set; }
    }

    public class UpdateAvailableColumnViewModel
    {
        public string ID { get; set; }
        public string ColumnIdentifier { get; set; }
    }
}