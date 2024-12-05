using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnePointBooking.Domain.Entities
{
    public class ApplicationUser  : IdentityUser
    {
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<Company> Companies { get; set; } = new List<Company>(); // New Navigation Property
                                                                                   // Foreign key to Company
        [ForeignKey("Company")]
        public int CompanyId { get; set; }

        [ValidateNever]
        public Company Company { get; set; }

    }
}
