using AutoMapper;
using StockManagement.Models;
using StockManagement.Models.Dto;
using StockManagement.Repositories.Interfaces;
using StockManagement.Services.Interfaces;

namespace StockManagement.Services
{
    public class SettingService(IMapper mapper, ISettingRepository settingRepository) : ISettingService
    {
        public async Task<List<SettingResponseModel>> GetAllAsync()
        {
            var settings = mapper.Map<List<SettingResponseModel>>(await settingRepository.GetAllAsync());
            return settings;
        }

        public async Task<bool> UpdateAsync(int currentUserId, SettingEditModel setting)
        {
            var settingDto = mapper.Map<SettingDto>(setting);
            return await settingRepository.UpdateAsync(currentUserId, settingDto);
        }
    }
}
