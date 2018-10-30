using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using atm.services.models;
using Newtonsoft.Json;

namespace atm.Models
{
    public class RouteAggregateCommentViewModel
    {

        public int RouteId { get; set; }
        public string RouteNumber { get; set; }
        public int RoutePlanId { get; set; }
        public int StopNumber { get; set; }
        public List<RouteCommentViewModel> Comments { get; set; }
    }
    public class RouteCommentListViewModel : List<RouteCommentViewModel>
    {
        public RouteCommentListViewModel(IEnumerable<Comment> comments, int billTo, int shipTo, int centerNumber, string routePlanId, int stopNumber, string screen)
        {
            RoutePlanId = routePlanId;
            BillTo = billTo;
            ShipTo = shipTo;
            StopNumber = stopNumber;
            CenterNumber = centerNumber;
            Screen = screen;
            foreach (var comment in comments)
            {
                Add(new RouteCommentViewModel(comment));
            }
        }

        public string RoutePlanId { get; set; }
        public int BillTo { get; set; }
        public int ShipTo { get; set; }
        public int StopNumber { get; set; }
        public int? CenterNumber { get; set; }
        public string Screen { get; set; }
    }
    public class RouteCommentViewModel
    {
        public RouteCommentViewModel(Comment comment)
        {
            CommentId = comment.CommentId;
            RoutePlanId = comment.SecondaryRecordId;
            BillTo = int.Parse(comment.PrimaryRecordId.Split('-')[0]);
            ShipTo = int.Parse(comment.PrimaryRecordId.Split('-')[1]);
            CenterNumber = comment.CenterNumber;
            ShortComment = comment.ShortComment;
            LongComment = comment.LongComment;
            Status = comment.Status;
            CreatedBy = comment.CreatedBy;
            CreatedDate = comment.CreatedDate;
            UpdatedBy = comment.UpdatedBy;
            UpdatedDate = comment.UpdatedDate;
        }
        public int CommentId { get; set; }
        public string RoutePlanId { get; set; }
        public int BillTo { get; set; }
        public int ShipTo { get; set; }
        public int? CenterNumber { get; set; }
        public string ShortComment { get; set; }
        public string LongComment { get; set; }
        public int Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<DateTime> UpdatedDate { get; set; }
        public string CreatedDate_MMDDHHMM => CreatedDate.ToString("MM/dd HH:mm", CultureInfo.InvariantCulture);
    }

    public class CreateRouteCommentModelWithOptions : CreateRouteCommentModel
    {
        public IEnumerable<SelectListItem> RouteList { get; set; }

    }

    public class BasicStopModel
    {
        public int BillTo { get; set; }
        public int ShipTo { get; set; }
        public int CenterNumber { get; set; }
        public int RoutePlanId { get; set; }
        public int StopNumber { get; set; }
    }

    public class CreateRouteCommentModel : BasicStopModel
    {
        public string LongComment { get; set; }
        public bool IsInternal { get; set; }
        public string Category { get; set; }
        public string Screen { get; set; }
    }

    public class UpdateRouteCommentModel : BasicStopModel
    {
        public int CommentId { get; set; }
        public string LongComment { get; set; }
        public bool IsInternal { get; set; }
        public string Category { get; set; }
        public string Screen { get; set; }
    }
}