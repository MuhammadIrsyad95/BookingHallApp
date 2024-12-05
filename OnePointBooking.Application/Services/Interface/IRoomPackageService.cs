using OnePointBooking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePointBooking.Application.Services.Interface
{
    public interface IRoomPackageService
    {
        IEnumerable<RoomPackage> GetAllRoomPackages();
        RoomPackage GetRoomPackageById(int id);
        void CreateRoomPackage(RoomPackage roomPackage);
        void UpdateRoomPackage(RoomPackage roomPackage);
        bool DeleteRoomPackage(int id);

        bool CheckRoomPackageExists(int roomPackageId);

        // New method
        IEnumerable<RoomPackage> GetRoomPackagesByRoomId(int roomId);
    }
}
