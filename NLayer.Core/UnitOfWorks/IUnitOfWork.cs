using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Core.UnitOfWorks
{
    public interface IUnitOfWork
    {
        Task CommitAsycn();//transactionda commit methodu daha çok kullanılır
        void Commit();
        //dbcontextin savechange ve savechangeasync methodlarını çağırmış olacak
    }
}
