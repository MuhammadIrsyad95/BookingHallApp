using Microsoft.AspNetCore.Hosting;
using OnePointBooking.Application.Common.Interfaces;
using OnePointBooking.Application.Services.Interface;
using OnePointBooking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OnePointBooking.Application.Services.Implementation
{
    public class RoomService : IRoomService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public RoomService(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        // Create Room with image
        public void CreateRoom(Room room)
        {
            if (room.Image != null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(room.Image.FileName);
                string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "RoomImage");

                // Ensure the directory exists
                Directory.CreateDirectory(imagePath);

                using var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create);
                room.Image.CopyTo(fileStream);

                room.ImageUrl = "/images/RoomImage/" + fileName; // Use / as the separator for URL
            }
            else
            {
                room.ImageUrl = "https://placehold.co/600x400"; // Default image if no file is uploaded
            }

            _unitOfWork.Room.Add(room);
            _unitOfWork.Save();
        }

        // Delete Room with image removal
        public bool DeleteRoom(int id)
        {
            try
            {
                Room? objFromDb = _unitOfWork.Room.Get(u => u.Id == id);
                if (objFromDb != null)
                {
                    if (!string.IsNullOrWhiteSpace(objFromDb.ImageUrl))
                    {
                        // Ensure the image file is removed from the server
                        var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, objFromDb.ImageUrl.TrimStart('/'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    _unitOfWork.Room.Remove(objFromDb);
                    _unitOfWork.Save();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Update Room with image change handling
        public void UpdateRoom(Room room)
        {
            if (room.Image != null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(room.Image.FileName);
                string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "RoomImage");

                // Ensure the directory exists
                Directory.CreateDirectory(imagePath);

                // If there is an old image, delete it
                if (!string.IsNullOrEmpty(room.ImageUrl))
                {
                    var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, room.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                // Save new image
                string filePath = Path.Combine(imagePath, fileName);
                using var fileStream = new FileStream(filePath, FileMode.Create);
                room.Image.CopyTo(fileStream);

                room.ImageUrl = "/images/RoomImage/" + fileName; // Use / as the separator for URL
            }

            _unitOfWork.Room.Update(room);
            _unitOfWork.Save();
        }

        // Get all Rooms
        public IEnumerable<Room> GetAllRooms()
        {
            return _unitOfWork.Room.GetAll(includeProperties: "RoomPackage.RoomAmenity,RoomSetup");
        }

        // Get Room by Id
        public Room GetRoomById(int id)
        {
            return _unitOfWork.Room.Get(u => u.Id == id, includeProperties: "RoomPackage.RoomAmenity,RoomSetup");
        }

        // Get Rooms Availability based on date and days
        public IEnumerable<Room> GetRoomsAvailabilityByDate(int days, DateOnly startDate)
        {
            var rooms = _unitOfWork.Room.GetAll(includeProperties: "RoomPackage.RoomAmenity,RoomSetup").ToList();
            var bookings = _unitOfWork.Booking.GetAll(u => u.Status == "Approved").ToList();

            foreach (var room in rooms)
            {
                bool isAvailable = CheckRoomAvailability(room.Id, days, startDate, bookings);
                room.isAvailable = isAvailable; // Set availability status
            }

            return rooms;
        }

        // Check availability of specific room for given dates
        public bool IsRoomAvailableByDate(int roomId, int days, DateOnly startDate)
        {
            var bookings = _unitOfWork.Booking.GetAll(u => u.Status == "Approved" && u.RoomId == roomId).ToList();
            return CheckRoomAvailability(roomId, days, startDate, bookings);
        }

        // Check if the room is available based on booking overlaps
        private bool CheckRoomAvailability(int roomId, int days, DateOnly startDate, List<Booking> bookings)
        {
            foreach (var booking in bookings)
            {
                if (booking.RoomId == roomId)
                {
                    var bookingEndDate = booking.StartDate.AddDays(booking.Days);
                    var requestedEndDate = startDate.AddDays(days);

                    if ((startDate < bookingEndDate) && (booking.StartDate < requestedEndDate))
                    {
                        return false; // Room is not available due to overlap
                    }
                }
            }
            return true; // Room is available
        }

        // Get available room packages by Room Id
        public IEnumerable<RoomPackage> GetAvailableRoomPackages(int roomId)
        {
            return _unitOfWork.RoomPackage.GetAll(u => u.RoomId == roomId && u.Status == "Active");
        }

        // Get room setups by Room Id
        public IEnumerable<RoomSetup> GetRoomSetupsByRoomId(int roomId)
        {
            return _unitOfWork.RoomSetup.GetAll(u => u.RoomId == roomId);
        }
    }
}
