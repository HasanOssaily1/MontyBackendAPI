using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MontyBackendAPI.Models
{
    public class Users
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required]
        [MaxLength(200)]
        public string? name { get; set; }
        public string? password { get; set; }
        [Required]
        [MaxLength(200)]
        [EmailAddress]
        public string? email { get; set; }
        public DateTime? creationdate { get; set; }
        public DateTime modificationdate { get; set; }
      
    }
}
