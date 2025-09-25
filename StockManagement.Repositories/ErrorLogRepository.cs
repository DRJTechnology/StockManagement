using Dapper;
using StockManagement.Repositories.Interfaces;
using System.Data;

namespace StockManagement.Repositories
{
    public class ErrorLogRepository(IDbConnection dbConnection) : IErrorLogRepository
    {
        public async Task<int?> LogErrorAsync(int? userId, string procedureName, int? errorNumber, int? errorSeverity, int? errorState, int? errorLine, string errorMessage)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                parameters.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                parameters.Add("@ProcedureName", procedureName);
                parameters.Add("@ErrorNumber", errorNumber);
                parameters.Add("@ErrorSeverity", errorSeverity);
                parameters.Add("@ErrorState", errorState);
                parameters.Add("@ErrorLine", errorLine);
                parameters.Add("@ErrorMessage", errorMessage);
                parameters.Add("@UserId", userId);

                await dbConnection.ExecuteAsync("dbo.ErrorLog_Create", parameters, commandType: CommandType.StoredProcedure);

                if (parameters.Get<bool>("@Success"))
                {
                    return parameters.Get<int?>("@Id");
                }
                return null;
            }
            catch
            {
                // If we can't log the error, we shouldn't throw another exception
                return null;
            }
        }

        public async Task<int?> LogExceptionAsync(int userId, string procedureName, Exception exception)
        {
            return await LogErrorAsync(
                userId,
                procedureName,
                null, // No SQL error number for C# exceptions
                null, // No SQL error severity for C# exceptions
                null, // No SQL error state for C# exceptions
                null, // No SQL error line for C# exceptions
                $"{exception.GetType().Name}: {exception.Message}"
            );
        }
    }
}