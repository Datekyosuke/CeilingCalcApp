using System.ComponentModel.DataAnnotations;

namespace WebApiDB.Models
{
    public class Material
    {
        /// <summary>
        /// ID. Auto increment
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Texture: mate, glosse, satin, exclusive etc.
        /// </summary>
        public string Texture { get; set; }

        /// <summary>
        /// Color not null, 
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// Size in meters
        /// </summary>
        public float Size { get; set; }

        /// <summary>
        /// Price per square meter in rubles
        /// </summary>
        public float Price { get; set; }

    }
}
