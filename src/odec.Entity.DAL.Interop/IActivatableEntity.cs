namespace odec.Entity.DAL.Interop
{
    public interface IActivatableEntity<in TKey, TEntity> where TKey : struct where TEntity : class
    {
        /// <summary>
        /// ����������� ���������� ������� ���� � �� ��� ��������������
        /// </summary>
        /// <param name="id">�������������</param>
        /// <returns>���������� - ���������������� ��������� ������ </returns>
        TEntity Deactivate(TKey id);

        /// <summary>
        /// ����������� ���������� ������� ���� �
        /// </summary>
        /// <param name="entity">��������� ������ ���� �</param>
        void Deactivate(TEntity entity);

        /// <summary>
        /// ��������� ���������� ������� ���� � �� ��� ���������������
        /// </summary>
        /// <param name="id">�������������</param>
        /// <returns>���������� - �������������� ��������� ������ </returns>
        TEntity Activate(TKey id);

        /// <summary>
        /// ��������� ���������� ������� ���� �
        /// </summary>
        /// <param name="entity">��������� ������ ���� �</param>
        void Activate(TEntity entity);
    }
}