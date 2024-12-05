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
    public class ServiceService : IServiceService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ServiceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public void CreateService(Service service)
        {
            ArgumentNullException.ThrowIfNull(service);

            _unitOfWork.Service.Add(service);
            _unitOfWork.Save();
        }

        public bool DeleteService(int id)
        {
            try
            {
                var service = _unitOfWork.Service.Get(u => u.RoomServiceId == id);

                if (service != null)
                {

                    _unitOfWork.Service.Remove(service);
                    _unitOfWork.Save();
                    return true;
                }
                else
                {
                    throw new InvalidOperationException($"Service with ID {id} not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }

       

        public IEnumerable<Service> GetAllServices()
        {
            return _unitOfWork.Service.GetAll(includeProperties: "Room");
        }

        public Service GetServiceById(int id)
        {
            return _unitOfWork.Service.Get(u => u.RoomServiceId == id, includeProperties: "Room");
        }

        public void UpdateService(Service service)
        {
            ArgumentNullException.ThrowIfNull(service);
            _unitOfWork.Service.Update(service);
            _unitOfWork.Save();
        }
        public IEnumerable<Service> GetServicesByRoomId(int roomId)
        {
            return _unitOfWork.Service.GetServicesByRoomId(roomId);
        }
    }
}
