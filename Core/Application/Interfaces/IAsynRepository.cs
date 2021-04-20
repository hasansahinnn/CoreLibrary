using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces
{
    public interface  IAsyncRepository<T> where T: BaseEntity
    {
        Task<T> GetByIdAsync(Guid id, bool crypto = true);
        IQueryable<T> Get(Expression<Func<T, bool>> predicate, bool crypto = true);
        Task<List<T>> ListSpecificationAsync(ISpecification<T> specification);
        Task<IReadOnlyList<T>> ListAllAsync(bool crypto = true);
        Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec, bool crypto = true);
        Task<T> AddAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> UpdateRangeAsync(List<T> entity);
        Task<bool> AddRangeAsync(List<T> entity);
        Task DeleteAsync(T entity);
        Task<int> CountAsync(ISpecification<T> spec);
        Task<T> FirstAsync(ISpecification<T> spec, bool crypto = true);
        Task<T> FirstOrDefaultAsync(ISpecification<T> spec, bool crypto = true);
        Task<T> FirstOrDefaultAsync(ISpecification<T> spec, Guid Id, bool crypto = true);
    }
}
