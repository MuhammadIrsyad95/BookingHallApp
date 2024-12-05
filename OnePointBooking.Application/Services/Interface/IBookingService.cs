using OnePointBooking.Domain.Entities;
using System.Collections.Generic;

namespace OnePointBooking.Application.Services.Interface
{
    public interface IBookingService
    {
        void CreateBooking(Booking booking);
        Booking GetBookingById(int bookingId);
        IEnumerable<Booking> GetAllBookings(string userId = "", string? statusFilterList = "", string? search = "");
        void UpdateStatus(int bookingId, string modifiedBy, string bookingStatus, int roomPackageId , int roomSetupId );
        IEnumerable<int> GetCheckedInRoomPackages(int roomId);

        IEnumerable<RoomPackage> GetAvailableRoomPackages(int roomId, bool isKalventisUser);
        IEnumerable<RoomSetup> GetRoomSetupsByRoomId(int roomId);

        // Tambahkan metode ini
        RoomPackage GetRoomPackageById(int roomPackageId);

        // Add this method for Room Setup
        RoomSetup GetRoomSetupById(int roomSetupId);
        IEnumerable<Amenity> GetRoomAmenitiesByPackageId(int roomPackageId);

    }
}
