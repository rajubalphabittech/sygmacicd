using System.Collections.Generic;

namespace atm.services.models
{
	public class Menu
		{
				public string Title { get; set; }
				public List<MenuSection> Sections { get; set; }

				public Menu()
				{
						Sections = new List<MenuSection>();
				}
		}
}