using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using WorkOrder.Online.Data.Interfaces;
using WorkOrder.Online.Data.Models;

namespace WorkOrder.Online.Data
{
    public class TaskFactory : ITaskFactory
    {
        private readonly IConfiguration _configuration;

        public TaskFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }
               
        public async Task<IEnumerable<TaskModel>> GetTasks(int organizationId)
        {
            var sql = @"SELECT [Id]
                  ,[Code]
                  ,[Description_Fr]
                  ,[Description_En]
                  ,[Price]
                  ,[IsFlatRate]
                  ,[OrganizationId]
              FROM [dbo].[Tasks]
              WHERE OrganizationId = @OrganizationId";

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var result = await connection.QueryAsync<TaskModel>(sql,
                             new
                             {
                                 OrganizationId = organizationId
                             },
                             commandType: CommandType.Text);

                return result;
            }
        }

        public async Task<int> Create(TaskModel model)
        {
            try
            {
                var sql = @"INSERT INTO [dbo].[Tasks]
                           ([Code]
                           ,[Description_Fr]
                           ,[Description_En]
                           ,[Price]
                           ,[IsFlatRate]
                           ,[OrganizationId])
                        OUTPUT INSERTED.Id
                        VALUES
                           (@Code
                           ,@Description_Fr
                           ,@Description_En
                           ,@Price
                           ,@IsFlatRate
                           ,@OrganizationId)";

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    var result = await connection.QuerySingleOrDefaultAsync<int>(sql,
                        new
                        {
                            Code = model.Code,
                            Description_Fr = model.Description_Fr,
                            Description_En = model.Description_En,
                            Price = model.Price,
                            IsFlatRate = model.IsFlatRate,
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

        public async Task<int> Update(TaskModel model)
        {
            try
            {
                var sql = @"UPDATE [dbo].[Tasks]
                           SET [Code] = @Code
                           ,[Description_Fr] = @Description_Fr
                           ,[Description_En] = @Description_En
                           ,[Price] = @Price
                           ,[IsFlatRate] = @IsFlatRate
                           WHERE Id = @Id";

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    var result = await connection.ExecuteScalarAsync<int>(sql,
                        new
                        {
                            Id = model.Id,
                            Code = model.Code,
                            Description_Fr = model.Description_Fr,
                            Description_En = model.Description_En,
                            Price = model.Price,
                            IsFlatRate = model.IsFlatRate
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

        public async Task<int> Delete(int taskId)
        {
            var sql = @"DELETE FROM [dbo].[Tasks]
                        WHERE Id = @Id";

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var result = await connection.ExecuteAsync(sql,
                    new
                    {
                        Id = taskId
                    },
                    commandType: CommandType.Text);

                return result;
            }
        }

        //public async Task<OrganizationModel> GetOrganization(int organizationId)
        //{
        //    var sql = @"SELECT 
        //                   [Id]
        //                  ,[Name]
        //                  ,[Address]
        //                  ,[City]
        //                  ,[Province]
        //                  ,[PostalCode]
        //                  ,[Phone]
        //                  ,[Email]
        //                  ,[Language]
        //                  ,[NbrUsers]
        //                  ,[Notes]
        //                  ,[CreationDate]
        //                  ,[IsActive]
        //                FROM [dbo].[Organizations]
        //                WHERE ID = @Id";

        //    using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
        //    {
        //        connection.Open();

        //        var result = await connection.QueryFirstOrDefaultAsync<OrganizationModel>(sql,
        //                     new
        //                     {
        //                         Id = organizationId
        //                     },
        //                     commandType: CommandType.Text);

        //        return result;
        //    }
        //}
    }
}
