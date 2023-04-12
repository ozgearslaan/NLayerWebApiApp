using Microsoft.EntityFrameworkCore;
using NLayer.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Repository.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
        //generic çünkü yazdığımız bu operasyonlar tüm entityler için geçerli olsun
    {
        protected readonly AppDbContext _context;
        //sadece miras alınan sınıflardan erişebilmek istediğimiz için protected
        //db ile ilgili işlem yapabilmek için appdbcontext nesnesine ihtiyaç var
        private readonly DbSet<T> _dbSet;
        //veritabanındaki tabloya denk geliyor
        //inşa edilirken set edildiği için readonly tanımladık.
        //sadece constractureda ya da inşa edilirken değer atanır daha sonradan atanamaz

        public GenericRepository(AppDbContext context)
        {
            _context= context;
            _dbSet= _context.Set<T>(); //set dbset dönüyor 
        }
        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbSet.AnyAsync(expression); 
        }

        public IQueryable<T> GetAll()
        {
            return _dbSet.AsNoTracking().AsQueryable();
            //daha veritabanına gitmeden iquerayble dönelim, tolist dyince dbye yansısın
            //asnotracking çekmiş olduğu verileri memorye atmasın, daha gızlı çalışmasını sağlar
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
            //async methodu yok, daha dbden silmez, sadece efcoreun memoryde 
            //id sine entityi track ediyor o entitynin stateini delete olarak işaretliyor
            //savechangei calıştırınca db de değişiklik oluyor
            //_context.Entry(entity).State=EntityState.Deleted
            //yukarıdaki methodla aynı
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
            //foreachle dönüyor
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> expression)
        {
            return _dbSet.Where(expression);
        }
    }


}
