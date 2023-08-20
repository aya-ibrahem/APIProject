using APIProject.Dto;
using APIProject.Errors;
using APIProject.Helper;
using AutoMapper;
using TalabatBLL.Entities;
using TalabatBLL.Interfaces;
using TalabatBLL.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Xml.Linq;

namespace APIProject.Controllers
{
    //[Authorize]
    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepositry<Product> _productRepository;
        private readonly IGenericRepositry<ProductBrand> _brandRepository;
        private readonly IGenericRepositry<ProductType> _typeRepository;
        private readonly IMapper _mapper;

        //public ProductsController(IProductRepository productRepository)
        //{
        //    _productRepository = productRepository;
        //}
        public ProductsController(IGenericRepositry<Product> productRepository, IGenericRepositry<ProductBrand> brandRepository, IGenericRepositry<ProductType> typeRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _brandRepository = brandRepository;
            _typeRepository = typeRepository;
            _mapper = mapper;
        }


        [HttpGet]
        [Cache(700)]
        [Route("GetProducts")]
        public async Task<ActionResult<Pagination<ProductDto>>> GetProducts([FromQuery] ProductSpecificationParameters productParameters)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(productParameters);
            var countSpec = new ProductsWithFilterForCountSpecifications(productParameters);
            var totalItems = await _productRepository.CountAsync(countSpec);
            var data = await _productRepository.ListWithSpecAsync(spec);
            var mappedData = _mapper.Map<IReadOnlyList<ProductDto>>(data);
            var paginatedList = new Pagination<ProductDto>(productParameters.PageIndex, productParameters.PageSize, totalItems, mappedData);
            return Ok(paginatedList);
        }

        [HttpGet("{id}")]
        [Cache(10)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);
            var data = await _productRepository.GetEntityWithSpec(spec);

            if (data is null)
                return NotFound(new ApiResponse(404));
            var mappedData = _mapper.Map<ProductDto>(data);
            return Ok(mappedData);
        }

        [HttpGet]

        [Route("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            var data = await _brandRepository.ListAllAsync();
            return Ok(data);
        }

        [HttpGet]
        [Route("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            var data = await _typeRepository.ListAllAsync();
            
            return Ok(data);
        }
    }
}
