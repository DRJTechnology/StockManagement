using System.Data;
using Dapper;
using StockManagement.Models.Dto;
using StockManagement.Repositories.Interfaces;

namespace StockManagement.Repositories
{
    public class ProductRepository(IDbConnection dbConnection) : IProductRepository
    {
        public async Task<int> CreateAsync(int currentUserId, ProductDto productDto)
        {
            if (productDto == null)
            {
                throw new ArgumentNullException(nameof(productDto));
            }

            var parameters = new DynamicParameters();
            parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
            parameters.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@ProductName", productDto.ProductName);
            parameters.Add("@Deleted", productDto.Deleted);
            parameters.Add("@CurrentUserId", currentUserId);

            await dbConnection.ExecuteAsync("dbo.Product_Create", parameters, commandType: CommandType.StoredProcedure);

            if (parameters.Get<bool>("@Success"))
            {
                return parameters.Get<int>("@Id");
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        public async Task<bool> DeleteAsync(int currentUserId, int productId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
            parameters.Add("@ProductId", productId);
            parameters.Add("@CurrentUserId", currentUserId);

            await dbConnection.ExecuteAsync("dbo.Product_Delete", parameters, commandType: CommandType.StoredProcedure);

            if (parameters.Get<bool>("@Success"))
            {
                return true;
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        public async Task<List<ProductDto>> GetAllAsync()
        {
            var parameters = new DynamicParameters();

            var productList = await dbConnection.QueryAsync<ProductDto>("dbo.Product_LoadAll", parameters, commandType: CommandType.StoredProcedure);
            return productList.Cast<ProductDto>().ToList();
        }

        public async Task<bool> UpdateAsync(int currentUserId, ProductDto productDto)
        {
            if (productDto == null)
            {
                throw new ArgumentNullException(nameof(productDto));
            }

            var parameters = new DynamicParameters();
            parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
            parameters.Add("@Id", productDto.Id);
            parameters.Add("@ProductName", productDto.ProductName);
            parameters.Add("@Deleted", productDto.Deleted);
            parameters.Add("@CurrentUserId", currentUserId);

            await dbConnection.ExecuteAsync("dbo.Product_Update", parameters, commandType: CommandType.StoredProcedure);

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
