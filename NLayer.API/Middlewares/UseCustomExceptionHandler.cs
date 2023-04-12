using Microsoft.AspNetCore.Diagnostics;
using NLayer.Core.DTOs;
using NLayer.Service.Exceptions;
using System.Text.Json;

namespace NLayer.API.Middlewares
{
    public static class UseCustomExceptionHandler
    {
        public static void UseCustomException(this IApplicationBuilder app) 
        {
            app.UseExceptionHandler(config =>
            {
                config.Run(async context =>
                {
                    context.Response.ContentType = "application/json";
                    //contenttypeı belirledim
                    var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();
                    //hata fırlatıldıysa al
                    var statusCode = exceptionFeature.Error switch
                    {
                        ClientSideException => 400,
                        NotFoundException => 404,
                        _ => 500
                    };
                    context.Response.StatusCode = statusCode;

                    var response = CustomResponseDto<NoContentDto>.Fail(statusCode, exceptionFeature.Error.Message);
                    //fail nocontent dön
                    await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                    //controllerda otomatik dönüyor burda kendimiz jsona döndürmeliyiz
                });
                //run sonlandırıcı bir middleware
                //request buraya girdiği anda geri dönecek
            });
        }
    }
}
