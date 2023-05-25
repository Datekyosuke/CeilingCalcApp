using System.ComponentModel.DataAnnotations;

namespace WebApiDB.Models
{
    public class Dealer
    {
        /// <summary>
        /// ID. Auto increment
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// FirstName may be null
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// LastName not null
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Last name is required!")]
        public string LastName { get; set; }
        /// <summary>
        /// only numbers
        /// </summary>
        
        [Range(100000, 100000000000, ErrorMessage = "Phone does not exist")]
        public long? Telephone { get; set; }
        /// <summary>
        /// only numbers, may be negative  
        /// </summary>
        public float Debts { get; set; }
        /// <summary>
        /// City of residence
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "City is required!")]
        public string City { get; set; }

      
    }
}
