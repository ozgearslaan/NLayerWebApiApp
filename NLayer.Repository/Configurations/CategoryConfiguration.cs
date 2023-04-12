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
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            //primary key
            builder.HasKey(x => x.Id);
            //bir bir artan değer olmasını istiyorum
            builder.Property(x => x.Id).UseIdentityColumn();
            //null olamaz(zorunlu alan) ve 50 karakter maximum
            builder.Property(x=> x.Id).IsRequired().HasMaxLength(50);
            builder.ToTable("Categories");
            //eğer bunu tanımlamazsak dbsetteki propertye verdiğimiz ismi default olarak alır
        }
    }
}
