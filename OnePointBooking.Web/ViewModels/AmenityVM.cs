using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnePointBooking.Application.Common.Utility;
using OnePointBooking.Domain.Entities;

namespace OnePointBooking.Web.ViewModels
{
    public class AmenityVM
    {

        public Amenity? Amenity { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem>? RoomPackageList { get; set; }
    
    }
}
