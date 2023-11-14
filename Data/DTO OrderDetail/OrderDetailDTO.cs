using FluentValidation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using WebApiDB.Context;
using WebApiDB.Models;

namespace CeilingCalc.Data.DTO_OrderDetail
{
    public class OrderDetailDTO
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
     
        /// <summary>
        /// Material id not null, must exist 
        /// </summary>
        [JsonPropertyName("materialId")]
        public int MaterialId { get; set; }

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

    public class OrderDetailDTOValidator : AbstractValidator<OrderDetailDTO>
    {
        public OrderDetailDTOValidator(AplicationContext context)
        {
            RuleLevelCascadeMode = CascadeMode.Stop;
            RuleFor(x => x.Sum).GreaterThanOrEqualTo(0).WithMessage("{PropertyName} amount must be 0 or more").LessThan(float.MaxValue).WithMessage("To mach sum");
            RuleFor(x => x.Count).GreaterThanOrEqualTo(0).WithMessage("{PropertyName} must be 0 or more");
            RuleFor(x => x.MaterialId).Must(materialId => context.Materials.Any(material => material.Id == materialId))
            .WithMessage("Material does not exist");
            RuleFor(x => x.OrderId).Must(orderId => context.Orders.Any(order => order.Id == orderId))
            .WithMessage("Order does not exist");
        }
    }
}
