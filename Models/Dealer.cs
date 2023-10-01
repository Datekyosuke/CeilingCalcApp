﻿using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebApiDB.Models
{
    public class Dealer
    {
        /// <summary>
        /// ID. Auto increment
        /// </summary>
        [Key]
        [JsonPropertyName("Id")]
        public int Id { get; set; }

        /// <summary>
        /// FirstName may be null
        /// </summary>
        [JsonPropertyName("FirstName")]
        public string FirstName { get; set; }

        /// <summary>
        /// LastName not null
        /// </summary>
        [JsonPropertyName("LastName")]
        public string LastName { get; set; }

        /// <summary>
        /// only numbers
        /// </summary>
        [JsonPropertyName("Telephone")]
        [Range(10000000000, 99999999999,
        ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public long? Telephone { get; set; }

        /// <summary>
        /// only numbers, may be negative  
        /// </summary>
        [JsonPropertyName("Debts")]
        public float Debts { get; set; }

        /// <summary>
        /// City of residence
        /// </summary>
        [JsonPropertyName("City")]
        public string City { get; set; }

      
    }
}
