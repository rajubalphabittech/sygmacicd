using System.Collections.Generic;
using System.Data;

namespace atm.services.models
{
    public class MenuSection
    {
        public MenuSection()
        {
            MenuItems = new List<MenuItem>();
        }

        public int SectionId { get; set; }
        public string SectionDescription { get; set; }
        public int DisplayOrder { get; set; }

        public List<MenuItem> MenuItems { get; set; }
    }
}