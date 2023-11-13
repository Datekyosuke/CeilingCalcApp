using CeilingCalc.Models;
using FluentValidation;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using WebApiDB.Models;

namespace CeilingCalc.Data.DTO_Material
{
    public class MaterialDTO
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
        public class MaterialValidator : AbstractValidator<Material>
        {
            public MaterialValidator()
            {
                RuleLevelCascadeMode = CascadeMode.Stop;
                RuleFor(x => x.Texture).Length(3, 50).WithMessage("{PropertyName} more 2 and less 50 character");
                RuleFor(x => x.Color).Length(2, 50).WithMessage("{PropertyName} more 2 and less 50 character");
                RuleFor(x => x.Size).NotNull().InclusiveBetween(float.MinValue, float.MaxValue).WithMessage("Wrong {PropertyName}! Too big (small) number");
                RuleFor(x => x.Price).NotNull().InclusiveBetween(float.MinValue, float.MaxValue).WithMessage("Wrong {PropertyName}! Too big (small) number");

            }
        }
    }
}
