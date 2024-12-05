using Microsoft.AspNetCore.Mvc;
using OnePointBooking.Application.Common.Utility;
using OnePointBooking.Application.Services.Interface;
using OnePointBooking.Domain.Entities;
using OnePointBooking.Web.Models;
using OnePointBooking.Web.ViewModels;
using System.Diagnostics;

namespace OnePointBooking.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRoomService _roomService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(IRoomService roomService, IWebHostEnvironment webHostEnvironment)
        {
            _roomService = roomService;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var userEmail = User.Identity.Name;
            bool isKalventisUser = !string.IsNullOrEmpty(userEmail) && userEmail.EndsWith("@kalventis.com");
            bool isAdmin = User.IsInRole(SD.Role_Admin);

            var roomList = _roomService.GetAllRooms();

            // If the user is not an admin and not a Kalventis user, filter out Kalventis RoomPackages
            if (!isAdmin && !isKalventisUser)
            {
                foreach (var room in roomList)
                {
                    // Filter out RoomPackages with IsKalventis = true for non-Kalventis and non-admin users
                    room.RoomPackage = room.RoomPackage.Where(rp => rp.IsKalventis != true).ToList();
                }
            }

            HomeVM homeVM = new()
            {
                RoomList = roomList,
                Days = 1,
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                IsKalventis = isKalventisUser,
                IsAdmin = isAdmin  // Pass the admin flag to the view model
            };

            return View(homeVM);
        }

        [HttpPost]
        public IActionResult GetRoomsByDate(int days, DateOnly startDate)
        {
            var userEmail = User.Identity.Name;
            bool isKalventisUser = !string.IsNullOrEmpty(userEmail) && userEmail.EndsWith("@kalventis.com");
            bool isAdmin = User.IsInRole(SD.Role_Admin);

            var roomList = _roomService.GetRoomsAvailabilityByDate(days, startDate);

            // Filter RoomPackages jika pengguna bukan admin atau Kalventis
            if (!isAdmin && !isKalventisUser)
            {
                foreach (var room in roomList)
                {
                    room.RoomPackage = room.RoomPackage.Where(rp => rp.IsKalventis != true).ToList();
                }
            }

            HomeVM homeVM = new()
            {
                StartDate = startDate,
                RoomList = roomList,
                Days = days,
                IsKalventis = isKalventisUser
            };

            return PartialView("_RoomList", homeVM);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
