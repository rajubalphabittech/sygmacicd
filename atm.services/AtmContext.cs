using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using atm.services.models;

namespace atm.services
{
	public class AtmContext : DbContext
    {
		public AtmContext() : base("atm")
		{
            Database.CommandTimeout = WebConfigurationManager.AppSettings["CommandTimeout"] == null ? 60 : int.Parse(WebConfigurationManager.AppSettings["CommandTimeout"]);
        }

		public AtmContext(string connectionString) : base(connectionString)
		{
            Database.CommandTimeout = WebConfigurationManager.AppSettings["CommandTimeout"] == null ? 60 : int.Parse(WebConfigurationManager.AppSettings["CommandTimeout"]);
        }
    }
}
