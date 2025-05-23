using System.Data;
using Dapper;
using StockManagement.Models.Dto;
using StockManagement.Repositories.Interfaces;

namespace StockManagement.Repositories
{
    public class ProductProductTypeRepository(IDbConnection dbConnection) : IProductProductTypeRepository
    {
        public async Task<int> CreateAsync(int currentUserId, ProductProductTypeDto productProductTypeDto)
        {
            if (productProductTypeDto == null)
            {
                throw new ArgumentNullException(nameof(productProductTypeDto));
            }

            var parameters = new DynamicParameters();
            parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
            parameters.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@ProductId", productProductTypeDto.ProductId);
            parameters.Add("@ProductTypeId", productProductTypeDto.ProductTypeId);
            parameters.Add("@Deleted", productProductTypeDto.Deleted);
            parameters.Add("@CurrentUserId", currentUserId);

            await dbConnection.ExecuteAsync("dbo.ProductProductType_Create", parameters, commandType: CommandType.StoredProcedure);

            if (parameters.Get<bool>("@Success"))
            {
                return parameters.Get<int>("@Id");
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        public async Task<bool> DeleteAsync(int currentUserId, int productProductTypeId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
            parameters.Add("@ProductProductTypeId", productProductTypeId);
            parameters.Add("@CurrentUserId", currentUserId);

            await dbConnection.ExecuteAsync("dbo.ProductProductType_Delete", parameters, commandType: CommandType.StoredProcedure);

            if (parameters.Get<bool>("@Success"))
            {
                return true;
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        public async Task<List<ProductProductTypeDto>> GetAllAsync()
        {
            var parameters = new DynamicParameters();

            var productProductTypeList = await dbConnection.QueryAsync<ProductProductTypeDto>("dbo.ProductProductType_LoadAll", parameters, commandType: CommandType.StoredProcedure);
            return productProductTypeList.Cast<ProductProductTypeDto>().ToList();
        }

        public async Task<bool> UpdateAsync(int currentUserId, ProductProductTypeDto productProductTypeDto)
        {
            if (productProductTypeDto == null)
            {
                throw new ArgumentNullException(nameof(productProductTypeDto));
            }

            var parameters = new DynamicParameters();
            parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
            parameters.Add("@Id", productProductTypeDto.Id);
            parameters.Add("@ProductId", productProductTypeDto.ProductId);
            parameters.Add("@ProductTypeId", productProductTypeDto.ProductTypeId);
            parameters.Add("@Deleted", productProductTypeDto.Deleted);
            parameters.Add("@CurrentUserId", currentUserId);

            await dbConnection.ExecuteAsync("dbo.ProductProductType_Update", parameters, commandType: CommandType.StoredProcedure);

            if (parameters.Get<bool>("@Success"))
            {
                return true;
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }
    }
}
