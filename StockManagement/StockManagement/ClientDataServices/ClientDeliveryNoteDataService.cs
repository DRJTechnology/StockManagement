using StockManagement.Client.Interfaces;
using StockManagement.Models;

namespace StockManagement.ClientDataServices
{
    public class ClientDeliveryNoteDataService : IDeliveryNoteDataService
    {
        public Task<int> CreateAsync(DeliveryNoteEditModel entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int entityId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DeliveryNoteResponseModel>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<DeliveryNoteResponseModel> GetByIdAsync(int entityId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(DeliveryNoteEditModel entity)
        {
            throw new NotImplementedException();
        }
    }
}
