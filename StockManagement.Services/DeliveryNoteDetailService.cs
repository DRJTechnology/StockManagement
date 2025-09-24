using AutoMapper;
using StockManagement.Models;
using StockManagement.Models.Dto;
using StockManagement.Repositories.Interfaces;
using StockManagement.Services.Interfaces;

namespace StockManagement.Services
{
    public class DeliveryNoteDetailService(IMapper mapper, IDeliveryNoteDetailRepository deliveryNoteDetailRepository) : IDeliveryNoteDetailService
    {
        public async Task<int> CreateAsync(int currentUserId, DeliveryNoteDetailEditModel deliveryNote)
        {
            var dto = mapper.Map<DeliveryNoteDetailDto>(deliveryNote);
            return await deliveryNoteDetailRepository.CreateAsync(currentUserId, dto);
        }

        public async Task<bool> DeleteAsync(int currentUserId, int deliveryNoteId)
        {
            return await deliveryNoteDetailRepository.DeleteAsync(currentUserId, deliveryNoteId);
        }

        public async Task<bool> UpdateAsync(int currentUserId, DeliveryNoteDetailEditModel deliveryNote)
        {
            var dto = mapper.Map<DeliveryNoteDetailDto>(deliveryNote);
            return await deliveryNoteDetailRepository.UpdateAsync(currentUserId, dto);
        }
    }
}
