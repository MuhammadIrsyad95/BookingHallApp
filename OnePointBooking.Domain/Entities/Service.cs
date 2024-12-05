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
    public class Service
    {
        [Key]
        public int RoomServiceId { get; set; } // Primary key
        [ForeignKey("Room")]
        public int RoomId { get; set; }

        [ValidateNever]
        public Room Room { get; set; }

        public string ServiceName { get; set; } // Name of the service

        public string? Description { get; set; } // Description of the service
        // Foreign key to Room
        
        public DateTime? CreatedDate { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
