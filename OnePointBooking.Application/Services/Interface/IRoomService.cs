using OnePointBooking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePointBooking.Application.Services.Interface
{
    public interface IRoomService
    {
        IEnumerable<Room> GetAllRooms();
        Room GetRoomById(int id);
        void CreateRoom(Room room);
        void UpdateRoom(Room room);
        bool DeleteRoom(int id);

        IEnumerable<Room> GetRoomsAvailabilityByDate(int days, DateOnly startDate);
        bool IsRoomAvailableByDate(int roomId, int days, DateOnly checkInDate);

        // Tambahkan metode untuk mendapatkan Room Packages dan Room Setups
        IEnumerable<RoomPackage> GetAvailableRoomPackages(int roomId);
        IEnumerable<RoomSetup> GetRoomSetupsByRoomId(int roomId);

    }
}
