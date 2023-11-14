using FluentValidation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebApiDB.Models
{
    public class DealerDTOGet
    {
        /// <summary>
        /// ID. Auto increment
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonPropertyName("id")]
        public int Id { get; set; }

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

        public class DTODealerValidator : AbstractValidator<DealerDTOGet>
        {
            public DTODealerValidator()
            {
                RuleLevelCascadeMode = CascadeMode.Stop;
                RuleFor(x => x.FirstName).Must(c => c.All(Char.IsLetter)).WithMessage("Invalid character in {PropertyName}").MaximumLength(50).WithMessage("{PropertyName} maximum lenght 50 character");
                RuleFor(x => x.LastName).Must(c => c.All(Char.IsLetter)).WithMessage("Invalid character in {PropertyName}").Length(2, 50).WithMessage("{PropertyName} more 2 and less 50 character");
                RuleFor(x => x.Telephone).NotNull().InclusiveBetween(10000000000, 99999999999).WithMessage("Invalid {PropertyName}. Must contain 11 digits!");
                RuleFor(x => x.Debts).InclusiveBetween(float.MinValue, float.MaxValue).WithMessage("Wrong {PropertyName}! Too big (small) number");
                RuleFor(x => x.City).NotNull().WithMessage("{PropertyName} is requered!").Length(2, 50).WithMessage("{PropertyName} more 2 and less 50 character");

            }
        }

    }
}
