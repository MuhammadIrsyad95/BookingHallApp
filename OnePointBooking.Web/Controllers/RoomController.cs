using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnePointBooking.Application.Services.Implementation;
using OnePointBooking.Application.Services.Interface;
using OnePointBooking.Domain.Entities;

namespace OnePointBooking.Web.Controllers
{
    [Authorize]
    public class RoomController : Controller
    {
        private readonly IRoomService _roomService;
        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }
        public IActionResult Index()
        {
            var rooms = _roomService.GetAllRooms();
            return View(rooms);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Room obj)
        {

            if (obj.Name == obj.Description)
            {
                ModelState.AddModelError("name", "The Description cannot exactly match the Name.");
            }
            if (ModelState.IsValid)
            {
                // Set CreatedBy to the current user's email or username
                obj.CreatedBy = User.Identity.Name; // Ensure the user is authenticated
              // Set CreatedDate to the current date and time
                obj.CreatedDate = DateTime.Now; // Set CreatedDate to now
                 // Leave UpdatedDate and UpdatedBy as null
                obj.UpdatedDate = null; // Explicitly set to null for creation
                obj.UpdatedBy = null;    // Explicitly set to null for creation

                //Upload Image
                _roomService.CreateRoom(obj);
                TempData["success"] = "The Room has been created successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        public IActionResult Update(int roomId)
        {
            Room? obj = _roomService.GetRoomById(roomId);

            if (obj is null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(obj);
        }

        [HttpPost]
        public IActionResult Update(Room obj)
        {
            if (ModelState.IsValid && obj.Id > 0)
            {
                // Update the fields
                var roomFromDb = _roomService.GetRoomById(obj.Id);

                // Pastikan bahwa CreatedBy dan CreatedDate tetap dari data sebelumnya
                obj.CreatedBy = roomFromDb.CreatedBy;
                obj.CreatedDate = roomFromDb.CreatedDate;

                obj.UpdatedBy = User.Identity.Name; // Set to the current user's email or username
                obj.UpdatedDate = DateTime.Now; // Set the current date and time

                _roomService.UpdateRoom(obj);
                TempData["success"] = "The Room has been Updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        public IActionResult Delete(int roomId)
        {
            Room? obj = _roomService.GetRoomById(roomId);

            if (obj == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(obj);
        }
        [HttpPost]
        public IActionResult Delete(Room obj)
        {
            bool deleted = _roomService.DeleteRoom(obj.Id);
            if (deleted)
            {
                TempData["success"] = "The Room has been Deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = "Failed to delete the room.";
            }
            return View();
        }

        
    }

    
}
