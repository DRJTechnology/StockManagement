using AutoMapper;
using StockManagement.Models;
using StockManagement.Models.Dto;
using StockManagement.Models.Enums;
using StockManagement.Repositories.Interfaces;
using StockManagement.Services.Interfaces;

namespace StockManagement.Services
{
    public class ContactService(IMapper mapper, IContactRepository contactRepository) : IContactService
    {
        public async Task<int> CreateAsync(int currentUserId, ContactEditModel contact)
        {
            var contactDto = mapper.Map<ContactDto>(contact);
            return await contactRepository.CreateAsync(currentUserId, contactDto);
        }

        public async Task<bool> DeleteAsync(int currentUserId, int contactId)
        {
            return await contactRepository.DeleteAsync(currentUserId, contactId);
        }

        public async Task<List<ContactResponseModel>> GetAllAsync()
        {
            var contacts = mapper.Map<List<ContactResponseModel>>(await contactRepository.GetAllAsync());
            return contacts;
        }

        public async Task<List<ContactResponseModel>> GetByTypeAsync(ContactTypeEnum contactType)
        {
            var contacts = mapper.Map<List<ContactResponseModel>>(await contactRepository.GetByTypeAsync(contactType));
            return contacts;
        }

        public async Task<bool> UpdateAsync(int currentUserId, ContactEditModel contact)
        {
            var contactDto = mapper.Map<ContactDto>(contact);
            return await contactRepository.UpdateAsync(currentUserId, contactDto);
        }
    }
}
