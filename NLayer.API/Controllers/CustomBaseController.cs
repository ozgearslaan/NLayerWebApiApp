using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLayer.Core.DTOs;

namespace NLayer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomBaseController : ControllerBase
    {
        //bu method endpoint değil o yüzden nonaction tanımlamak lazım
        //get veya post olmadığında swagger endpointolarak algılar ve hata fırlatır

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
            };//eğer statuscode 200se geriye 200 dönecel eğer 400se 400 dönecek
            //product controllerda ok badrequest yazmaya gerek kalmadan return edeceğiz
        }
    }
}
