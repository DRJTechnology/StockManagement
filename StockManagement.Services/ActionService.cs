using AutoMapper;
using StockManagement.Models;
using StockManagement.Models.Dto;
using StockManagement.Repositories.Interfaces;
using StockManagement.Services.Interfaces;

namespace StockManagement.Services
{
    public class ActionService(IMapper mapper, IActionRepository actionRepository) : IActionService
    {
        public async Task<int> CreateAsync(int currentUserId, ActionEditModel action)
        {
            var actionDto = mapper.Map<ActionDto>(action);
            return await actionRepository.CreateAsync(currentUserId, actionDto);
        }

        public async Task<bool> DeleteAsync(int currentUserId, int actionId)
        {
            var returnVal = await actionRepository.DeleteAsync(currentUserId, actionId);
            return returnVal;
        }

        public async Task<List<ActionResponseModel>> GetAllAsync()
        {
            var actions = mapper.Map<List<ActionResponseModel>>(await actionRepository.GetAllAsync());
            return actions;
        }

        public async Task<bool> UpdateAsync(int currentUserId, ActionEditModel action)
        {
            var actionDto = mapper.Map<ActionDto>(action);
            return await actionRepository.UpdateAsync(currentUserId, actionDto);
        }
    }
}
