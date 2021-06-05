using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MinhaLoja.Core.Domain.Entities.EntityBase;

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
    }
}
