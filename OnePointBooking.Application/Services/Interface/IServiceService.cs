using OnePointBooking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePointBooking.Application.Services.Interface
{
    public interface IServiceService
    {
        IEnumerable<Service> GetAllServices();
        Service GetServiceById(int id);
        void CreateService(Service service);
        void UpdateService(Service service);
        bool DeleteService(int id);

        IEnumerable<Service> GetServicesByRoomId(int roomId);

    }
}
