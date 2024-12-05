using OnePointBooking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePointBooking.Application.Services.Interface
{
    public interface IRoomSetupService
    {
        IEnumerable<RoomSetup> GetAllRoomSetups();
        RoomSetup GetRoomSetupById(int id);
        void CreateRoomSetup(RoomSetup roomSetup);
        void UpdateRoomSetup(RoomSetup roomSetup);
        bool DeleteRoomSetup(int id);

        IEnumerable<RoomSetup> GetRoomSetupsByRoomId(int roomId);

    }
}
