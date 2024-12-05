using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnePointBooking.Application.Common.Utility;
using OnePointBooking.Domain.Entities;

namespace OnePointBooking.Web.ViewModels
{
    public class ServiceVM
    {

        public Service? Service { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem>? RoomList { get; set; }
     

    }
}
