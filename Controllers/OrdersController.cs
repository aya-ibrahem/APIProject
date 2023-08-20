using APIProject.Dto;
using APIProject.Errors;
using AutoMapper;
using TalabatBLL.Entities.Order;
using TalabatBLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace APIProject.Controllers
{
    [Authorize]
    public class OrdersController : BaseApiController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrdersController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        //first method
        [HttpPost("CreateOrder")]
        public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
        {
            var email = User?.FindFirstValue(ClaimTypes.Email);
            /*(address dto)  الي address */
            var address = _mapper.Map<Address>(orderDto.ShipToAddress);
            var order = await _orderService.CreateOrderAsync(email, orderDto.DeliveryMethodId, orderDto.BasketId, address);

            //check order
            if (order is null)
                return BadRequest(new ApiResponse(400, "Problem Creating order"));
            return Ok(order);

        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderDetailsDto>>> GetOrdersForUser()
        {
            var email = User?.FindFirstValue(ClaimTypes.Email);
            var orders = await _orderService.GetOrdersForUserAsync(email);
            return Ok(_mapper.Map<IReadOnlyList<OrderDetailsDto>>(orders));
        }
        //send parameter
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDetailsDto>> GetOrderByIdForUser(/*[FromHeader] [Required]*/ int id)
        {
            var email = User?.FindFirstValue(ClaimTypes.Email);
            var order= await _orderService.GetOrderByIdAsync(id,email);
            if(order is null)
                return NotFound(new ApiResponse(404));
            return Ok(_mapper.Map<OrderDetailsDto>(order));
        }

        [HttpGet("DeliveryMethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
            => Ok(await _orderService.GetDeliveryMethodsAsync());
    }
}
