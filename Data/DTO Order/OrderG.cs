﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using WebApiDB.Models;

namespace WebApiDB.Data.DTO_Order
{
    public class OrderG
    {
        /// <summary>
        /// ID. Auto increment
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonPropertyName("id")]
        public int Id { get; set; }

        /// <summary>
        /// Dealer last name + first name
        /// </summary>

        [JsonPropertyName("dealer")]
        public DealerDTOGet Dealer { get; set; }

        /// <summary>
        /// data
        /// </summary>
        [JsonPropertyName("dateOrder")]
        public DateTime DateOrder { get; set; }

        /// <summary>
        /// Operator id not null, must exist 
        /// </summary>
        [JsonPropertyName("operatorId")]
        public int OperatorId { get; set; } = 1;

        /// <summary>
        /// only numbers, may be negative  
        /// </summary>
        [JsonPropertyName("sum")]
        public float? Sum { get; set; }

        /// <summary>
        /// Status orders
        /// </summary>
        [JsonPropertyName("status")]
        public string Status { get; set; }
    }
}
