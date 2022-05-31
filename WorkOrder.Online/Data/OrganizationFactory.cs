using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using WorkOrder.Online.Data.Interfaces;
using WorkOrder.Online.Data.Models;

namespace WorkOrder.Online.Data
{
    public class OrganizationFactory : IOrganizationFactory
    {
        private readonly IConfiguration _configuration;

        public OrganizationFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }
               
        public async Task<IEnumerable<OrganizationModel>> GetOrganizations()
        {
            var sql = @"SELECT 
                           [Id]
                          ,[Name]
                          ,[Address]
                          ,[City]
                          ,[Province]
                          ,[PostalCode]
                          ,[Phone]
                          ,[Email]
                          ,[Language]
                          ,[NbrUsers]
                          ,[Notes]
                          ,[CreationDate]
                          ,[IsActive]
                        FROM [dbo].[Organizations]";

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var result = await connection.QueryAsync<OrganizationModel>(sql,
                             commandType: CommandType.Text);

                return result;
            }
        }

        public async Task<int> Create(OrganizationModel model)
        {
            try
            {
                var sql = @"INSERT INTO [dbo].[Organizations]
                           ([Name]
                           ,[Address]
                           ,[City]
                           ,[Province]
                           ,[PostalCode]
                           ,[Phone]
                           ,[Email]
                           ,[Language]
                           ,[NbrUsers]
                           ,[Notes]
                           ,[CreationDate]
                           ,[IsActive])
                     VALUES
                           (@Name
                           ,@Address
                           ,@City
                           ,@Province
                           ,@PostalCode 
                           ,@Phone 
                           ,@Email 
                           ,@Language 
                           ,@NbrUsers 
                           ,@Notes 
                           ,@CreationDate
                           ,@IsActive)";

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    var result = await connection.ExecuteScalarAsync<int>(sql,
                        new
                        {
                            Name = model.Name,
                            Address = model.Address,
                            City = model.City,
                            Province = model.Province,
                            PostalCode = model.PostalCode,
                            Phone = model.Phone,
                            Email = model.Email,
                            Language = model.Language.ToUpper(),
                            NbrUsers = model.NbrUsers,
                            Notes = model.Notes,
                            CreationDate = model.CreationDate,
                            IsActive = model.IsActive
                        },
                        commandType: CommandType.Text);

                    return result;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> Update(OrganizationModel model)
        {
            try
            {
                var sql = @"UPDATE [dbo].[Organizations]
                           SET [Name] = @Name
                           ,[Address] = @Address
                           ,[City] = @City
                           ,[Province] = @Province
                           ,[PostalCode] = @PostalCode
                           ,[Phone] = @Phone
                           ,[Email] = @Email
                           ,[Language] = @Language
                           ,[NbrUsers] = @NbrUsers
                           ,[Notes] = @Notes
                           ,[IsActive] = @IsActive
                           WHERE Id = @Id";

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    var result = await connection.ExecuteScalarAsync<int>(sql,
                        new
                        {
                            Id = model.Id,
                            Name = model.Name,
                            Address = model.Address,
                            City = model.City,
                            Province = model.Province,
                            PostalCode = model.PostalCode,
                            Phone = model.Phone,
                            Email = model.Email,
                            Language = model.Language.ToUpper(),
                            NbrUsers = model.NbrUsers,
                            Notes = model.Notes,
                            CreationDate = model.CreationDate,
                            IsActive = model.IsActive
                        },
                        commandType: CommandType.Text);

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<int> Delete(int organizationId)
        {
            var sql = @"DELETE FROM [dbo].[Organizations]
                        WHERE Id = @Id";

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var result = await connection.ExecuteAsync(sql,
                    new
                    {
                        Id = organizationId
                    },
                    commandType: CommandType.Text);

                return result;
            }
        }

        public async Task<OrganizationModel> GetOrganization(int organizationId)
        {
            var sql = @"SELECT 
                           [Id]
                          ,[Name]
                          ,[Address]
                          ,[City]
                          ,[Province]
                          ,[PostalCode]
                          ,[Phone]
                          ,[Email]
                          ,[Language]
                          ,[NbrUsers]
                          ,[Notes]
                          ,[CreationDate]
                          ,[IsActive]
                        FROM [dbo].[Organizations]
                        WHERE ID = @Id";

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var result = await connection.QueryFirstOrDefaultAsync<OrganizationModel>(sql,
                             new
                             {
                                 Id = organizationId
                             },
                             commandType: CommandType.Text);

                return result;
            }
        }
    }
}
