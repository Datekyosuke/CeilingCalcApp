using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using WebApiDB.Models;

namespace CeilingCalc.Models
{
    public class OrderDetail
    {
        /// <summary>
        /// ID. Auto increment
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonPropertyName("id")]
        public int Id { get; set; }

        /// <summary>
        /// Dealer id not null, must exist 
        /// </summary>


        [JsonPropertyName("orderId")]
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }

        /// <summary>
        /// Material id not null, must exist 
        /// </summary>
        [JsonPropertyName("materialId")]
        public int MaterialId { get; set; }
        public virtual Material Material { get; set; }

        /// <summary>
        /// Count in square meters
        /// </summary>
        [JsonPropertyName("count")]
        public float Count { get; set; }

        /// <summary>
        /// Count in square meters
        /// </summary>
        [JsonPropertyName("price")]
        public float Price { get; set; }

        /// <summary>
        /// Count in square meters
        /// </summary>
        [JsonPropertyName("sum")]
        public float Sum { get; set; }

    }
}
