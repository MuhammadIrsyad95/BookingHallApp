using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePointBooking.Domain.Entities
{
    public class Amenity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        // Foreign key to RoomPackage
        [ForeignKey("RoomPackage")]
        public int RoomPackageId { get; set; }
        [ValidateNever]

        public RoomPackage RoomPackage { get; set; }

        public decimal Price { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; } // Nullable to indicate it might not be updated yet

        public string? UpdatedBy { get; set; }
    }
}
