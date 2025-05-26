using StockManagement.Client.Interfaces;
using StockManagement.Models.InternalObjects;
using System.Net.Http.Json;

namespace StockManagement.Client.Services
{
    public abstract class GenericDataService<TCreateEntity, TResponseEntity> : IGenericDataService<TCreateEntity, TResponseEntity>
    {
        protected HttpClient httpClient { get; }
        protected string ApiControllerName { get; set; } = string.Empty;

        public GenericDataService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<int> CreateAsync(TCreateEntity entity)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync<TCreateEntity>($"api/{ApiControllerName}", entity);
                if (response == null || !response.IsSuccessStatusCode)
                {
                    throw new Exception($"Failed to create {ApiControllerName}.");
                }

                var returnValue = await response.Content.ReadFromJsonAsync<ApiResponse>();

                return returnValue.CreatedId;
            }
            catch (Exception)
            {
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
                    throw new Exception($"Failed to delete {ApiControllerName}.");
                }

                var returnValue = await response.Content.ReadFromJsonAsync<ApiResponse>();

                return returnValue.Success;
            }
            catch (Exception)
            {
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
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<TResponseEntity> GetByIdAsync(int id)
        {
            try
            {
                return await httpClient.GetFromJsonAsync<TResponseEntity>($"api/{ApiControllerName}/{id}");
            }
            catch (Exception)
            {
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
                    throw new Exception($"Failed to update {ApiControllerName}.");
                }

                var returnValue = await response.Content.ReadFromJsonAsync<ApiResponse>();

                return returnValue.CreatedId > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
