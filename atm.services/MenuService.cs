using System;
using System.Data;
using atm.services.models;
using SygmaFramework;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;

namespace atm.services
{
    public class MenuService : IMenuService
    {
        public string GetFirstPageUrlForUserName(string userName)
        {
            var result = new Menu { Title = "ATM" };
            result = GetByUserName(userName);
            //If current user doesn't have access to any ATM pages
            if (result.Sections.Count == 0)
            {
                //TODO: What page should we return the user to?
                return "";
            }
            else
            {
                var destinationUrl = result.Sections.First().MenuItems.First().RootUrl;
                return destinationUrl;
            }

        }

        public Menu GetByUserName(string userName)
        {
            var key = $"{userName}.menu";
            if (CacheHandler.Get(key, out Menu cachedObject))
            {
                return cachedObject;
            }
            else
            {
                var result = new Menu { Title = "ATM" };

                using (var Db = new AtmContext())
                {
                    // Create a SQL command to execute the sproc
                    var cmd = Db.Database.Connection.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = $"[dbo].[up_user_getMenuFunctions]";
                    cmd.Parameters.Add(new SqlParameter("@userName", userName));

                    try
                    {
                        // Run the sproc
                        Db.Database.Connection.Open();
                        var reader = cmd.ExecuteReader();

                        // Read Blogs from the first result set
                        result.Sections = ((IObjectContextAdapter)Db)
                            .ObjectContext
                            .Translate<MenuSection>(reader).ToList();

                        // Move to second result set 
                        reader.NextResult();
                        var items = ((IObjectContextAdapter)Db)
                            .ObjectContext
                            .Translate<MenuItem>(reader).ToList();

                        foreach (var section in result.Sections)
                        {
                            section.MenuItems.AddRange(items.Where(i => i.SectionId == section.SectionId));
                        }
                    }
                    finally
                    {
                        Db.Database.Connection.Close();
                    }

                    CacheHandler.Add(result, key);
                    return result;
                }
            }
        }

        public MenuService()
        {
        }
    }
}