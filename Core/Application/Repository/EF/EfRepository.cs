using Ardalis.Specification;
using Core.Application;
using Core.Application.Interfaces;
using Core.DataSecurity;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.DataEncryption;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Core.Application.Repository.EF
{

    #region Example DB Context
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<UserEntity> UserEntity { get; set; }

    }

    public class CryptedDataContext : DataContext
    {
        private readonly IEncryptionProvider _provider;
        public CryptedDataContext(DbContextOptions<DataContext> options) : base(options)
        {
            this._provider = new Encryption();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseEncryption(this._provider);
            base.OnModelCreating(modelBuilder);
        }

    }

    #endregion


    public class EfRepository<T> :IAsyncRepository<T> where T : BaseEntity
    {
        private readonly DataContext unCryptedContext;
        private readonly CryptedDataContext cryptedContext;
        public EfRepository(DataContext context, CryptedDataContext _cryptedContext)
        {
            this.unCryptedContext = context;
            this.cryptedContext = _cryptedContext;
        }

        public async Task<T> GetByIdAsync(Guid id, bool crypto = true)
        {
            var context = crypto ? cryptedContext: unCryptedContext;
            T entity= await context.Set<T>().FindAsync(id);
            return entity;
        }
        public async Task<IReadOnlyList<T>> ListAllAsync(bool crypto = true)
        {
            var context = crypto ? cryptedContext : unCryptedContext;
            return await context.Set<T>().ToListAsync();
        }
        public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec, bool crypto = true)
        {
            try
            {
                var context = crypto ? cryptedContext : unCryptedContext;
                var specificationResult = ApplySpecification(spec);
                return await specificationResult.ToListAsync();
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public async Task<int> CountAsync(ISpecification<T> spec)
        {
            var specificationResult = ApplySpecification(spec);
            return await specificationResult.CountAsync();
        }
        public async Task<T> AddAsync(T entity)
        {
            try
            {
                await cryptedContext.Set<T>().AddAsync(entity);
                await cryptedContext.SaveChangesAsync();

                return entity;
            }
            catch (Exception e)
            {

                throw e;
            }
            
        }
        public async Task<bool> AddRangeAsync(List<T> entity)
        {
            try
            {
                foreach (var item in entity)
                    cryptedContext.Entry(item).State = EntityState.Added;
                await cryptedContext.Set<T>().AddRangeAsync(entity);
                await cryptedContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public IQueryable<T> Get(Expression<Func<T, bool>> predicate, bool crypto=true)
        {
            try
            {

                var context = crypto ? cryptedContext : unCryptedContext;
                var queryResult = context.Set<T>().Where(predicate);
                return queryResult;

            }
            catch (Exception e)
            {

                throw e;
            }
        }
        
     
        public async Task<bool> UpdateAsync(T entity)
        {
            try
            {
                cryptedContext.Entry(entity).State = EntityState.Modified;
                int result =await cryptedContext.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public async Task<bool> UpdateRangeAsync(List<T> entity)
        {
            try
            {
                foreach (var item in entity)
                  cryptedContext.Entry(item).State = EntityState.Modified;
                int result = await cryptedContext.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public async Task FullUpdateAsync(T target, T source)
        {
            try
            {
                cryptedContext.Entry(target).CurrentValues.SetValues(source);
                int t = await cryptedContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<List<T>> ListSpecificationAsync(ISpecification<T> specification)
        {
            return await ApplySpecification(specification).ToListAsync();
        }
        public async Task DeleteAsync(T entity)
        {
            cryptedContext.Set<T>().Remove(entity);
            await cryptedContext.SaveChangesAsync();
        }
        public async Task<T> FirstAsync(ISpecification<T> spec, bool crypto = true)
        {
            var specificationResult = ApplySpecification(spec);
            return await specificationResult.FirstAsync();
        }
        public async Task<T> FirstOrDefaultAsync(ISpecification<T> spec, bool crypto = true)
        {
            var specificationResult = ApplySpecification(spec);
            return await specificationResult.FirstOrDefaultAsync();
        }
        public async Task<T> FirstOrDefaultAsync(ISpecification<T> spec, Guid Id, bool crypto = true)
        {
            var specificationResult = ApplySpecification(spec);
            return await specificationResult.FirstOrDefaultAsync(x => x.Id == Id);
        }
        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            var evaluator = new Ardalis.Specification.EntityFrameworkCore.SpecificationEvaluator<T>();
            return evaluator.GetQuery(cryptedContext.Set<T>().AsQueryable(), spec);
        }

    
    }
}
