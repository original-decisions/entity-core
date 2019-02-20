namespace odec.Entity.DAL.Interop
{
    public interface IContextRepository<T>
    {
        T Db { get; set; }

        void SetConnection(string connection);

        void SetContext(T db);
    }
}
