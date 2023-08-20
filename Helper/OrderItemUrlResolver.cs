using APIProject.Dto;
using AutoMapper;
using TalabatBLL.Entities;
using TalabatBLL.Entities.Order;

namespace APIProject.Helper
{
    public class OrderItemUrlResolver : IValueResolver<OrderItem, OrderItemdto, string>
    {
        private readonly IConfiguration _configuration;

        public OrderItemUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(OrderItem source, OrderItemdto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.ItemOrdered.PictureUrl))
                return _configuration["ApiUrl"] + source.ItemOrdered.PictureUrl;
            return null;
        }
    }
}
