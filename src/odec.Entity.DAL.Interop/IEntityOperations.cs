namespace odec.Entity.DAL.Interop
{
    /// <summary>
    /// Интерфейс взаимодействия - операции над сущностями
    /// </summary>
    /// <typeparam name="TKey">Тип идентификатора серверного объекта</typeparam>
    /// <typeparam name="TEntity">Тип серверного объекта</typeparam>
    public interface IEntityOperations<in TKey, TEntity> 
        where TKey : struct
    {
        /// <summary>
        /// Получить серверный объект типа T по его идентификатору
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns>Возвращает -серверный объект типа Т</returns>
        TEntity GetById(TKey id);
        /// <summary>
        /// Сохранение серверного объекта
        /// </summary>
        /// <param name="entity">Серверный объект типа Т</param>
        void  Save(TEntity entity);

        /// <summary>
        /// Сохранение серверного объекта
        /// </summary>
        /// <param name="entity">Серверный объект типа Т</param>
        void SaveById(TEntity entity);

        /// <summary>
        /// Удаление серверного объекта типа Т по его идентификатору
        /// </summary>
        /// <param name="id">идентификатор</param>
        void Delete(TKey id);
        /// <summary>
        ///  Удаление серверного объекта типа Т
        /// </summary>
        /// <param name="entity">Серверный объект типа Т</param>
        void Delete(TEntity entity);
    }
}
