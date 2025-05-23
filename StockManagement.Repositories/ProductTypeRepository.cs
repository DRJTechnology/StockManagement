using System.Data;
using Dapper;
using StockManagement.Models.Dto;
using StockManagement.Repositories.Interfaces;

namespace StockManagement.Repositories
{
    public class ProductTypeRepository(IDbConnection dbConnection) : IProductTypeRepository
    {
        public async Task<int> CreateAsync(int currentUserId, ProductTypeDto productTypeDto)
        {
            if (productTypeDto == null)
            {
                throw new ArgumentNullException(nameof(productTypeDto));
            }

            var parameters = new DynamicParameters();
            parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
            parameters.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@ProductTypeName", productTypeDto.ProductTypeName);
            parameters.Add("@Deleted", productTypeDto.Deleted);
            parameters.Add("@CurrentUserId", currentUserId);

            await dbConnection.ExecuteAsync("dbo.ProductType_Create", parameters, commandType: CommandType.StoredProcedure);

            if (parameters.Get<bool>("@Success"))
            {
                return parameters.Get<int>("@Id");
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        public async Task<bool> DeleteAsync(int currentUserId, int productTypeId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
            parameters.Add("@ProductTypeId", productTypeId);
            parameters.Add("@CurrentUserId", currentUserId);

            await dbConnection.ExecuteAsync("dbo.ProductType_Delete", parameters, commandType: CommandType.StoredProcedure);

            if (parameters.Get<bool>("@Success"))
            {
                return true;
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        public async Task<List<ProductTypeDto>> GetAllAsync()
        {
            var parameters = new DynamicParameters();

            var productTypeList = await dbConnection.QueryAsync<ProductTypeDto>("dbo.ProductType_LoadAll", parameters, commandType: CommandType.StoredProcedure);
            return productTypeList.Cast<ProductTypeDto>().ToList();
        }

        public async Task<bool> UpdateAsync(int currentUserId, ProductTypeDto productTypeDto)
        {
            if (productTypeDto == null)
            {
                throw new ArgumentNullException(nameof(productTypeDto));
            }

            var parameters = new DynamicParameters();
            parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
            parameters.Add("@Id", productTypeDto.Id);
            parameters.Add("@ProductTypeName", productTypeDto.ProductTypeName);
            parameters.Add("@Deleted", productTypeDto.Deleted);
            parameters.Add("@CurrentUserId", currentUserId);

            await dbConnection.ExecuteAsync("dbo.ProductType_Update", parameters, commandType: CommandType.StoredProcedure);

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
