using OnePointBooking.Domain.Entities;

namespace OnePointBooking.Web.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Room>? RoomList { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public int Days { get; set; }
        public int RoomSetupId { get; set; }  // Add this line
        public int RoomPackageId { get; set; } // Add this line
        public bool IsKalventis { get; set; } // Flag to indicate if it's a Kalventis package
        public bool IsAdmin { get; set; }  // Indicates if the user is an admin

    }
}
