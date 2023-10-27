using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebApiDB.Models
{
    public class DTODealer
    {
        /// <summary>
        /// ID. Auto increment
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonPropertyName("id")]
        public int DealerId { get; set; }

        /// <summary>
        /// FirstName may be null
        /// </summary>
        [JsonPropertyName("firstName")]
        public string FirstName { get; set; }

        /// <summary>
        /// LastName not null
        /// </summary>
        [JsonPropertyName("lastName")]
        public string LastName { get; set; }

        /// <summary>
        /// only numbers
        /// </summary>
        [JsonPropertyName("telephone")]
        [Range(10000000000, 99999999999,
        ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public long? Telephone { get; set; }

        /// <summary>
        /// only numbers, may be negative  
        /// </summary>
        [JsonPropertyName("debts")]
        public float Debts { get; set; }
        
        /// <summary>
        /// City of residence
        /// </summary>
        [JsonPropertyName("city")]
        public string City { get; set; }

    }
}
