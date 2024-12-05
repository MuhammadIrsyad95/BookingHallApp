using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace OnePointBooking.Domain.Entities
{
    public class RoomPackage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "RoomPackage")]
        public int RoomPackageId { get; set; }

        // Foreign key to Room
        [ForeignKey("Room")]
        public int RoomId { get; set; }

        [ValidateNever]
        public Room Room { get; set; }

        [Required]
        public string PackageName { get; set; }

        // Properti untuk harga dasar
        [Range(0, double.MaxValue)]
        public decimal BasePrice { get; set; }

        // Total harga paket yang dihitung berdasarkan harga dasar dan amenities
        [NotMapped] // Menandakan bahwa ini tidak perlu disimpan di database
        public decimal TotalPrice => BasePrice + (RoomAmenity?.Sum(a => a.Price) ?? 0);

        public DateTime? EffectiveDate { get; set; }

        public DateTime? ExpiredDate { get; set; }

        public string Status { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; } // Nullable to indicate it might not be updated yet

        public string? UpdatedBy { get; set; }

        [ValidateNever]
        public IEnumerable<Amenity> RoomAmenity { get; set; }
        public bool? IsKalventis { get; set; } = false; // Default false

    }
}
