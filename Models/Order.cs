using CeilingCalc.Models;
using FluentValidation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using WebApiDB.Context;

namespace WebApiDB.Models
{
    public class Order
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

        [JsonPropertyName("dealerId")]
        public int DealerId { get; set; }
        public virtual Dealer Dealer { get; set; }

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
        public float Sum { get; set; } = 0;

        /// <summary>
        /// Status orders
        /// </summary>
        [JsonPropertyName("status")]
        public string Status { get; set; }

        public virtual ICollection<OrderDetail>? OrderDetail { get; set; }
    }

    public class OrderValidator : AbstractValidator<Order>
    {
        public OrderValidator(AplicationContext context)
        {
            RuleFor(x => x.DateOrder).LessThan(DateTime.Now).WithMessage("Сannot create an order in the future!");
            RuleFor(x => x.Sum).GreaterThanOrEqualTo(0).WithMessage("Order amount must be 0 or more").LessThan(float.MaxValue).WithMessage("To mach sum");
            RuleFor(x => x.Status).Length(0, 50).WithMessage("To long status!");
            RuleFor(x => x.DealerId).Must(dealerId => context.Dealers.Any(dealer => dealer.Id == dealerId))
           .WithMessage("Dealer does not exist");
        }
    }
}
