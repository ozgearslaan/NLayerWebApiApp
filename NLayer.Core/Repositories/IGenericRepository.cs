using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Core.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);//async method
        //productRepository.where(x->x.id>5).OrderBy.ToListAsync();
        IQueryable<T> Where(Expression<Func<T,bool>> expression); //list dönmedik-yazdığımız sorgular direkt veritabanına gitmez                                                //burdaki T x'e karşılık geliyor
        //IQueryable<T> Any(Expression<Func<T,bool>> expression);
        //senkron olarak true false dönmesi
        Task<bool> AnyAsync(Expression<Func<T, bool>> expression);
        IQueryable<T> GetAll();
        //Ienumerable dönmedik çünkü veritabanına hemen sorgu atmak istemedik
        //memoryde birleştirilir ve tek seferde veri tabanına gönderilir
        Task AddAsync(T entity);
        void Update(T entity);
        //update veya remove'un asenkron methodu yok
        //olmasına gerek yok çünkü efcore memorye alıp 
        //takip etmiş olduğu bir productı stateini değiştirir sadece
        //o yüzden bu uzun süren bir işlem olmadığı için asenkron methodu yok
        //bir şeyi update edebilmek için entity vermek lazım ve bu entity memoryde efcore tarafından takip ediliyor
        void Remove(T entity);
        //asenkron methodları neden kullanıyoruz 
        //çünkü var olan tradeleri bloklamamak için
        //tradeleri daha effektif kullanmak için

        Task AddRangeAsync(IEnumerable<T> entities);
        //birrden fazla kaydetme yapabiliriz
        //list almıyoruz, interface alıyoruz.
        //yazılımda soyut nesnelerle çalışmak önemli
        //çünkü istediğimiz bir tipe dönüştürebiliriz
        void RemoveRange(IEnumerable<T> entities);

        //void dedik çünkü dbcontextin asenkron için methodları yoktu
    }
}
