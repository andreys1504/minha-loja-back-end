using Microsoft.EntityFrameworkCore;
using MinhaLoja.Core.Domain.Entities.EntityBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MinhaLoja
{
    public static partial class Helpers
    {
        public static IQueryable<TEntity> Include<TEntity, TProperty>(
            this IQueryable<TEntity> source,
            Expression<Func<TEntity, TProperty>> navigationPropertyPath) where TEntity : Entity
        {
            return EntityFrameworkQueryableExtensions.Include(source, navigationPropertyPath);
        }

        public static async Task<bool> AnyAsync<TEntity>(
            this IQueryable<TEntity> source,
            Expression<Func<TEntity, bool>> predicate) where TEntity : Entity
        {
            return await EntityFrameworkQueryableExtensions.AnyAsync(source, predicate);
        }

        public static async Task<TEntity> FirstOrDefaultAsync<TEntity>(
            this IQueryable<TEntity> source)
        {
            return await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync(source);
        }

        public static async Task<TEntity> FirstOrDefaultAsync<TEntity>(
            this IQueryable<TEntity> source,
            Expression<Func<TEntity, bool>> predicate)
        {
            return await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync(source, predicate);
        }

        public static async Task<List<TEntity>> ToListAsync<TEntity>(
            this IQueryable<TEntity> source)
        {
            return await EntityFrameworkQueryableExtensions.ToListAsync(source);
        }
    }
}
