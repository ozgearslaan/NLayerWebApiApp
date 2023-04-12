using Microsoft.EntityFrameworkCore;
using NLayer.Core.Models;
using NLayer.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Repository.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {//product repository üzerinden hem genericrepository methodlarına erişeceğiz hem de ekstra methodlara ıproductrepositoryden erişicez
        public ProductRepository(AppDbContext context) : base(context)
        {

        }

        public async Task<List<Product>> GetProductsWithCategory()
        {
            //eager loading include ile datayı çekerken alınmasını istedik
            //datayı çekerken yüklemesini istedik
            return await _context.Products.Include(x=> x.Category).ToListAsync();
            //lazy loading var bir de producta bağlı kategoriyi de ihtiyaç olduğunda çekersek lazy loading olur 
        }

        
    }
}
