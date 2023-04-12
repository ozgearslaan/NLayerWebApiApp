using Microsoft.EntityFrameworkCore;
using NLayer.Core.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Repository
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        //options alıyor çünkü veritabanı yolunu startup dosyasından vereceğiz.
        //kim için options appdbcontext için sonra base ile optionsa gönderiyoruz.
        {

        }

        //her bir entitye karşılık bir dbset oluşturacağız
        public DbSet<Category> Categories{get;set;}
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductFeature> ProductFeatures { get; set; }
        //productfeature ürünlerini product üstünden eklenebilir
        
        //entityler ile ilgili ayarları
        //yapabilmrk için migration esnasında override etmemiz gereken methodumuz var
        //kkategorideki idyi primary key olarak belirleyebiliriz 
        //efcore'daki gibi id isimlendirmesi yapmazsak bu şekilde belirtebiliriz
        //dbkontexte tanımlarsak cok karışık bir dosya olur o yüzden
        //her entity için ayarları ayrı bir dosyada yapmak lazım
        //configuration klasöründe yapıldığı gibi
        
        //public override void OnModelCreating(ModelBuilder modelBuilder)
        //{
          //  modelBuilder.Entity<Category>().HasKey(x => x.Id);
            //base.OnModelCreating(modelBuilder);
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //entity ile ilgili ayarları yapmak için override etmemiz gereken bir method var


            //.applyconfigurationfromassemnly
            //yani diyor ki git assembly içerisindeki tüm configuration dosyalarını oku
            //yukarıdaki interfacei implemente ettiği için tüm configuration dosyalarını bulur
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            //çalışmış olduğu assemblyi tarama getexecuting

            //tek birini uygulamak istesek
            //modelBuilder.ApplyConfiguration(new ProductConfiguration());


            modelBuilder.Entity<ProductFeature>().HasData(new ProductFeature()
                {
                    Id=1,
                    Color="Kırmızı",
                    Height = 100,
                    Width = 200,
                    ProductId=1
                },
                new ProductFeature()
                {
                    Id = 2,
                    Color = "Mavi",
                    Height = 300,
                    Width = 500,
                    ProductId = 2
                });




            base.OnModelCreating(modelBuilder);
        }

    }
}
