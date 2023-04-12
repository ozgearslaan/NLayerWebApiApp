using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using NLayer.API.Filters;
using NLayer.Core.Services;

namespace NLayer.API.Controllers
{
    //[ValidateFilterAttribute] ama sürekli eklemek sıkıntı
    //o yüzden global tanımlamak lazım bir şey globalse
    //programcsde tanımlamak lazım 
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : CustomBaseController
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        //api/categories/GetSingleCategoryByIdWithProducts/2
        [HttpGet("[action]/{categoryId}")]
        public async Task<IActionResult> GetSingleCategoryByIdWithProducts(int categoryId)
        {//async olduğu için taskle kapsülledik
            return CreateActionResult(await _categoryService.GetSingleCategoryByIdWithProducts(categoryId));
        }

    }
}
