using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using atm.services;
using atm.services.models;
using Microsoft.AspNet.SignalR;

namespace atm.helpers
{
    public class RouteHub : Hub
    {
        public void UpdateMoveStatus(int routePlanId, MoveStatuses status)
        {
            Clients.All.updateMoveStatus(routePlanId, status);
        }
    }
}