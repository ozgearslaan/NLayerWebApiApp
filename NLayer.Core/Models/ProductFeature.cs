using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Core.Models
{
    public class ProductFeature
    {
        //base entity vermeye gerek yok çünkü zaten productın oluşturulma tarihi ile aynı
        public int Id { get; set; }
        public string Color { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public int ProductId { get; set; }
        //foreign key ProductId
        public Product Product { get; set; }
    }
}
