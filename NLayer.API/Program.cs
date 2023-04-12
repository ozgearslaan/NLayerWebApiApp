
using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;
using NLayer.API.Filters;
using NLayer.API.Middlewares;
using NLayer.API.Modules;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using NLayer.Repository;
using NLayer.Repository.Repositories;
using NLayer.Repository.UnitOfWorks;
using NLayer.Service.Mapping;
using NLayer.Service.Services;
using NLayer.Service.Validations;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options => options.Filters.Add(new ValidateFilterAttribute())).AddFluentValidation(x=>x.RegisterValidatorsFromAssemblyContaining<ProductDtoValidator>());
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});//validationýn kendi döndüðü filtreyi kendi filtremizle baskýladýk
//mvc tarafýnda filterý baskýlamamýza gerek yok 
//apida default olarak açýk çünkü
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();//cachele haberdar ettik

//notfoundfilterý program cs e de ekledik

//ayrý repositoryler kullanýyoruz ve ayrý ayrý interfaceler var onlarý da eklemek lazým ki migration yapabilelim

builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();
//ýunitofwork ile karþýlaþýrsan unitofworkten nesne alacaksýn
//genericleri typeofla ekleyeceðiz, birden fazla generic alsaydý <,> veya <,,> koyacaktýk
//builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
//builder.Services.AddScoped(typeof(IService<>), typeof(Service<>));
//T aldýðý için <>
//mapping
builder.Services.AddScoped(typeof(NotFoundFilter<>));
builder.Services.AddAutoMapper(typeof(MapProfile));

//builder.Services.AddScoped<IProductService,ProductService>();
//builder.Services.AddScoped<IProductRepository,ProductRepository>();
//productwithcategory için ekledik
//builder.Services.AddScoped<ICategoryService, CategoryService>();
//.Services.AddScoped<ICategoryRepository, CategoryRepository>();

builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"),option =>
    {
        option.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name);
    
    });
    //migration dosyalarý repository oluþacak ve appdbcontextte repository katmanýnda
    //o yüzden appdbcontextin bulunmuþ olduðu assemblyi api tarafýnda uygulamaya haber vermemiz lazým
    //optionla içine girilir
});

builder.Host.UseServiceProviderFactory
    (new AutofacServiceProviderFactory());
//autofac kütüphanesi
//modül ekleyeceðiz ve dinamik olarak ekleme yapabileceðiz.

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder => containerBuilder.RegisterModule(new RepoServiceModule()));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCustomException();
app.UseAuthorization();

app.MapControllers();

app.Run();
