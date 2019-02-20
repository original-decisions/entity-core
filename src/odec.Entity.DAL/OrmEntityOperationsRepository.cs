using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using odec.Entity.DAL.Interop;
using odec.Framework.Generic;
using odec.Framework.Logging;

namespace odec.Entity.DAL
{
    /// <summary>
    /// Обобщенная реализация интерфейса IEntityOperations для Серверных объектов типа словарь.
    /// </summary>
    /// <typeparam name="TKey">Тип идентификатора </typeparam>
    /// <typeparam name="TEntity">Тип серверного объекта</typeparam>
    /// <typeparam name="TContext">Тип контекста</typeparam>
    public class OrmEntityOperationsRepository<TKey, TEntity, TContext> : 
        OrmDataRepository<TContext>,
        IEntityOperations<TKey, TEntity>,
        IActivatableEntity<TKey, TEntity>
        where TContext : DbContext//, new()
        where TEntity : Glossary<TKey>
        where TKey : struct
    {
        /// <summary>
        /// Базовая реализация - Получить серверный объект типа T по его идентификатору
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns>Возвращает -серверный объект типа Т</returns>
        public virtual TEntity GetById(TKey id)
        {
            try
            {
                LogEventManager.Logger.Info("Start execute GetById(TKey id)");
                return GetById<TKey, TEntity>(id);
            }
            //catch (DbEntityValidationException dbEx)
            //{
            //    var exception = dbEx.EntityValidationErrors.Aggregate<DbEntityValidationResult, Exception>(dbEx,
            //        (current1, validationErrors) =>
            //            validationErrors.ValidationErrors.Select(
            //                validationError =>
            //                    string.Format("{0}:{1}", validationErrors.Entry.Entity.ToString(),
            //                        validationError.ErrorMessage))
            //                .Aggregate(current1, (current, message) => new InvalidOperationException(message, current)));
            //    throw exception;
            //}
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
            finally
            {
                LogEventManager.Logger.Info("End execute GetById(TKey id)");
            }

        }

        /// <summary>
        /// Базовая реализация -Сохранение серверного объекта
        /// </summary>
        /// <param name="entity">Серверный объект типа Т</param>
        public virtual void Save(TEntity entity)
        {
            try
            {
                LogEventManager.Logger.Info("Start execute Save(TEntity entity)");
                AddOrUpdate(entity, e => e.Code == entity.Code);
            }
            //catch (DbEntityValidationException dbEx)
            //{
            //    var exception = dbEx.EntityValidationErrors.Aggregate<DbEntityValidationResult, Exception>(dbEx,
            //        (current1, validationErrors) =>
            //            validationErrors.ValidationErrors.Select(
            //                validationError =>
            //                    string.Format("{0}:{1}", validationErrors.Entry.Entity.ToString(),
            //                        validationError.ErrorMessage))
            //                .Aggregate(current1, (current, message) => new InvalidOperationException(message, current)));
            //    throw exception;
            //}
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
            finally
            {
                LogEventManager.Logger.Info("End execute Save(TEntity entity)");
            }
        }


        /// <summary>
        /// Базавая реализация -Сохранение серверного объекта
        /// </summary>
        /// <param name="entity">Серверный объект типа Т</param>
        public virtual void SaveById(TEntity entity)
        {
            try
            {
                LogEventManager.Logger.Info("Start execute Save(TEntity entity)");
                AddOrUpdate(entity, e => e.Id.Equals(entity.Id));
            }
            //catch (DbEntityValidationException dbEx)
            //{
            //    var exception = dbEx.EntityValidationErrors.Aggregate<DbEntityValidationResult, Exception>(dbEx,
            //        (current1, validationErrors) =>
            //            validationErrors.ValidationErrors.Select(
            //                validationError =>
            //                    string.Format("{0}:{1}", validationErrors.Entry.Entity.ToString(),
            //                        validationError.ErrorMessage))
            //                .Aggregate(current1, (current, message) => new InvalidOperationException(message, current)));
            //    throw exception;
            //}
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
            finally
            {
                LogEventManager.Logger.Info("End execute Save(TEntity entity)");
            }
        }
        /// <summary>
        /// Базовая реализация - Деактивация Серверного объекта типа Т по его идентификатору
        /// </summary>
        /// <param name="id">идентификатор</param>
        /// <returns>Возвращает - деактивированный серверный объект </returns>
        public virtual TEntity Deactivate(TKey id)
        {
            try
            {
                LogEventManager.Logger.Info("Start execute Deactivate(TKey id)");
                var entity = GetById(id);
                Deactivate(entity);
                return entity;
            }
            //catch (DbEntityValidationException dbEx)
            //{
            //    var exception = dbEx.EntityValidationErrors.Aggregate<DbEntityValidationResult, Exception>(dbEx,
            //        (current1, validationErrors) =>
            //            validationErrors.ValidationErrors.Select(
            //                validationError =>
            //                    string.Format("{0}:{1}", validationErrors.Entry.Entity.ToString(),
            //                        validationError.ErrorMessage))
            //                .Aggregate(current1, (current, message) => new InvalidOperationException(message, current)));
            //    throw exception;
            //}
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
            finally
            {
                LogEventManager.Logger.Info("End execute Deactivate(TKey id)");
            }
        }
        /// <summary>
        /// Базовая реализация- Деактивация Серверного объекта типа Т
        /// </summary>
        /// <param name="entity">Серверный объект типа Т</param>
        public virtual void Deactivate(TEntity entity)
        {

            entity.IsActive = false;
            Update(entity);


        }

        /// <summary>
        /// Базовая реализация- Активация серверного объекта типа Т по его индентификатору
        /// </summary>
        /// <param name="id">идентификатор</param>
        /// <returns>Возвращает - активированный серверный объект</returns>
        public TEntity Activate(TKey id)
        {
            try
            {
                LogEventManager.Logger.Info("Start execute Deactivate(TKey id)");
                var entity = GetById(id);
                Activate(entity);
                return entity;
            }
            //catch (DbEntityValidationException dbEx)
            //{
            //    var exception = dbEx.EntityValidationErrors.Aggregate<DbEntityValidationResult, Exception>(dbEx,
            //        (current1, validationErrors) =>
            //            validationErrors.ValidationErrors.Select(
            //                validationError =>
            //                    string.Format("{0}:{1}", validationErrors.Entry.Entity.ToString(),
            //                        validationError.ErrorMessage))
            //                .Aggregate(current1, (current, message) => new InvalidOperationException(message, current)));
            //    throw exception;
            //}
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
            finally
            {
                LogEventManager.Logger.Info("End execute Deactivate(TKey id)");
            }
        }
        /// <summary>
        /// Базовая реализация - Активация серверного объекта типа Т
        /// </summary>
        /// <param name="entity">Серверный объект типа Т</param>
        public void Activate(TEntity entity)
        {
            entity.IsActive = true;
            Update(entity);
        }
        /// <summary>
        /// Базовая реализация - Удаление серверного объекта типа Т по его идентификатору
        /// </summary>
        /// <param name="id">идентификатор</param>
        public virtual void Delete(TKey id)
        {
            try
            {
                LogEventManager.Logger.Info("Start execute Save(TEntity entity)");
                Delete(GetById(id));
            }
            //catch (DbEntityValidationException dbEx)
            //{
            //    var exception = dbEx.EntityValidationErrors.Aggregate<DbEntityValidationResult, Exception>(dbEx,
            //        (current1, validationErrors) =>
            //            validationErrors.ValidationErrors.Select(
            //                validationError =>
            //                    string.Format("{0}:{1}", validationErrors.Entry.Entity.ToString(),
            //                        validationError.ErrorMessage))
            //                .Aggregate(current1, (current, message) => new InvalidOperationException(message, current)));
            //    throw exception;
            //}
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
            finally
            {
                LogEventManager.Logger.Info("End execute Save(TEntity entity)");
            }
        }
        /// <summary>
        ///  Базовая реализация - Удаление серверного объекта типа Т
        /// </summary>
        /// <param name="entity">Серверный объект типа Т</param>
        public virtual void Delete(TEntity entity)
        {
            try
            {
                LogEventManager.Logger.Info("Start execute Save(TEntity entity)");
                Delete<TEntity>(entity);
            }
            //catch (DbEntityValidationException dbEx)
            //{
            //    var exception = dbEx.EntityValidationErrors.Aggregate<DbEntityValidationResult, Exception>(dbEx,
            //        (current1, validationErrors) =>
            //            validationErrors.ValidationErrors.Select(
            //                validationError =>
            //                    string.Format("{0}:{1}", validationErrors.Entry.Entity.ToString(),
            //                        validationError.ErrorMessage))
            //                .Aggregate(current1, (current, message) => new InvalidOperationException(message, current)));
            //    throw exception;
            //}
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
            finally
            {
                LogEventManager.Logger.Info("End execute Save(TEntity entity)");
            }
        }
    }
}
