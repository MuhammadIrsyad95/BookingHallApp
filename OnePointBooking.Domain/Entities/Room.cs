using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePointBooking.Domain.Entities
{
    public class Room
    {
        [Key]
        public int Id { get; set; } // Primary key

        [Required]  
        [Display(Name = "Room Name")]
        [MaxLength(100)]
        public string Name { get; set; }
        public string? Description { get; set; }
        [Required]
        public string Location { get; set; }
        public string Floor { get; set; }
        public string Company { get; set; }
        [Display(Name = "Price per days")]
        [Range(100000, 100000000)]
        public decimal Price { get; set; }
        
        [Range(1, int.MaxValue, ErrorMessage = "Capacity must be at least 1.")]
        public int Capacity { get; set; }
        [NotMapped]
        public IFormFile? Image { get; set; }
        [Display(Name = "Image Url")]
        public string? ImageUrl { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }

        [ValidateNever]
        public IEnumerable<RoomPackage> RoomPackage { get; set; }
        [ValidateNever]
        public IEnumerable<RoomSetup> RoomSetup { get; set; }

        // New property for services
        [ValidateNever]
        public IEnumerable<Service> Services { get; set; } // <-- Add this line

        [NotMapped]
        public bool isAvailable { get; set; } = true;
    }
}
