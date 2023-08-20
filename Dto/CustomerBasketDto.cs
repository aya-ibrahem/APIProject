using TalabatBLL.Entities;
using System.ComponentModel.DataAnnotations;

namespace APIProject.Dto
{
    public class CustomerBasketDto
    {
        [Required]
        public string Id { get; set; }
        public List<BasketItem> Items { get; set; } 
        public int? DeliveryMethodId { get; set; }
        public decimal ShippingPrice { get; set; }

        public string? ClientSecret { get; set; }
        public string? PaymentIntentId { get; set; }
    }
}
