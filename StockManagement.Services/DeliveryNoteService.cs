using AutoMapper;
using StockManagement.Models;
using StockManagement.Models.Dto;
using StockManagement.Repositories.Interfaces;
using StockManagement.Services.Interfaces;

namespace StockManagement.Services
{
    public class DeliveryNoteService(IMapper mapper, IDeliveryNoteRepository deliveryNoteRepository) : IDeliveryNoteService
    {
        public async Task<int> CreateAsync(int currentUserId, DeliveryNoteEditModel deliveryNote)
        {
            var dto = mapper.Map<DeliveryNoteDto>(deliveryNote);
            return await deliveryNoteRepository.CreateAsync(currentUserId, dto);
        }

        public async Task<bool> DeleteAsync(int currentUserId, int deliveryNoteId)
        {
            return await deliveryNoteRepository.DeleteAsync(currentUserId, deliveryNoteId);
        }

        public async Task<List<DeliveryNoteResponseModel>> GetAllAsync()
        {
            var deliveryNotes = await deliveryNoteRepository.GetAllAsync();
            return mapper.Map<List<DeliveryNoteResponseModel>>(deliveryNotes);
        }

        public async Task<DeliveryNoteResponseModel> GetByIdAsync(int deliveryNoteId)
        {
            var deliveryNote = await deliveryNoteRepository.GetByIdAsync(deliveryNoteId);
            return mapper.Map<DeliveryNoteResponseModel>(deliveryNote);
        }

        public async Task<bool> UpdateAsync(int currentUserId, DeliveryNoteEditModel deliveryNote)
        {
            var dto = mapper.Map<DeliveryNoteDto>(deliveryNote);
            return await deliveryNoteRepository.UpdateAsync(currentUserId, dto);
        }
    }
}
