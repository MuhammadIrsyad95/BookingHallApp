using OnePointBooking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePointBooking.Application.Common.Interfaces
{
    public interface IRoomPackageRepository : IRepository<RoomPackage>
    {
        void Update(RoomPackage entity);
        IEnumerable<RoomPackage> GetRoomPackagesByRoomId(int roomId);

    }
}
