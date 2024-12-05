using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnePointBooking.Domain.Entities;

namespace OnePointBooking.Web.ViewModels
{
    public class RoomVM
    {
        public Room? Room { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem>? RoomPackageList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem>? RoomSetupList { get; set; }
    }
}
