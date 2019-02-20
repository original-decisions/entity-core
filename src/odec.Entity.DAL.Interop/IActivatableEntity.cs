namespace odec.Entity.DAL.Interop
{
    public interface IActivatableEntity<in TKey, TEntity> where TKey : struct where TEntity : class
    {
        /// <summary>
        /// Деактивация Серверного объекта типа Т по его идентификатору
        /// </summary>
        /// <param name="id">идентификатор</param>
        /// <returns>Возвращает - деактивированный серверный объект </returns>
        TEntity Deactivate(TKey id);

        /// <summary>
        /// Деактивация Серверного объекта типа Т
        /// </summary>
        /// <param name="entity">Серверный объект типа Т</param>
        void Deactivate(TEntity entity);

        /// <summary>
        /// Активация серверного объекта типа Т по его индентификатору
        /// </summary>
        /// <param name="id">идентификатор</param>
        /// <returns>Возвращает - активированный серверный объект </returns>
        TEntity Activate(TKey id);

        /// <summary>
        /// Активация серверного объекта типа Т
        /// </summary>
        /// <param name="entity">Серверный объект типа Т</param>
        void Activate(TEntity entity);
    }
}