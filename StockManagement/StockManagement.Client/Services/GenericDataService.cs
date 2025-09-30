using StockManagement.Client.Interfaces;
using StockManagement.Models.InternalObjects;
using System.Net.Http.Json;

namespace StockManagement.Client.Services
{
    public abstract class GenericDataService<TCreateEntity, TResponseEntity> : IGenericDataService<TCreateEntity, TResponseEntity>
    {
        protected HttpClient httpClient { get; }
        protected ErrorNotificationService ErrorService { get; }
        protected string ApiControllerName { get; set; } = string.Empty;

        public GenericDataService(HttpClient httpClient, ErrorNotificationService errorService)
        {
            this.httpClient = httpClient;
            this.ErrorService = errorService;
        }

        public async Task<int> CreateAsync(TCreateEntity entity)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync<TCreateEntity>($"api/{ApiControllerName}", entity);
                if (response == null || !response.IsSuccessStatusCode)
                {
                    throw new Exception($"Failed to create {ApiControllerName}: HTTP {response?.StatusCode}");
                }

                var returnValue = await response.Content.ReadFromJsonAsync<ApiResponse>();
                
                if (returnValue != null && !returnValue.Success)
                {
                    var errorMessage = !string.IsNullOrEmpty(returnValue.ErrorMessage) 
                        ? returnValue.ErrorMessage 
                        : $"Failed to create {ApiControllerName}";
                    throw new Exception(errorMessage);
                }

                return returnValue?.CreatedId ?? 0;
            }
            catch (Exception ex)
            {
                await ErrorService.NotifyErrorAsync(ex.Message);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var response = await httpClient.DeleteAsync($"api/{ApiControllerName}/{id}");
                if (response == null || !response.IsSuccessStatusCode)
                {
                    throw new Exception($"Failed to delete {ApiControllerName}: HTTP {response?.StatusCode}");
                }

                var returnValue = await response.Content.ReadFromJsonAsync<ApiResponse>();
                
                if (returnValue != null && !returnValue.Success)
                {
                    var errorMessage = !string.IsNullOrEmpty(returnValue.ErrorMessage) 
                        ? returnValue.ErrorMessage 
                        : $"Failed to delete {ApiControllerName}";
                    throw new Exception(errorMessage);
                }

                return returnValue?.Success ?? false;
            }
            catch (Exception ex)
            {
                await ErrorService.NotifyErrorAsync(ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<TResponseEntity>> GetAllAsync()
        {
            try
            {
                var returnVal = await httpClient.GetFromJsonAsync<IEnumerable<TResponseEntity>>($"api/{ApiControllerName}");
                return returnVal;
            }
            catch (Exception ex)
            {
                await ErrorService.NotifyErrorAsync(ex.Message);
                throw;
            }
        }

        public async Task<TResponseEntity> GetByIdAsync(int id)
        {
            try
            {
                return await httpClient.GetFromJsonAsync<TResponseEntity>($"api/{ApiControllerName}/{id}");
            }
            catch (Exception ex)
            {
                await ErrorService.NotifyErrorAsync(ex.Message);
                throw;
            }
        }

        public async Task<bool> UpdateAsync(TCreateEntity entity)
        {
            try
            {
                var response = await httpClient.PutAsJsonAsync<TCreateEntity>($"api/{ApiControllerName}", entity);
                if (response == null || !response.IsSuccessStatusCode)
                {
                    throw new Exception($"Failed to update {ApiControllerName}: HTTP {response?.StatusCode}");
                }

                var returnValue = await response.Content.ReadFromJsonAsync<ApiResponse>();
                
                if (returnValue != null && !returnValue.Success)
                {
                    var errorMessage = !string.IsNullOrEmpty(returnValue.ErrorMessage) 
                        ? returnValue.ErrorMessage 
                        : $"Failed to update {ApiControllerName}";
                    throw new Exception(errorMessage);
                }

                return returnValue?.CreatedId > 0;
            }
            catch (Exception ex)
            {
                await ErrorService.NotifyErrorAsync(ex.Message);
                throw;
            }
        }
    }
}
