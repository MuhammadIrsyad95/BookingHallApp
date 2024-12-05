using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnePointBooking.Application.Common.Utility;
using OnePointBooking.Application.Services.Implementation;
using OnePointBooking.Application.Services.Interface;
using OnePointBooking.Domain.Entities;
using OnePointBooking.Web.ViewModels;

namespace OnePointBooking.Web.Controllers
{
    [Authorize(Roles = SD.Role_Admin)]
    public class ServiceController : Controller
    {
        private readonly IServiceService _serviceService;
        private readonly IRoomService _roomService;

        public ServiceController(IServiceService serviceService, IRoomService roomService)
        {
            _serviceService = serviceService;
            _roomService = roomService;
        }
        public IActionResult Index()
        {
            var service = _serviceService.GetAllServices();
            return View(service);
        }
        public IActionResult Create()
        {
            ServiceVM serviceVM = new()
            {
                RoomList = _roomService.GetAllRooms().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),

            };
            return View(serviceVM);
        }
        [HttpPost]
        public IActionResult Create(ServiceVM obj)
        {

            if (ModelState.IsValid)
            {
                // Set CreatedBy to the current user's email or username
                obj.Service.CreatedBy = User.Identity.Name; // Ensure the user is authenticated
                                                    // Set CreatedDate to the current date and time
                obj.Service.CreatedDate = DateTime.Now; // Set CreatedDate to now

                _serviceService.CreateService(obj.Service);
                TempData["success"] = "The Service has been created successfully.";
                return RedirectToAction(nameof(Index));
            }
            
            obj.RoomList = _roomService.GetAllRooms().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            return View(obj);
        }
        public IActionResult Update(int roomServiceId)
        {
            ServiceVM serviceVM = new()
            {
                RoomList = _roomService.GetAllRooms().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Service = _serviceService.GetServiceById(roomServiceId)
            };
            if (serviceVM.Service == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(serviceVM);
        }
        [HttpPost]
        public IActionResult Update(ServiceVM serviceVM)
        {

            if (ModelState.IsValid)
            {
                serviceVM.Service.UpdatedBy = User.Identity.Name; // Ensure the user is authenticated
                serviceVM.Service.UpdatedDate = DateTime.Now; // Ensure the user is authenticated

                _serviceService.UpdateService(serviceVM.Service);
                TempData["success"] = "The Service has been updated successfully.";
                return RedirectToAction("Index");
            }
            serviceVM.RoomList = _roomService.GetAllRooms().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            return View(serviceVM);
        }
        public IActionResult Delete(int roomServiceId)
        {
            ServiceVM serviceVM = new()
            {
                RoomList = _roomService.GetAllRooms().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Service = _serviceService.GetServiceById(roomServiceId)
            };
            if (serviceVM.Service == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(serviceVM);
        }
        [HttpPost]
        public IActionResult Delete(ServiceVM serviceVM)
        {

            Service? objFromDb = _serviceService.GetServiceById(serviceVM.Service.RoomServiceId);
            if (objFromDb is not null)
            {
                _serviceService.DeleteService(objFromDb.RoomServiceId);
                TempData["success"] = "The Service has been deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "The Service could not be deleted .";

            return View();
        }

    }
}
