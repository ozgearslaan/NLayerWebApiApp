using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NLayer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Repository.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x=>x.Id).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Stock).IsRequired();
            builder.Property(x=>x.Price).IsRequired().HasColumnType("decimal(18,2)");
            //hascolumntype virgülden sonra kaç sıfır olması gerektiği toplamda 18
            ////karakter olacak, virgülden sonra da 2 karakter olabilir
            builder.ToTable("Products");

            //efcore normalde bu ilişkiyi anlayacak
            //kategori ve product ilişkisini anlayacak(çünkü isimlendirme uygun Id gibi vs)ancak açık açık da verebiliriz
            //Bir productın bir kategorisi olabilir
            //bir kategorininde birden fazla productı olabilir
            //foreign keyi belirtebiliriz.

            builder.HasOne(x => x.Category).WithMany(x => x.Products).HasForeignKey(x => x.CategoryId);


        }
    }
}
