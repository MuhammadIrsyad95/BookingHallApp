using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Security.Claims;
using OnePointBooking.Application.Common.Utility;
using OnePointBooking.Application.Services.Interface;
using OnePointBooking.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace OnePointBooking.Web.Controllers
{
    public class BookingController : Controller
    {
        private readonly IBookingService _bookingService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IRoomService _roomService;
        private readonly ICompanyService _companyService;
        private readonly UserManager<ApplicationUser> _userManager;

        public BookingController(IBookingService bookingService,
                                 IWebHostEnvironment webHostEnvironment,
                                 IRoomService roomService,
                                 UserManager<ApplicationUser> userManager,
                                 ICompanyService companyService)
        {
            _bookingService = bookingService;
            _webHostEnvironment = webHostEnvironment;
            _roomService = roomService;
            _userManager = userManager;
            _companyService = companyService;
        }

        [Authorize]
        public IActionResult Index(string userId = "", string statusFilterList = "", string search = "", string sortOrder = "")
        {
            ViewBag.UserId = userId;
            ViewBag.Status = statusFilterList;
            ViewBag.SortOrder = sortOrder;

            var isAdmin = User.IsInRole(SD.Role_Admin);
            IEnumerable<Booking> bookings;

            if (isAdmin)
            {
                bookings = _bookingService.GetAllBookings(userId, statusFilterList, search);
            }
            else
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var currentUserId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                bookings = _bookingService.GetAllBookings(currentUserId, statusFilterList, search);
            }

            // Sort the bookings based on the sortOrder
            bookings = sortOrder switch
            {
                "StartDate_asc" => bookings.OrderBy(b => b.StartDate),
                "StartDate_desc" => bookings.OrderByDescending(b => b.StartDate),
                "Name_asc" => bookings.OrderBy(b => b.User.Name),
                "Name_desc" => bookings.OrderByDescending(b => b.User.Name),
                _ => bookings
            };
            return View(bookings);
        }

        [Authorize]
        public IActionResult FinalizeBooking(int roomId, DateOnly startDate, int days)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            ApplicationUser user = _userManager.FindByIdAsync(userId).GetAwaiter().GetResult();
            var room = _roomService.GetRoomById(roomId);

            bool isKalventisUser = !string.IsNullOrEmpty(user.Email) && user.Email.EndsWith("@kalventis.com");

            var roomPackages = _bookingService.GetAvailableRoomPackages(roomId, isKalventisUser);
            roomPackages = roomPackages.Where(rp => isKalventisUser || rp.IsKalventis != true).ToList();

            var roomSetups = _bookingService.GetRoomSetupsByRoomId(roomId);
            var companies = _companyService.GetAllCompanies();

            
            var companyList = companies.Select(c => new SelectListItem
            {
                Value = c.CompanyName,
                Text = c.CompanyName
            }).ToList();


            Booking booking = new()
            {
                RoomId = roomId,
                Room = room,
                StartDate = startDate,
                Days = days,
                EndDate = startDate.AddDays(days - 1),
                UserId = userId,
                Phone = user.PhoneNumber,
                Email = user.Email,
                Name = user.Name,
                Company = "",
                RoomPackage = roomPackages.FirstOrDefault() // Pilih paket pertama secara default
            };

            booking.Room.RoomPackage = roomPackages;


            ViewBag.RoomPackages = roomPackages;
            ViewBag.RoomSetups = roomSetups;
            ViewBag.Companies = companyList;

            return View(booking);
        }

        [HttpPost]
        [Authorize]
        public IActionResult FinalizeBooking(Booking booking, int roomPackageId, int roomSetupId, decimal totalCost)
        {

            booking.Company = booking.Company;  

            booking.TotalCost = totalCost;
            booking.RoomPackageId = roomPackageId;
            booking.RoomSetupId = roomSetupId;
            booking.EventName = booking.EventName;
            booking.TotalParticipant = booking.TotalParticipant;
            booking.BookingDate = DateTime.Now;

       

            _bookingService.CreateBooking(booking);

            return RedirectToAction(nameof(BookingConfirmation), new { bookingId = booking.Id });
        }


        private string GetCompanyNameForUser(int companyId)
        {
            if (companyId <= 0)
            {
                return "Default Company";
            }

            var company = _companyService.GetCompanyById(companyId);
            return company != null ? company.CompanyName : "Default Company";
        }

        [Authorize]
        public IActionResult BookingConfirmation(int bookingId)
        {
            Booking bookingFromDb = _bookingService.GetBookingById(bookingId);
            return View(bookingId);
        }

        [Authorize]
        public IActionResult BookingDetails(int bookingId)
        {
            Booking bookingFromDb = _bookingService.GetBookingById(bookingId);
            var roomPackage = _bookingService.GetRoomPackageById(bookingFromDb.RoomPackageId);
            var roomSetup = _bookingService.GetRoomSetupById(bookingFromDb.RoomSetupId);

            roomPackage.RoomAmenity = _bookingService.GetRoomAmenitiesByPackageId(roomPackage.RoomPackageId); 

            bookingFromDb.RoomPackage = roomPackage;
            bookingFromDb.RoomSetup = roomSetup;

            return View(bookingFromDb);
        }


        [HttpPost]
        [Authorize(Roles = SD.Role_Admin)]
        public IActionResult Approve(Booking booking)
        {
            int roomPackageId = booking.RoomPackageId;
            int roomSetupId = booking.RoomSetupId;
            booking.ModifiedBy = User.Identity.Name; // Ensure the user is authenticated
            booking.ApprovedDate = DateTime.Now; // Ensure the user is authenticated
            booking.ModifiedDate = DateTime.Now; // Ensure the user is authenticated


            _bookingService.UpdateStatus(booking.Id,booking.ModifiedBy, SD.StatusApproved, roomPackageId, roomSetupId);
            TempData["Success"] = "Booking Updated Successfully.";
            return RedirectToAction(nameof(BookingDetails), new { bookingId = booking.Id });
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin)]
        public IActionResult Complete(Booking booking)
        {
            int roomPackageId = booking.RoomPackageId;
            int roomSetupId = booking.RoomSetupId;
            booking.ModifiedBy = User.Identity.Name; // Ensure the user is authenticated
            booking.ModifiedDate = DateTime.Now;
            _bookingService.UpdateStatus(booking.Id, booking.ModifiedBy,SD.StatusCompleted, roomPackageId, roomSetupId);
            TempData["Success"] = "Booking Completed Successfully.";
            return RedirectToAction(nameof(BookingDetails), new { bookingId = booking.Id });
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin)]
        public IActionResult CancelBooking(Booking booking)
        {
            if (booking.Id <= 0)
            {
                TempData["error"] = "Invalid booking ID.";
                return RedirectToAction(nameof(BookingDetails), new { bookingId = booking.Id });
            }
            var userName = User.Identity.Name; // Get the logged-in admin's username

            booking.ModifiedBy = userName; // Set the ModifiedBy with the admin's name
            booking.ModifiedDate = DateTime.Now;
            var existingBooking = _bookingService.GetBookingById(booking.Id);

            if (existingBooking == null)
            {
                TempData["error"] = "Booking not found.";
                return RedirectToAction(nameof(Index));
            }

            _bookingService.UpdateStatus(booking.Id, booking.ModifiedBy, SD.StatusCancelled,  existingBooking.RoomPackageId, existingBooking.RoomSetupId);
            TempData["Success"] = "Booking Cancelled Successfully.";
            return RedirectToAction(nameof(BookingDetails), new { bookingId = booking.Id });
        }

        #region API Calls
        [HttpGet]
        [Authorize]
        public IActionResult GetAll(string status)
        {
            IEnumerable<Booking> objBookings;
            string userId = "";
            if (string.IsNullOrEmpty(status))
            {
                status = "";
            }

            if (!User.IsInRole(SD.Role_Admin))
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            }
            objBookings = _bookingService.GetAllBookings(userId, status);
            return Json(new { data = objBookings });
        }
        #endregion
    }
}
