namespace BeautySalon.Services.Interfaces
{
    public interface IBaseService<TModel,TDb,TInsert,TSearch>
    {
        Task<TDb> Insert(TInsert insert);
        Task<TDb> GetAll();
        Task<TDb> GetByID(int id);

    }
}
