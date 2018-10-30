using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using SygmaFramework;

namespace atm.services
{
    public class AuthorizationService : IAuthorizationService
    {
        public bool IsValiduser(string userName, string route)
        {
            var key = $"{userName}.{route}";
            if (CacheHandler.Get(key, out bool cachedObject))
            {
                return cachedObject;
            }

            using (var Db = new AtmContext())
            {
                var result = Db.Database
                       .SqlQuery<bool>("exec [up_sec_isValidUser] @userName = {0}, @rootUrl = {1}",
                           userName,
                           route
                       ).ToList().FirstOrDefault();
                CacheHandler.Add(result, key);
                return result;
            }
        }

        public bool IsApprover(string userName)
        {
            using (var Db = new AtmContext())
            {
                var result = Db.Database
                    .SqlQuery<int>("exec [up_user_isApprover] @userName = {0}",
                        userName
                    ).ToList().FirstOrDefault();

                return result > 0; // false if 0
            }
        }

        public AuthorizationService()
        {
        }
    }
}