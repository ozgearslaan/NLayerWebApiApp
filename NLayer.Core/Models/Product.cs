using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Core.Models
{
    public class Product : BaseEntity
    {
        //public Product(string name)
        // {
        //     Name = name ?? throw new ArgumentNullException(nameof(Name));
        //  } //eğer null olursa exception hatası fırlat
        public string Name { get; set; }
        public int Stock { get; set; }
        public decimal Price { get; set; }
        //bire çok ilişki o yüzden category id gerekli
        public int CategoryId { get; set; }
        //foreign key
        //her bir productın da bir tane categorysi vardır
        public Category Category { get; set; }
        //navigation property
        public ProductFeature ProductFeature { get; set; }
        //navigation property

    }
}
