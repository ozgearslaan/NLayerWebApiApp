using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLayer.API.Filters;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Services;

namespace NLayer.API.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {



        private readonly IMapper _mapper;
        private readonly IService<Product> _service;
        //controllerlar sadece Service katmanını bilir

        
        public ProductsController(IMapper mapper, IService<Product> Service)
        {
            _mapper = mapper;
            _service = Service;
        }

        [NonAction]
        public IActionResult CreateActionResult<T>(CustomResponseDto<T> response)
        {//Iactionresultı implemente eden createactionresult
            //204 nocontent geriye bir şey dönme
            if (response.StatusCode == 204)
                return new ObjectResult(null)
                {
                    StatusCode = response.StatusCode
                };
            return new ObjectResult(response)
            {
                StatusCode = response.StatusCode
            };
        }
            //GET api/products/GetProductsWithCategory
            //[HttpGet("[action]")]
            //isim belirtmemize gerek yok [HttpGet("[action]")]
            //public async Task<IActionResult> GetProductsWithCategory()
            // {
            //   return CreateActionResult(await _service.GetProductWithCategory());
            //aşağıdaki methodlardaki 2 satırı gerçekten olması gereken yerde servis katmanında yaptık
            // }
            //genericler dtoya dönüştürmediği için içinde dtoya dönüştürüyoruz ancak yukarıda 
            //custom olduğu için özelleştirildiği için direkt api in istediği datayı tek satırda
            //dönebiliriz


            //GET api/products
            [HttpGet]
        public async Task<IActionResult> All()
        {
            var products = await _service.GetAllAsync();
            //bu bir entitydir geriye dto dönmek lazım
            var productsDtos = _mapper.Map<List<ProductDto>>(products.ToList());
            //ienumerable dönüyor liste dönüştürmek için tolist
            //return Ok(CustomResponseDto<List<ProductDto>>.Success(200, productDtos));
            return CreateActionResult(CustomResponseDto<List<ProductDto>>.Success(200, productsDtos));
        }//geriye tek bir dto dönücez o da customresponsedto dönecek
         //dönme işlemini custombasecontrollerdan alıyor CreateActionResult


        //GET /api/products/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _service.GetByIdAsync(id);
            //bu bir entitydir geriye dto dönmek lazım
            
           // if(product==null)
           // {
           //     return CreateActionResult(CustomResponseDto<ProductDto>.Fail(400,"Bu id'ye sahip ürün bulunamadı."));
           // }
           //yukarıda null kontrolü yaptık ancak kötü bir yol çünkü bunu birçok entityde de ayrı ayrı yapmamız gerekebilir
           //bunu servis katmanında çağıracağız
            
            var productsDto = _mapper.Map<ProductDto>(product);
            //ienumerable dönüyor liste dönüştürmek için tolist
            //return Ok(CustomResponseDto<List<ProductDto>>.Success(200, productDtos));
            return CreateActionResult(CustomResponseDto<ProductDto>.Success(200, productsDto));
        }

        [HttpPost()]
        public async Task<IActionResult> Save(ProductDto productDto)
        {
            var product = await _service.AddAsync(_mapper.Map<Product>(productDto));
            //bu bir entitydir geriye dto dönmek lazım
            var productsDto = _mapper.Map<ProductDto>(product);
            //ienumerable dönüyor liste dönüştürmek için tolist
            //return Ok(CustomResponseDto<List<ProductDto>>.Success(200, productDtos));
            return CreateActionResult(CustomResponseDto<ProductDto>.Success(201, productsDto));
            //201 created

        }
       
        [HttpPut]
        public async Task<IActionResult> Update(ProductDto productDto)
        {
            await _service.UpdateAsync(_mapper.Map<Product>(productDto));
            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        }

        //DELETE api/product/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var product = await _service.GetByIdAsync(id);
            //bu idye sahip ürün var mı diye if leri yazmayacağız exceptionları ayrı yerden fırlatacağız
            await _service.RemoveAsync(product);
            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        }//remove ve updatede geriye bir şey dönmeye gerek yok o yüzden nocontentdto döndü
    }
}
