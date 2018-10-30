using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace atm.services.models
{
    public class Notification
    {
        [Required]
        public string From { get; set; }

        [Required]
        public string[] To { get; set; }

        public string[] Cc { get; set; }

        public string Subject { get; set; }

        [Required]
        public string Body { get; set; }
    }
}
