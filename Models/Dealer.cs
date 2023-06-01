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
        public string LastName { get; set; }
        /// <summary>
        /// only numbers
        /// </summary>
        
        public long? Telephone { get; set; }
        /// <summary>
        /// only numbers, may be negative  
        /// </summary>
        public float Debts { get; set; }
        /// <summary>
        /// City of residence
        /// </summary>
        public string City { get; set; }

      
    }
}
