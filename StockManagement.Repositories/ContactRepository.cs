using System.Data;
using Dapper;
using StockManagement.Models.Dto;
using StockManagement.Models.Enums;
using StockManagement.Repositories.Interfaces;

namespace StockManagement.Repositories
{
    public class ContactRepository(IDbConnection dbConnection) : IContactRepository
    {
        public async Task<int> CreateAsync(int currentUserId, ContactDto contactDto)
        {
            if (contactDto == null)
            {
                throw new ArgumentNullException(nameof(contactDto));
            }

            var parameters = new DynamicParameters();
            parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
            parameters.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@ContactTypeId", contactDto.ContactTypeId);
            parameters.Add("@Name", contactDto.Name);
            parameters.Add("@Notes", contactDto.Notes);
            parameters.Add("@Deleted", contactDto.Deleted);
            parameters.Add("@CurrentUserId", currentUserId);

            await dbConnection.ExecuteAsync("dbo.Contact_Create", parameters, commandType: CommandType.StoredProcedure);

            if (parameters.Get<bool>("@Success"))
            {
                return parameters.Get<int>("@Id");
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        public async Task<bool> DeleteAsync(int currentUserId, int contactId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
            parameters.Add("@ContactId", contactId);
            parameters.Add("@CurrentUserId", currentUserId);

            await dbConnection.ExecuteAsync("dbo.Contact_Delete", parameters, commandType: CommandType.StoredProcedure);

            if (parameters.Get<bool>("@Success"))
            {
                return true;
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        public async Task<List<ContactDto>> GetAllAsync()
        {
            var parameters = new DynamicParameters();

            var contactList = await dbConnection.QueryAsync<ContactDto>("dbo.Contact_LoadAll", parameters, commandType: CommandType.StoredProcedure);
            return contactList.Cast<ContactDto>().ToList();
        }

        public async Task<List<ContactDto>> GetByTypeAsync(ContactTypeEnum contactType)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@ContactTypeId", (int)contactType);

            var contactList = await dbConnection.QueryAsync<ContactDto>("dbo.Contact_LoadByType", parameters, commandType: CommandType.StoredProcedure);
            return contactList.Cast<ContactDto>().ToList();
        }

        public async Task<bool> UpdateAsync(int currentUserId, ContactDto contactDto)
        {
            try
            {
                if (contactDto == null)
                {
                    throw new ArgumentNullException(nameof(contactDto));
                }

                var parameters = new DynamicParameters();
                parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                parameters.Add("@Id", contactDto.Id);
                parameters.Add("@ContactTypeId", contactDto.ContactTypeId);
                parameters.Add("@Name", contactDto.Name);
                parameters.Add("@Notes", contactDto.Notes);
                parameters.Add("@Deleted", contactDto.Deleted);
                parameters.Add("@CurrentUserId", currentUserId);

                await dbConnection.ExecuteAsync("dbo.Contact_Update", parameters, commandType: CommandType.StoredProcedure);

                if (parameters.Get<bool>("@Success"))
                {
                    return true;
                }
                else
                {
                    throw new UnauthorizedAccessException();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
