using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Core.Services
{
    public interface IService<T> where T : class
    {
        //IGenericRepository ile aynı methodlar ancak farklı işlenebilir
        Task<T> GetByIdAsync(int id);
        IQueryable<T> Where(Expression<Func<T, bool>> expression);
        Task<bool> AnyAsync(Expression<Func<T, bool>> expression);

        Task<IEnumerable<T>> GetAllAsync();
        Task<T> AddAsync(T entity);
        //repositoryde void döndük çünkü dbcontext efcoreun async yoktu
        //ama veritabanına bu değişiklikleri serviste yansıtacağımız için
        //async a dönüştürdük
        Task UpdateAsync(T entity);
        Task RemoveAsync(T entity);
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);
        Task RemoveRangeAsync(IEnumerable<T> entities);
        //iservice'te veritabanına bu değişiklikleri yansıtacağımız için
        ////savechangeasync methodunu kullanacağımız için
        //bunları Task'e yani asenkrona dönüştürüyoruz
    }
}
