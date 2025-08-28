using Dapper;
using StockManagement.Models.Dto;
using StockManagement.Repositories.Interfaces;
using System.Data;

namespace StockManagement.Repositories
{
    public class InventoryBatchRepository(IDbConnection dbConnection) : IInventoryBatchRepository
    {
        //public async Task<int> CreateAsync(int currentUserId, InventoryBatchDto inventoryBatchDto)
        //{
        //    try
        //    {
        //        if (inventoryBatchDto == null)
        //        {
        //            throw new ArgumentNullException(nameof(inventoryBatchDto));
        //        }

        //        var parameters = new DynamicParameters();
        //        parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
        //        parameters.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
        //        parameters.Add("@InventoryBatchStatusId", inventoryBatchDto.InventoryBatchStatusId);
        //        parameters.Add("@ProductId", inventoryBatchDto.ProductId);
        //        parameters.Add("@ProductTypeId", inventoryBatchDto.ProductTypeId);
        //        parameters.Add("@LocationId", inventoryBatchDto.LocationId);
        //        parameters.Add("@InitialQuantity", inventoryBatchDto.InitialQuantity);
        //        parameters.Add("@UnitCost", inventoryBatchDto.UnitCost);
        //        parameters.Add("@Deleted", inventoryBatchDto.Deleted);
        //        parameters.Add("@CurrentUserId", currentUserId);

        //        await dbConnection.ExecuteAsync("finance.InventoryBatch_Create", parameters, commandType: CommandType.StoredProcedure);

        //        if (parameters.Get<bool>("@Success"))
        //        {
        //            return parameters.Get<int>("@Id");
        //        }
        //        else
        //        {
        //            throw new UnauthorizedAccessException();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}
    }
}
