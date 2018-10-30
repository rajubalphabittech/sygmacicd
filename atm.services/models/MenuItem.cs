using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace atm.services.models
{
    public class MenuItem
    {
        public MenuItem() { }
        public int SectionId { get; set; }
        public int FunctionId { get; set; }
        public string FunctionDescription { get; set; }
        public string RootUrl { get; set; }
        public int DisplayOrder { get; set; }
    }
}
