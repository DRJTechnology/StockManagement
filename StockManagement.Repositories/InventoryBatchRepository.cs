using AutoMapper;
using Dapper;
using StockManagement.Models;
using StockManagement.Models.Dto.Finance;
using StockManagement.Models.Finance;
using StockManagement.Repositories.Interfaces;
using System.Data;
using System.Diagnostics;

namespace StockManagement.Repositories
{
    public class InventoryBatchRepository(IDbConnection dbConnection, IMapper mapper) : IInventoryBatchRepository
    {
        public async Task<InventoryBatchFilteredResponseModel> GetFilteredAsync(InventoryBatchFilterModel inventoryBatchFilterModel)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@InventoryBatchStatusId", (int)inventoryBatchFilterModel.Status);
                parameters.Add("@ProductId", inventoryBatchFilterModel.ProductId);
                parameters.Add("@ProductTypeId", inventoryBatchFilterModel.ProductTypeId);
                parameters.Add("@LocationId", inventoryBatchFilterModel.LocationId);
                parameters.Add("@PurchaseDate", inventoryBatchFilterModel.PurchaseDate);
                parameters.Add("@CurrentPage", inventoryBatchFilterModel.CurrentPage);
                parameters.Add("@PageSize", inventoryBatchFilterModel.PageSize);
                parameters.Add("@TotalPages", dbType: DbType.Int32, direction: ParameterDirection.Output);

                var filteredinventory = await dbConnection.QueryAsync<InventoryBatchDto>("finance.InventoryBatch_LoadFiltered", parameters, commandType: CommandType.StoredProcedure);

                return new InventoryBatchFilteredResponseModel()
                {
                    InventoryBatchList = mapper.Map<List<InventoryBatchResponseModel>>(filteredinventory.ToList()),
                    TotalPages = parameters.Get<int>("@TotalPages"),
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in GetFilteredAsync: {ex.Message}");
                throw;
            }
        }
    }
}
