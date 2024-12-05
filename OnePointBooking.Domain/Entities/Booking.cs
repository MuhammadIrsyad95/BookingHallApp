using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace OnePointBooking.Domain.Entities
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }

        // Foreign key to User
        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        // Foreign key to Room
        [Required]
        public int RoomId { get; set; }
        [ForeignKey("RoomId")]
        public Room Room { get; set; }

        // Foreign key to RoomPackage
        [Required]
        public int RoomPackageId { get; set; }
        [ForeignKey("RoomPackageId")]
        public RoomPackage RoomPackage { get; set; }

        [Required]
        // Foreign key to RoomSetup
        public int RoomSetupId { get; set; }
        [ForeignKey("RoomSetupId")]
        public RoomSetup RoomSetup { get; set; }
        // Foreign key to Service
        //[Required] a
        //public int? ServiceId { get; set; }
        //[ForeignKey("ServiceId")]
        //public Service Service { get; set; }
        //[Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        public string Company { get; set; }
        public string Phone { get; set; }
        public decimal TotalCost { get; set; }
        public int Days { get; set; }
        public string? EventName { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Total participants must be at least 1.")]
        public int? TotalParticipant { get; set; }
        public string Status { get; set; }

        public DateTime BookingDate { get; set; }
        // Change to DateOnly
        [Required]
        public DateOnly StartDate { get; set; }

        [Required]
        public DateOnly EndDate { get; set; }
        public DateTime? ApprovedDate { get; set; }
        [NotMapped]
       
        public List<RoomPackage> AvailableRoomPackages { get; set; } = new List<RoomPackage>();

        [NotMapped]
        public List<RoomSetup> AvailableRoomSetups { get; set; } = new List<RoomSetup>();

        [NotMapped]
        public List<Service> AvailableServices { get; set; } = new List<Service>();
        [NotMapped]

        public List<SelectListItem> CompanyList { get; set; } = new List<SelectListItem>();

        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }


    }
}
