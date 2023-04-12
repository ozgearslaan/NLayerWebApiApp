﻿using Microsoft.EntityFrameworkCore;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using NLayer.Repository.UnitOfWorks;
using NLayer.Service.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Service.Services
{
    public class Service<T> : IService<T> where T : class
    {
        private readonly IGenericRepository<T> _repository;
        //repo ile haberleşecek
        private readonly IUnitOfWork _unitOfWork;
        //repoda savechangei çağırmadık o yüzden unitofworkle haberleşecek
        public Service(IGenericRepository<T> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }
        

        public async Task<T> AddAsync(T entity)
        {
            await _repository.AddAsync(entity);
            await _unitOfWork.CommitAsycn();
            //savechangei çağırdık, commitasync demiştik
            return entity;
        }

        public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
        {
            await _repository.AddRangeAsync(entities);
            await _unitOfWork.CommitAsycn();
            return entities;
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
        {
            return await _repository.AnyAsync(expression);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            //tolist tolistasnc çağırılır
            return await _repository.GetAll().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var hasProduct = await _repository.GetByIdAsync(id);
            //try cash yaptık return yerine 
            if(hasProduct == null) 
            {
                throw new NotFoundException($"{typeof(T).Name}({id}) not found");
            }
            return hasProduct;
        }

        public async Task RemoveAsync(T entity)
        {
            //async var çünkü savechange çağırıcaz
            _repository.Remove(entity);
            await _unitOfWork.CommitAsycn();
        }

        public async Task RemoveRangeAsync(IEnumerable<T> entities)
        {
            _repository.RemoveRange(entities);
            await _unitOfWork.CommitAsycn();
        }

        public async Task UpdateAsync(T entity)
        {
            _repository.Update(entity);
            await _unitOfWork.CommitAsycn();
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> expression)
        {
            //ıqueryable dönüyor async yok
            return _repository.Where(expression);
            //where methodunun savechangeini apida çağıracağız
        }
    }
}