using OnePointBooking.Application.Common.Interfaces;
using OnePointBooking.Application.Services.Interface;
using OnePointBooking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OnePointBooking.Application.Services.Implementation
{
    public class RoomPackageService : IRoomPackageService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoomPackageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public bool CheckRoomPackageExists(int roomPackageId)
        {
            return _unitOfWork.RoomPackage.Any(u => u.RoomPackageId == roomPackageId);
        }

        public void CreateRoomPackage(RoomPackage roomPackage)
        {
            // Set the IsKalventis flag if it's provided
            roomPackage.IsKalventis = roomPackage.IsKalventis;

            _unitOfWork.RoomPackage.Add(roomPackage);
            _unitOfWork.Save();
        }

        public bool DeleteRoomPackage(int id)
        {
            try
            {
                RoomPackage? objFromDb = _unitOfWork.RoomPackage.Get(u => u.RoomPackageId == id);
                if (objFromDb is not null)
                {
                    _unitOfWork.RoomPackage.Remove(objFromDb);
                    _unitOfWork.Save();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<RoomPackage> GetAllRoomPackages()
        {
            return _unitOfWork.RoomPackage.GetAll(includeProperties: "Room, RoomAmenity");
        }

        public RoomPackage GetRoomPackageById(int id)
        {
            // Ensure the IsKalventis flag is included in the retrieved RoomPackage
            return _unitOfWork.RoomPackage.Get(u => u.RoomPackageId == id, includeProperties: "Room, RoomAmenity");
        }

        public void UpdateRoomPackage(RoomPackage roomPackage)
        {
            // Set or update the IsKalventis flag during update
            var existingRoomPackage = _unitOfWork.RoomPackage.Get(u => u.RoomPackageId == roomPackage.RoomPackageId);
            if (existingRoomPackage != null)
            {
                // Retain the IsKalventis flag during update if needed
                roomPackage.IsKalventis = roomPackage.IsKalventis;
                _unitOfWork.RoomPackage.Update(roomPackage);
                _unitOfWork.Save();
            }
        }

        public IEnumerable<RoomPackage> GetRoomPackagesByRoomId(int roomId)
        {
            return _unitOfWork.RoomPackage.GetRoomPackagesByRoomId(roomId);
        }
    }
}
