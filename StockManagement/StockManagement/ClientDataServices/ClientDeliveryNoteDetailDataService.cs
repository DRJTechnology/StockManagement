using StockManagement.Client.Interfaces;
using StockManagement.Models;

namespace StockManagement.ClientDataServices
{
    public class ClientDeliveryNoteDetailDataService : IDeliveryNoteDetailDataService
    {
        public Task<int> CreateAsync(DeliveryNoteDetailEditModel entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> CreateAsync(StockSaleDetailEditModel entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int entityId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DeliveryNoteDetailResponseModel>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<DeliveryNoteDetailResponseModel> GetByIdAsync(int entityId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(DeliveryNoteDetailEditModel entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(StockSaleDetailEditModel entity)
        {
            throw new NotImplementedException();
        }
    }
}
