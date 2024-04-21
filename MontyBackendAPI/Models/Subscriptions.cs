using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MontyBackendAPI.Models
{
    public class Subscriptions
    {
        public int id { get; set; }
        [Required]
        public int userid { get; set; }
        [Required]
        [MaxLength(200)]
        public string? type { get; set; }
        [Required]
        [MaxLength(200)]
        public string? title { get; set; }
        [Required]
        public DateTime? startdate { get; set; }
        [Required]
        public DateTime? enddate { get; set; }
        public DateTime? creationdate { get; set; }
        public DateTime modificationdate { get; set; }


        public int RemainingDays
        {
            get
            {
                if (enddate.HasValue)
                {
                    TimeSpan remainingTime = enddate.Value - DateTime.UtcNow;
                    return remainingTime.Days > 0 ? remainingTime.Days : 0;
                }
                else
                {
                    return 0; // Return 0 for ongoing subscriptions
                }
            }
        }
    }
}
