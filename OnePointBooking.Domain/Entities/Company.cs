using System.ComponentModel.DataAnnotations;

namespace OnePointBooking.Domain.Entities
{
    public class Company
    {
        [Key]
        public int Id { get; set; }  // Primary key

        [Required]
        [StringLength(100)]
        public string CompanyName { get; set; }

        public string Pic { get; set; }

        [Phone]
        public string Hp { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Url]
        public string Web { get; set; }
    }
}