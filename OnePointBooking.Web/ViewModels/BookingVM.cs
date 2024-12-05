using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnePointBooking.Application.Common.Utility;
using OnePointBooking.Domain.Entities;

namespace OnePointBooking.Web.ViewModels
{
    public class BookingVM
    {

        public Booking? Booking { get; set; }

        [ValidateNever]
        public IEnumerable<RoomSetup> RoomSetups { get; set; } // List of Room Setups
        [ValidateNever]
        public IEnumerable<RoomPackage> RoomPackages { get; set; } // List of Room Packages     
                                                                   // Add this property to hold the company list
        [ValidateNever]
        public IEnumerable<SelectListItem> Companies { get; set; } // List of companies for dropdown

    }
}
