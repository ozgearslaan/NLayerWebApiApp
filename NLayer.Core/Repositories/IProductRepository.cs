using NLayer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Core.Repositories
{
    public interface IProductRepository:IGenericRepository<Product>
    {//productla alakalı işlemler için artık IProductRepositoryi alacak
        Task<List<Product>> GetProductsWithCategory();
        //asenkron olacak

    }
}
