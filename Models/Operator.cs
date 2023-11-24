using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using WebApiDB.Models;

namespace CeilingCalc.Models
{
    public class Operator
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonPropertyName("id")]
        public int Id { get; set; }

        /// <summary>
        /// Login 
        /// </summary>
        [JsonPropertyName("login")]
        [Required]
        public string Login { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        [JsonPropertyName("password")]
        [Required]
        public string Password { get; set; }

        public virtual ICollection<Order>? Orders { get; set; }
    }
}
