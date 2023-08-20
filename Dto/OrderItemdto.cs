using TalabatBLL.Entities.Order;

namespace APIProject.Dto
{
    public class OrderItemdto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}