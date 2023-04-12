using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Core.Models
{

    public class Category : BaseEntity
    //baseentityden miras aldı
    {

        public string Name { get; set; }
        public ICollection<Product> Products { get; set; }
        //categorinin birden çok productı olabilir
        //entityler içerisindeki farklı classlara veya entitylere referans verdiğimiz propertylere navigation property diyoruz
        //kategoriden productlara gidebiliriz çünkü
    }
}
