namespace StockManagement.Client.Interfaces
{
    public interface IGenericDataService<TCreateEntity, TResponseEntity>
    {
        Task<IEnumerable<TResponseEntity>> GetAllAsync();
        Task<TResponseEntity> GetByIdAsync(int entityId);
        Task<int> CreateAsync(TCreateEntity entity);
        Task<bool> UpdateAsync(TCreateEntity entity);
        Task<bool> DeleteAsync(int entityId);
    }
}
