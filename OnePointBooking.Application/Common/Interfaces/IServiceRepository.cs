using OnePointBooking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePointBooking.Application.Common.Interfaces
{
    public interface IServiceRepository : IRepository<Service>
    {
        void Update(Service entity);
        IEnumerable<Service> GetServicesByRoomId(int roomId); // Add this method

    }
}
