using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace atm.services.models
{
    public class Comment
    {
        public int CommentId { get; set; }
        public string Screen { get; set; }
        public string PrimaryRecordId { get; set; }
        public string SecondaryRecordId { get; set; }
        public string TertieryRecordId { get; set; }
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

    public class UpdateComment
    {
        [Required]
        public int CommentId { get; set; }
        [Required]
        public string LongComment { get; set; }
        public string ShortComment { get; set; }
        public int Status { get; set; }
        [Required]
        public string UpdatedBy { get; set; }
    }

    public class AddComment
    {
        [Required]
        public string Screen { get; set; }
        [Required]
        public string LongComment { get; set; }
        public string ShortComment { get; set; }
        [Required]
        public string PrimaryRecordId { get; set; }
        public string SecondaryRecordId { get; set; }
        public string TertieryRecordId { get; set; }
        [Required]
        public int? CenterNumber { get; set; }
        public int Status { get; set; }
        [Required]
        public string CreatedBy { get; set; }
    }
}
