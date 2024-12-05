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
    public class RoomSetup
    {
        [Key]
        public int RoomSetupId { get; set; }

        // Foreign key to Room
        [ForeignKey("Room")]
        public int RoomId { get; set; }

        [ValidateNever]
        public Room Room { get; set; }

        public string Type { get; set; }

        public int Capacity { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; } 
        public string? UpdatedBy { get; set; }
    }
}
