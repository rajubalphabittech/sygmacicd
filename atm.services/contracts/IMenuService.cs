using System.Collections.Generic;
using System.Threading.Tasks;
using atm.services.models;

namespace atm.services
{
    public interface IMenuService
    {
        Menu GetByUserName(string userName);
        string GetFirstPageUrlForUserName(string userName);
    }
}