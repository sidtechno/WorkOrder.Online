using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using WorkOrder.Online.Data.Interfaces;
using WorkOrder.Online.Data.Models;
using WorkOrder.Online.Models;

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
                  ,[Responsible]
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
                            ,[Responsible]
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
                           ,@Responsible
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
                            Responsible = model.Responsible,
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
                           ,[Responsible] = @Responsible
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
                            Responsible = model.Responsible,
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

        public async Task<IEnumerable<ResponsibleModel>> GetResponsibles(int customerId)
        {
            var sql = @"SELECT TOP (1000) [Id]
                  ,[Name]
                  ,[Cellphone]
                  ,[Email]
                  ,[CustomerId]
              FROM [dbo].[ResponsiblePerson]
              WHERE CustomerId = @CustomerId";

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var result = await connection.QueryAsync<ResponsibleModel>(sql,
                    new
                    {
                        CustomerId = customerId
                    },
                    commandType: CommandType.Text);

                return result;
            }
        }

        public async Task<int> CreateResponsible(ResponsibleModel model)
        {
            try
            {
                var sql = @"INSERT INTO [dbo].[ResponsiblePerson]
                           ([Name]
                            ,[Cellphone]
                            ,[Email]
                            ,[CustomerId])
                        OUTPUT INSERTED.Id
                        VALUES
                           (@Name
                           ,@Cellphone
                           ,@Email
                           ,@CustomerId)";

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    var result = await connection.QuerySingleOrDefaultAsync<int>(sql,
                        new
                        {
                            Name = model.Name,
                            Cellphone = model.Cellphone,
                            Email = model.Email,
                            CustomerId = model.CustomerId
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

        public async Task<int> UpdateResponsible(CustomerModel model)
        {
            try
            {
                var sql = @"UPDATE [dbo].[ResponsiblePerson]
                           SET [Name] = @Name
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

        public async Task<int> DeleteResponsible(int responsibleId)
        {
            var sql = @"DELETE FROM [dbo].[ResponsiblePerson]
                        WHERE Id = @Id";

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var result = await connection.ExecuteAsync(sql,
                    new
                    {
                        Id = responsibleId
                    },
                    commandType: CommandType.Text);

                return result;
            }
        }

        public async Task<int> ImportCustomers(IEnumerable<CustomerViewModel> customers)
        {
            try
            {
                var sql = @"INSERT INTO [dbo].[Customers]
                           ([Name]
                            ,[Responsible]
                            ,[Address]
                            ,[City]
                            ,[State]
                            ,[PostalCode]
                            ,[Phone]
                            ,[Cellphone]
                            ,[Email]
                            ,[OrganizationId])
                        VALUES
                           (@Name
                           ,@Responsible
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

                    var result = await connection.ExecuteAsync(sql,
                        customers,
                        commandType: CommandType.Text);

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
