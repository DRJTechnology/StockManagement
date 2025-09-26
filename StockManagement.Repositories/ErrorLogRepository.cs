using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using StockManagement.Models.Dto;
using StockManagement.Repositories.Interfaces;
using System.Data;

namespace StockManagement.Repositories
{
    public class ErrorLogRepository(IConfiguration configuration) : IErrorLogRepository
    {
        public async Task<int?> LogErrorAsync(ErrorLogDto errorDetails)
        {
            try
            {
                using var dbConnection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));

                await dbConnection.OpenAsync();

                var parameters = new DynamicParameters();
                parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                parameters.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                parameters.Add("@LogLevel", errorDetails.LogLevel);
                parameters.Add("@Location", errorDetails.Location);
                parameters.Add("@ErrorMessage", errorDetails.ErrorMessage);
                parameters.Add("@StackTrace", errorDetails.StackTrace);
                parameters.Add("@UserId", errorDetails.UserId);

                await dbConnection.ExecuteAsync("dbo.ErrorLog_Create", parameters, commandType: CommandType.StoredProcedure);

                if (parameters.Get<bool>("@Success"))
                {
                    return parameters.Get<int?>("@Id");
                }
                return null;
            }
            catch (Exception ex)
            {
                // If we can't log the error, we shouldn't throw another exception
                return null;
            }
        }
    }
}