using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using atm.services.models;

namespace atm.services
{
	public interface INotificationService
    {
        Task SendAsync(List<Notification> notifications);
    }
}