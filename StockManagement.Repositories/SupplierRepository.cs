using System.Data;
using Dapper;
using StockManagement.Models.Dto;
using StockManagement.Repositories.Interfaces;

namespace StockManagement.Repositories
{
    public class SupplierRepository(IDbConnection dbConnection) : ISupplierRepository
    {
        public async Task<int> CreateAsync(int currentUserId, SupplierDto SupplierDto)
        {
            if (SupplierDto == null)
            {
                throw new ArgumentNullException(nameof(SupplierDto));
            }

            var parameters = new DynamicParameters();
            parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
            parameters.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@SupplierName", SupplierDto.SupplierName);
            parameters.Add("@Deleted", SupplierDto.Deleted);
            parameters.Add("@CurrentUserId", currentUserId);

            await dbConnection.ExecuteAsync("dbo.Supplier_Create", parameters, commandType: CommandType.StoredProcedure);

            if (parameters.Get<bool>("@Success"))
            {
                return parameters.Get<int>("@Id");
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        public async Task<bool> DeleteAsync(int currentUserId, int SupplierId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
            parameters.Add("@SupplierId", SupplierId);
            parameters.Add("@CurrentUserId", currentUserId);

            await dbConnection.ExecuteAsync("dbo.Supplier_Delete", parameters, commandType: CommandType.StoredProcedure);

            if (parameters.Get<bool>("@Success"))
            {
                return true;
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        public async Task<List<SupplierDto>> GetAllAsync()
        {
            var parameters = new DynamicParameters();

            var SupplierList = await dbConnection.QueryAsync<SupplierDto>("dbo.Supplier_LoadAll", parameters, commandType: CommandType.StoredProcedure);
            return SupplierList.Cast<SupplierDto>().ToList();
        }

        public async Task<bool> UpdateAsync(int currentUserId, SupplierDto SupplierDto)
        {
            if (SupplierDto == null)
            {
                throw new ArgumentNullException(nameof(SupplierDto));
            }

            var parameters = new DynamicParameters();
            parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
            parameters.Add("@Id", SupplierDto.Id);
            parameters.Add("@SupplierName", SupplierDto.SupplierName);
            parameters.Add("@Deleted", SupplierDto.Deleted);
            parameters.Add("@CurrentUserId", currentUserId);

            await dbConnection.ExecuteAsync("dbo.Supplier_Update", parameters, commandType: CommandType.StoredProcedure);

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
