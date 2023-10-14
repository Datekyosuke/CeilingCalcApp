using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebApiDB.Models
{
    public class Material
    {
        /// <summary>
        /// ID. Auto increment
        /// </summary>
        [Key]
        [JsonPropertyName("id")]
        public int Id { get; set; }

        /// <summary>
        /// Texture: mate, glosse, satin, exclusive etc.
        /// </summary>
        [JsonPropertyName("texture")]
        public string Texture { get; set; }

        /// <summary>
        /// Color not null, 
        /// </summary>
        [JsonPropertyName("color")]
        public string Color { get; set; }

        /// <summary>
        /// Size in meters, 
        /// </summary>
        [JsonPropertyName("size")]
        public float Size { get; set; }

        /// <summary>
        /// Price per square meter in rubles
        /// </summary>
        [JsonPropertyName("price")]
        public float Price { get; set; }

    }
}
