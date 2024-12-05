using OnePointBooking.Application.Common.Interfaces;
using OnePointBooking.Application.Common.Utility;
using OnePointBooking.Application.Services.Interface;
using OnePointBooking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OnePointBooking.Application.Services.Implementation
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void CreateBooking(Booking booking)
        {
            booking.Status = SD.StatusPending; // Approve booking otomatis
            _unitOfWork.Booking.Add(booking);
            _unitOfWork.Save();
        }

        public IEnumerable<Booking> GetAllBookings(string userId = "", string? statusFilterList = "", string? search = "")
        {
            // Create a list of statuses from the provided string
            var statusList = !string.IsNullOrEmpty(statusFilterList) ? statusFilterList.ToLower().Split(",") : Array.Empty<string>();

            // If userId, statusFilterList, and search are provided, filter bookings accordingly
            return _unitOfWork.Booking.GetAll(u =>
                (string.IsNullOrEmpty(search) || u.User.Name.ToLower().Contains(search.ToLower())) && // Search by User Name
                (string.IsNullOrEmpty(userId) || u.UserId == userId) &&
                (statusList.Length == 0 || statusList.Contains(u.Status.ToLower())),
                includeProperties: "User,Room,RoomPackage.RoomAmenity,RoomSetup");
        }


        public Booking GetBookingById(int bookingId)
        {
            return _unitOfWork.Booking.Get(
                u => u.Id == bookingId,
                includeProperties: "User,Room,RoomPackage.RoomAmenity,RoomSetup");
        }

        public void UpdateStatus(int bookingId, string modifiedBy, string bookingStatus, int roomPackageId = 0, int roomSetupId = 0)
        {

            var bookingFromDb = _unitOfWork.Booking.Get(m => m.Id == bookingId, tracked: true);
            if (bookingFromDb != null)
            {
                bookingFromDb.Status = bookingStatus;
                bookingFromDb.RoomPackageId = roomPackageId;
                bookingFromDb.ModifiedBy = modifiedBy;  // Simpan nilai modifiedBy
                bookingFromDb.ModifiedDate = DateTime.Now;
                // Set ApprovedDate if the status is Approved
                if (bookingStatus == SD.StatusApproved)
                {
                    bookingFromDb.ApprovedDate = DateTime.Now;
                }
            }
            _unitOfWork.Save();
        }

        public IEnumerable<int> GetCheckedInRoomPackages(int roomId)
        {
            return _unitOfWork.Booking.GetAll(
                u => u.RoomId == roomId && u.Status == SD.StatusApproved)
                .Select(u => u.RoomPackageId);
        }
        public IEnumerable<RoomPackage> GetAvailableRoomPackages(int roomId, bool isKalventisUser)
        {
            var query = _unitOfWork.RoomPackage.GetAll(
                u => u.RoomId == roomId && u.Status == SD.StatusActive,
                includeProperties: "RoomAmenity"); // Menambahkan RoomAmenity untuk perhitungan harga total

            if (!isKalventisUser)
            {
                query = query.Where(u => u.IsKalventis != true);
            }

            return query.ToList();
        }


        public IEnumerable<RoomSetup> GetRoomSetupsByRoomId(int roomId)
        {
            // Logika untuk mendapatkan paket yang tersedia berdasarkan roomId
            return _unitOfWork.RoomSetup.GetAll(u => u.RoomId == roomId);
        }

        public RoomPackage GetRoomPackageById(int roomPackageId)
        {
            return _unitOfWork.RoomPackage.Get(u => u.RoomPackageId == roomPackageId);
        }

        public RoomSetup GetRoomSetupById(int roomSetupId)
        {
            return _unitOfWork.RoomSetup.Get(u => u.RoomSetupId == roomSetupId);
        }
        public IEnumerable<Amenity> GetRoomAmenitiesByPackageId(int roomPackageId)
        {
            // Retrieve amenities associated with the specified RoomPackageId
            return _unitOfWork.Amenity.GetAll(a => a.RoomPackageId == roomPackageId);
        }
    }
}
