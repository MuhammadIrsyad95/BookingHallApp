using Microsoft.AspNetCore.Hosting;
using OnePointBooking.Application.Common.Interfaces;
using OnePointBooking.Application.Services.Interface;
using OnePointBooking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePointBooking.Application.Services.Implementation
{
    public class RoomSetupService : IRoomSetupService
    {
        private readonly IUnitOfWork _unitOfWork;
        public RoomSetupService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public void CreateRoomSetup(RoomSetup roomSetup)
        {
            ArgumentNullException.ThrowIfNull(roomSetup);

            _unitOfWork.RoomSetup.Add(roomSetup);
            _unitOfWork.Save();
        }

        public bool DeleteRoomSetup(int id)
        {
            try
            {
                var roomSetup = _unitOfWork.RoomSetup.Get(u => u.RoomSetupId == id);

                if (roomSetup != null)
                {

                    _unitOfWork.RoomSetup.Remove(roomSetup);
                    _unitOfWork.Save();
                    return true;
                }
                else
                {
                    throw new InvalidOperationException($"RoomSetup with ID {id} not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }

       

        public IEnumerable<RoomSetup> GetAllRoomSetups()
        {
            return _unitOfWork.RoomSetup.GetAll(includeProperties: "Room");
        }

        public RoomSetup GetRoomSetupById(int id)
        {
            return _unitOfWork.RoomSetup.Get(u => u.RoomSetupId == id, includeProperties: "Room");
        }

        public void UpdateRoomSetup(RoomSetup roomSetup)
        {
            ArgumentNullException.ThrowIfNull(roomSetup);
            _unitOfWork.RoomSetup.Update(roomSetup);
            _unitOfWork.Save();
        }
        public IEnumerable<RoomSetup> GetRoomSetupsByRoomId(int roomId)
        {
            return _unitOfWork.RoomSetup.GetRoomSetupsByRoomId(roomId);
        }
    }
}
