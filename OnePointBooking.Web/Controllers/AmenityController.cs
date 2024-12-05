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
    public class AmenityController : Controller
    {
        private readonly IAmenityService _amenityService;
        private readonly IRoomPackageService _roomPackageService;
        public AmenityController(IAmenityService amenityService, IRoomPackageService roomPackageService)
        {
            _amenityService = amenityService;   
            _roomPackageService = roomPackageService;
        }
        public IActionResult Index()
        {
            var amenities = _amenityService.GetAllAmenities();
            return View(amenities);
        }
        public IActionResult Create()
        {
            AmenityVM amenityVM = new()
            {
                RoomPackageList = _roomPackageService.GetAllRoomPackages().Select(u => new SelectListItem
                {
                    Text = u.PackageName,
                    Value = u.RoomPackageId.ToString()
                })
            };
            return View(amenityVM);
        }

        [HttpPost]
        public IActionResult Create(AmenityVM obj)
        {
            if (ModelState.IsValid)
            {
                obj.Amenity.CreatedBy = User.Identity.Name; ;
                obj.Amenity.CreatedDate = DateTime.Now;

                _amenityService.CreateAmenity(obj.Amenity);
                TempData["success"] = "The Amenity has been created successfully.";
                return RedirectToAction(nameof(Index));
            }
            obj.RoomPackageList = _roomPackageService.GetAllRoomPackages().Select(u => new SelectListItem
            {
                Text = u.PackageName,
                Value = u.RoomPackageId.ToString()
            });

            return View();
        }
        public IActionResult Update(int amenityId)
        {
            AmenityVM amenityVM = new()
            {
                RoomPackageList = _roomPackageService.GetAllRoomPackages().Select(u => new SelectListItem
                {
                    Text = u.PackageName,
                    Value = u.RoomPackageId.ToString()
                }),
                Amenity = _amenityService.GetAmenityById(amenityId)
            };
            if (amenityVM.Amenity == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(amenityVM);
        }

        [HttpPost]
        public IActionResult Update(AmenityVM amenityVM)
        {
            if (ModelState.IsValid)
            {
                // Ambil Amenity dari DB untuk mendapatkan CreatedBy dan CreatedDate yang lama
                var amenityFromDb = _amenityService.GetAmenityById(amenityVM.Amenity.Id);

                // Pastikan nilai CreatedBy dan CreatedDate tetap ada
                amenityVM.Amenity.CreatedBy = amenityFromDb.CreatedBy;
                amenityVM.Amenity.CreatedDate = amenityFromDb.CreatedDate;

                amenityVM.Amenity.UpdatedBy = User.Identity.Name; ;
                amenityVM.Amenity.UpdatedDate = DateTime.Now;
                _amenityService.UpdateAmenity(amenityVM.Amenity);
                TempData["success"] = "The amenity has been updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            amenityVM.RoomPackageList = _roomPackageService.GetAllRoomPackages().Select(u => new SelectListItem
            {
                Text = u.PackageName,
                Value = u.RoomPackageId.ToString()
            });
            return View(amenityVM);
        }
        public IActionResult Delete(int amenityId)
        {
            AmenityVM amenityVM = new()
            {
                RoomPackageList = _roomPackageService.GetAllRoomPackages().Select(u => new SelectListItem
                {
                    Text = u.PackageName,
                    Value = u.RoomPackageId.ToString()
                }),
                Amenity = _amenityService.GetAmenityById(amenityId)
            };
            if (amenityVM.Amenity == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(amenityVM);
        }
        [HttpPost]
        public IActionResult Delete(AmenityVM amenityVM)
        {
            Amenity? objFromDb = _amenityService.GetAmenityById(amenityVM.Amenity.Id);
            if (objFromDb is not null)
            {
                _amenityService.DeleteAmenity(objFromDb.Id);
                TempData["success"] = "The amenity has been deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "The amenity could not be deleted .";
            return View();
        }
    }
    
}
