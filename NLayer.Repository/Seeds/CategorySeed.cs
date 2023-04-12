using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NLayer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Repository.Seeds
{
    public class CategorySeed : IEntityTypeConfiguration<Category>
    {
        //BURADAKİ İŞLEMİ APPDBCONTEXTİN İÇİNDE DE YAPABİLİRİZ
        //ama amaç dbcontexti kirletmemek
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            //seed yaparken
            //migration esnasında default kayıtlarımızın oluşmasını istiyorsak
            //id'leri kendimizin vermesi lazım, normalde entity bi alan new anahtar sözcüğü oluşturarak
            //dbye kaydettiğimizde sql server tarafında birer birer artan bir değerle
            //id alanı belirlenecek ancak seed data esnasında id yi kendimiz vermeliyiz
            builder.HasData(
                new Category { Id = 1, Name = "Kalemler" }, 
                new Category { Id = 2, Name = "Kitaplar" }, 
                new Category { Id = 3, Name = "Defterler" });

        }
    }
}
