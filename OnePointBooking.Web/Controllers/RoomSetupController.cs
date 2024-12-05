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
    public class RoomSetupController : Controller
    {
        private readonly IRoomSetupService _roomSetupService;
        private readonly IRoomService _roomService;

        public RoomSetupController(IRoomSetupService roomSetupService, IRoomService roomService)
        {
            _roomSetupService = roomSetupService;
            _roomService = roomService;
        }
        public IActionResult Index()
        {
            var roomSetups = _roomSetupService.GetAllRoomSetups();
            return View(roomSetups);
        }
        public IActionResult Create()
        {
            RoomSetupVM roomSetupVM = new()
            {
                RoomList = _roomService.GetAllRooms().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),

            };
            return View(roomSetupVM);
        }
        [HttpPost]
        public IActionResult Create(RoomSetupVM obj)
        {

            if (ModelState.IsValid)
            {
                // Set CreatedBy to the current user's email or username
                obj.RoomSetup.CreatedBy = User.Identity.Name; // Ensure the user is authenticated
                                                    // Set CreatedDate to the current date and time
                obj.RoomSetup.CreatedDate = DateTime.Now; // Set CreatedDate to now

                _roomSetupService.CreateRoomSetup(obj.RoomSetup);
                TempData["success"] = "The Room Setup has been created successfully.";
                return RedirectToAction(nameof(Index));
            }
            
            obj.RoomList = _roomService.GetAllRooms().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            return View(obj);
        }
        public IActionResult Update(int roomSetupId)
        {
            RoomSetupVM roomSetupVM = new()
            {
                RoomList = _roomService.GetAllRooms().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                RoomSetup = _roomSetupService.GetRoomSetupById(roomSetupId)
            };
            if (roomSetupVM.RoomSetup == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(roomSetupVM);
        }
        [HttpPost]
        public IActionResult Update(RoomSetupVM roomSetupVM)
        {

            if (ModelState.IsValid)
            {
                // Ambil RoomSetup dari DB untuk mendapatkan CreatedBy dan CreatedDate yang lama
                var roomSetupFromDb = _roomSetupService.GetRoomSetupById(roomSetupVM.RoomSetup.RoomSetupId);

                // Pastikan nilai CreatedBy dan CreatedDate tetap ada
                roomSetupVM.RoomSetup.CreatedBy = roomSetupFromDb.CreatedBy;
                roomSetupVM.RoomSetup.CreatedDate = roomSetupFromDb.CreatedDate;

                roomSetupVM.RoomSetup.UpdatedBy = User.Identity.Name; // Ensure the user is authenticated
                roomSetupVM.RoomSetup.UpdatedDate = DateTime.Now; // Ensure the user is authenticated

                _roomSetupService.UpdateRoomSetup(roomSetupVM.RoomSetup);
                TempData["success"] = "The Room Setup has been updated successfully.";
                return RedirectToAction("Index");
            }
            roomSetupVM.RoomList = _roomService.GetAllRooms().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            return View(roomSetupVM);
        }
        public IActionResult Delete(int roomSetupId)
        {
            RoomSetupVM roomSetupVM = new()
            {
                RoomList = _roomService.GetAllRooms().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                RoomSetup = _roomSetupService.GetRoomSetupById(roomSetupId)
            };
            if (roomSetupVM.RoomSetup == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(roomSetupVM);
        }
        [HttpPost]
        public IActionResult Delete(RoomSetupVM roomSetupVM)
        {

            RoomSetup? objFromDb = _roomSetupService.GetRoomSetupById(roomSetupVM.RoomSetup.RoomSetupId);
            if (objFromDb is not null)
            {
                _roomSetupService.DeleteRoomSetup(objFromDb.RoomSetupId);
                TempData["success"] = "The Room Setup has been deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "The Room PaSSckage could not be deleted .";

            return View();
        }

    }
}
