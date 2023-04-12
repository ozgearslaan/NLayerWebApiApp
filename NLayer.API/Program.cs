
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
});//validation�n kendi d�nd��� filtreyi kendi filtremizle bask�lad�k
//mvc taraf�nda filter� bask�lamam�za gerek yok 
//apida default olarak a��k ��nk�
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();//cachele haberdar ettik

//notfoundfilter� program cs e de ekledik

//ayr� repositoryler kullan�yoruz ve ayr� ayr� interfaceler var onlar� da eklemek laz�m ki migration yapabilelim

builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();
//�unitofwork ile kar��la��rsan unitofworkten nesne alacaks�n
//genericleri typeofla ekleyece�iz, birden fazla generic alsayd� <,> veya <,,> koyacakt�k
//builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
//builder.Services.AddScoped(typeof(IService<>), typeof(Service<>));
//T ald��� i�in <>
//mapping
builder.Services.AddScoped(typeof(NotFoundFilter<>));
builder.Services.AddAutoMapper(typeof(MapProfile));

//builder.Services.AddScoped<IProductService,ProductService>();
//builder.Services.AddScoped<IProductRepository,ProductRepository>();
//productwithcategory i�in ekledik
//builder.Services.AddScoped<ICategoryService, CategoryService>();
//.Services.AddScoped<ICategoryRepository, CategoryRepository>();

builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"),option =>
    {
        option.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name);
    
    });
    //migration dosyalar� repository olu�acak ve appdbcontextte repository katman�nda
    //o y�zden appdbcontextin bulunmu� oldu�u assemblyi api taraf�nda uygulamaya haber vermemiz laz�m
    //optionla i�ine girilir
});

builder.Host.UseServiceProviderFactory
    (new AutofacServiceProviderFactory());
//autofac k�t�phanesi
//mod�l ekleyece�iz ve dinamik olarak ekleme yapabilece�iz.

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
