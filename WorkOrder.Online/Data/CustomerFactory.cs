using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using WorkOrder.Online.Data.Interfaces;
using WorkOrder.Online.Data.Models;

namespace WorkOrder.Online.Data
{
    public class CustomerFactory : ICustomerFactory
    {
        private readonly IConfiguration _configuration;

        public CustomerFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }
               
        public async Task<IEnumerable<CustomerModel>> GetCustomers(int organizationId)
        {
            var sql = @"SELECT [Id]
                  ,[Name]
                  ,[Responsable]
                  ,[Address]
                  ,[City]
                  ,[State]
                  ,[PostalCode]
                  ,[Phone]
                  ,[Cellphone]
                  ,[Email]
                  ,[OrganizationId]
              FROM [dbo].[Customers]
              WHERE OrganizationId = @OrganizationId";

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var result = await connection.QueryAsync<CustomerModel>(sql,
                    new
                    {
                        OrganizationId = organizationId
                    },
                    commandType: CommandType.Text);

                return result;
            }
        }

        public async Task<int> Create(CustomerModel model)
        {
            try
            {
                var sql = @"INSERT INTO [dbo].[Customers]
                           ([Name]
                            ,[Responsable]
                            ,[Address]
                            ,[City]
                            ,[State]
                            ,[PostalCode]
                            ,[Phone]
                            ,[Cellphone]
                            ,[Email]
                            ,[OrganizationId])
                        OUTPUT INSERTED.Id
                        VALUES
                           (@Name
                           ,@Responsable
                           ,@Address
                           ,@City
                           ,@State
                           ,@PostalCode
                           ,@Phone
                           ,@Cellphone
                           ,@Email
                           ,@OrganizationId)";

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    var result = await connection.QuerySingleOrDefaultAsync<int>(sql,
                        new
                        {
                            Name = model.Name,
                            Responsable = model.Responsable,
                            Address = model.Address,
                            City = model.City,
                            State = model.State,
                            PostalCode = model.PostalCode,
                            Phone = model.Phone,
                            Cellphone = model.Cellphone,
                            Email = model.Email,
                            OrganizationId = model.OrganizationId
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

        public async Task<int> Update(CustomerModel model)
        {
            try
            {
                var sql = @"UPDATE [dbo].[Customers]
                           SET [Name] = @Name
                           ,[Responsable] = @Responsable
                           ,[Address] = @Address
                           ,[City] = @City
                           ,[State] = @State
                           ,[PostalCode] = @PostalCode
                           ,[Phone] = @Phone
                           ,[Cellphone] = @Cellphone
                           ,[Email] = @Email
                           WHERE Id = @Id";

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    var result = await connection.ExecuteScalarAsync<int>(sql,
                        new
                        {
                            Id = model.Id,
                            Name = model.Name,
                            Responsable = model.Responsable,
                            Address = model.Address,
                            City = model.City,
                            State = model.State,
                            PostalCode = model.PostalCode,
                            Phone = model.Phone,
                            Cellphone = model.Cellphone,
                            Email = model.Email
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

        public async Task<int> Delete(int customerId)
        {
            var sql = @"DELETE FROM [dbo].[Customers]
                        WHERE Id = @Id";

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var result = await connection.ExecuteAsync(sql,
                    new
                    {
                        Id = customerId
                    },
                    commandType: CommandType.Text);

                return result;
            }
        }
    }
}
