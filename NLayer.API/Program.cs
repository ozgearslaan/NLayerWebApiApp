
using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLayer.API.Filters;
using NLayer.API.Middlewares;
using NLayer.API.Modules;
using NLayer.Core.UnitOfWorks;
using NLayer.Repository;
using NLayer.Repository.UnitOfWorks;
using NLayer.Service.Mapping;
using NLayer.Service.Validations;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options => options.Filters.Add(new ValidateFilterAttribute())).AddFluentValidation(x=>x.RegisterValidatorsFromAssemblyContaining<ProductDtoValidator>());
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});//validationın kendi döndüğü filtreyi kendi filtremizle baskıladık
//mvc tarafında filterı baskılamamıza gerek yok 
//apida default olarak açık çünkü
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();//cachele haberdar ettik

//notfoundfilterı program cs e de ekledik

//ayrı repositoryler kullanıyoruz ve ayrı ayrı interfaceler var onları da eklemek lazım ki migration yapabilelim

builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();
//ıunitofwork ile karşılaşırsan unitofworkten nesne alacaksın
//genericleri typeofla ekleyeceğiz, birden fazla generic alsaydı <,> veya <,,> koyacaktık
//builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
//builder.Services.AddScoped(typeof(IService<>), typeof(Service<>));
//T aldığı için <>
//mapping
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
    //migration dosyaları repository oluşacak ve appdbcontextte repository katmanında
    //o yüzden appdbcontextin bulunmuş olduğu assemblyi api tarafında uygulamaya haber vermemiz lazım
    //optionla içine girilir
});

builder.Host.UseServiceProviderFactory
    (new AutofacServiceProviderFactory());
//autofac kütüphanesi
//modül ekleyeceğiz ve dinamik olarak ekleme yapabileceğiz.

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
