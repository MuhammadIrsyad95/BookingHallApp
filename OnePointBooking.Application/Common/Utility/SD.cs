using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnePointBooking.Domain.Entities;


namespace OnePointBooking.Application.Common.Utility
{
    public static class SD
    {
        public const string Role_Customer = "Customer";
        public const string Role_Admin = "Admin";

        public const string StatusPending = "Pending";
        public const string StatusApproved = "Approved";
        public const string StatusCompleted = "Completed";
        public const string StatusCancelled = "Cancelled";

        public const string StatusActive = "Active";
        public const string StatusInactive = "Inactive";

        public static int RoomsAvailable_Count(int roomId,
               List<Room> rooms, DateOnly startDate, int days,
               List<Booking> bookings)
              {
                List<int> bookingInDate = new();
                int finalAvailableRoomForAllDays = int.MaxValue;
                var roomsInRoom = rooms.Count(x => x.Id == roomId); // Count rooms based on roomId

                for (int i = 0; i < days; i++)
                {
                    var currentDate = startDate.AddDays(i);
                    var roomsBooked = bookings.Where(u =>
                        u.StartDate <= currentDate &&
                        u.EndDate > currentDate &&
                        u.RoomId == roomId);

                    foreach (var booking in roomsBooked)
                    {
                        if (!bookingInDate.Contains(booking.Id))
                        {
                            bookingInDate.Add(booking.Id);
                        }
                    }

                    var totalAvailableRooms = roomsInRoom - bookingInDate.Count;
                    if (totalAvailableRooms == 0)
                    {
                        return 0;
                    }
                    else
                    {
                        if (finalAvailableRoomForAllDays > totalAvailableRooms)
                        {
                            finalAvailableRoomForAllDays = totalAvailableRooms;
                        }
                    }
                }
                return finalAvailableRoomForAllDays;
             }

    }
}
