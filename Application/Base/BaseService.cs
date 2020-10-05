using Domain.Base;
using Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Application.Base
{
    public abstract class BaseService
    {
    }
    public abstract class Service<T> : BaseService, IService<T> where T : BaseEntity
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IGenericRepository<T> _repository;

        protected Service(IUnitOfWork unitOfWork, IGenericRepository<T> repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }
        protected virtual int Add(T entity)
        {
            _repository.Add(entity);
            return _unitOfWork.Commit();
        }
        protected virtual int Update(T entity)
        {
            _repository.Edit(entity);
            return _unitOfWork.Commit();
        }

        protected virtual List<T> Get(Expression<Func<T, bool>> expression = null, string include = "", uint page = 0, uint size = 10)
        {
            return _repository.FindBy(expression, includeProperties: include).Skip((int)(page * size)).Take((int)size).ToList();
        }

        protected virtual T Find(int id)
        {
            return _repository.Find(id);
        }
    }
}
