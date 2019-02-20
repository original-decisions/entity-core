using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using odec.Framework.Generic;
using odec.Framework.Logging;
#if net452 || NETCOREAPP2_1
using System.Transactions;
#endif

namespace odec.Entity.DAL
{
    /// <summary>
    /// Описание основных операций в контексте, и представление их в более удобном виде.
    /// </summary>
    /// <typeparam name="TContext">Тип контекста, с которым будет работать класс</typeparam>
    public class OrmDataRepository<TContext> : IDisposable
       where TContext : DbContext//, new()
    {
        public TContext Db { get; set; }
        public string DbConnection { get; set; }
#if net452 || NETCOREAPP2_1
        public TransactionOptions TransactionOptions { get; set; }
#endif
        public OrmDataRepository()
        {
            // Database.SetInitializer<TContext>(null);
        }

        /// <summary>
        /// Добавляет, обновляет, удаляет запись в зависимости от состояния
        /// </summary>
        /// <param name="servEntity">Серверный объект</param>
        /// <param name="state">Состояние</param>
        /// <returns>Идентификатор (в случаи добавления новой записи)</returns>
        private object SaveChanges<TServerEntity>(TServerEntity servEntity, EntityState state)
            where TServerEntity : class
        {
            LogEventManager.Logger.Info("SaveChanges Started");
            try
            {
                Db.ChangeTracker.DetectChanges();
#if net452 || NETCOREAPP2_1
                using (var scope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromHours(1)))
                {
#endif
                var objSet = Db.Set<TServerEntity>();
                //todo: it is a mistake we shouldn't add entity if it is modified.
                if (state != EntityState.Modified && (state==EntityState.Added || state == EntityState.Detached))
                    objSet.Add(servEntity);
                Db.Entry(servEntity).State = state;
                Db.SaveChanges();
#if net452 || NETCOREAPP2_1
                    scope.Complete();
#endif
                return state != EntityState.Added ? null : servEntity;
#if net452 || NETCOREAPP2_1
                }
#endif
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Info(ex.Message, ex);
                throw;
            }
            finally
            {
                LogEventManager.Logger.Info("SaveChanges Ended");
            }

        }

        /// <summary>
        /// Добавляет объект
        /// </summary>
        /// <param name="servEntity">Серверный объект</param>
        /// <returns>Идентификатор</returns>
        public object Add<TServerEntity>(TServerEntity servEntity)
            where TServerEntity : class
        {

            return SaveChanges(servEntity, EntityState.Added);
        }

        public TServerEntity GetById<TKey, TServerEntity>(TKey id) 
            where TServerEntity: Glossary<TKey> //where TKey : struct
        {
            return Db.Set<TServerEntity>().SingleOrDefault(it=>it.Id.Equals(id));
        }

        /// <summary>
        /// Обновляет либодобавляет сущность в зависимости от того, есть ли она в бд
        /// </summary>
        /// <typeparam name="TServerEntity">Тип серверной сущности</typeparam>
        /// <param name="servEntity">серверный объект</param>
        /// <param name="expression">Имя свойства по которому стоит искать при добавлении, обновлении записи</param>
        /// <returns>Серверный объект</returns>
        public TServerEntity AddOrUpdate<TServerEntity>(TServerEntity servEntity, Func<TServerEntity, bool> expression)
            where TServerEntity : class
        {
            try
            {
#if net452 || NETCOREAPP2_1
                using (var scope = new TransactionScope())
                {
#endif
                var entityIsNotFound = !Db.Set<TServerEntity>().AsNoTracking().Any(expression);
                if (entityIsNotFound)
                    Add(servEntity);
                else
                    Update(servEntity);
#if net452 || NETCOREAPP2_1
                    scope.Complete();
#endif
                return servEntity;
#if net452 || NETCOREAPP2_1
                }
#endif
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }

        }

        /// <summary>
        /// Обновляет либодобавляет сущность в зависимости от того, есть ли она в бд
        /// </summary>
        /// <typeparam name="TServerEntity">Тип серверной сущности</typeparam>
        /// <param name="servEntity">серверный объект</param>
        /// <param name="expression">Имя свойства по которому стоит искать при добавлении, обновлении записи</param>
        /// <returns>Серверный объект</returns>
        public bool Exists<TServerEntity>(TServerEntity servEntity, Func<TServerEntity, bool> expression)
            where TServerEntity : class
        {
            try
            {
                return Db.Set<TServerEntity>().Any(expression);

            }
            catch (Exception)
            {

                throw;
            }

        }



        /// <summary>
        /// Редактирует объект
        /// </summary>
        /// <param name="servEntity">Серверный объект</param>
        public void Update<TServerEntity>(TServerEntity servEntity)
            where TServerEntity : class
        {
            SaveChanges(servEntity, EntityState.Modified);
        }

        /// <summary>
        /// Удаляет запись
        /// </summary>
        /// <param name="servEntity">Серверный объект</param>
        public void Delete<TServerEntity>(TServerEntity servEntity)
            where TServerEntity : class
        {
            SaveChanges(servEntity, EntityState.Deleted);
        }

        /// <summary>
        /// Удаляет запись
        /// </summary>
        /// <param name="id">идентификатор серверного объекта</param>
        public void Delete<TKey, TServerEntity>(TKey id)
            where TServerEntity :  Glossary<TKey>
        {

            var entity = GetById<TKey, TServerEntity>(id);
            if (entity ==null)
                return;
            SaveChanges(entity, EntityState.Deleted);
        }

        /// <summary>
        /// Удаляет записи удовлетворяющие условиям
        /// </summary>
        /// <param name="expression">Условия которым должны удовлетворять удаляемые записи</param>
        public void DeleteSet<TServerEntity>(Func<TServerEntity, bool> expression)
            where TServerEntity : class
        {
            //TODO:refactor this because it is slow if we create ctx many times
            IEnumerable<TServerEntity> entities;

            entities = Db.Set<TServerEntity>().Where(expression);

            foreach (var entity in entities)
                SaveChanges(entity, EntityState.Deleted);
        }

        /// <summary>
        /// Проверяет что запись единственна в своем роде по заданным условиям и удаляет ее.
        /// Используется для удаления записей в таблицах по условиям на кластерные ключи,
        /// для остальных вариантов рекомендуется использовать DeleteSet
        /// </summary>
        /// <param name="expression">Условия которым должны удовлетворять удаляемые записи</param>
        public void DeleteSingle<TServerEntity>(Func<TServerEntity, bool> expression)
            where TServerEntity : class
        {
            //TODO:refactor this because it is slow if we create ctx many times
            TServerEntity entity;
            entity = Db.Set<TServerEntity>().Single(expression);
            SaveChanges(entity, EntityState.Deleted);
        }

        public void Dispose()
        {
            Db.Dispose();
        }
    }


    public static class Extensions
    {
        public static TEntity Find<TEntity,TDbContex>(this DbSet<TEntity> set,TDbContex ctx, params object[] keyValues) where TEntity : class where TDbContex:DbContext
        {
            

            var entityType = ctx.Model.FindEntityType(typeof(TEntity));
            var key = entityType.FindPrimaryKey();

            var entries = ctx.ChangeTracker.Entries<TEntity>();

            var i = 0;
            foreach (var property in key.Properties)
            {
                entries = Enumerable.Where(entries, e => e.Property(property.Name).CurrentValue == keyValues[i]);
                i++;
            }

            var entry = entries.FirstOrDefault();
            if (entry != null)
            {
                // Return the local object if it exists.
                return entry.Entity;
            }

            // TODO: Build the real LINQ Expression
            // set.Where(x => x.Id == keyValues[0]);
            var parameter = Expression.Parameter(typeof(TEntity), "x");
            var query = set.Where((Expression<Func<TEntity, bool>>)
                Expression.Lambda(
                    Expression.Equal(
                        Expression.Property(parameter, "Id"),
                        Expression.Constant(keyValues[0])),
                    parameter));

            // Look in the database
            return query.FirstOrDefault();
        }
    }
}


