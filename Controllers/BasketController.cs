using APIProject.Dto;
using AutoMapper;
using TalabatBLL.Entities;
using TalabatBLL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIProject.Controllers
{
    public class BasketController : BaseApiController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository basketRepository, IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetBasketById(string id)
        {
            var basket = await _basketRepository.GetBasketAsync(id);

            return Ok(basket ?? new CustomerBasket(id));
        }

        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto basket)
        {
            var customerBasket = _mapper.Map<CustomerBasket>(basket);
            var UpdatedBasket = await _basketRepository.UpdateBasketAsync(customerBasket);

            return Ok(UpdatedBasket);
        }

        [HttpDelete]
        public async Task DeleteBasketById(string id)
        {
            await _basketRepository.DeleteBasketAsync(id);
        }
    }
}
