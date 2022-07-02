using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using WorkOrder.Online.Data.Interfaces;
using WorkOrder.Online.Data.Models;

namespace WorkOrder.Online.Data
{
    public class CategoryFactory : ICategoryFactory
    {
        private readonly IConfiguration _configuration;

        public CategoryFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }
               
        public async Task<IEnumerable<CategoryModel>> GetCategories(int organizationId)
        {
            var sql = @"SELECT [Id]
                  ,[Description_Fr]
                  ,[Description_En]
                  ,[Cost]
                  ,[Retail]
                  ,[OrganizationId]
              FROM [dbo].[Categories]
              WHERE OrganizationId = @OrganizationId";

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var result = await connection.QueryAsync<CategoryModel>(sql,
                             new
                             {
                                 OrganizationId = organizationId
                             },
                             commandType: CommandType.Text);

                return result;
            }
        }

        public async Task<int> Create(CategoryModel model)
        {
            try
            {
                var sql = @"INSERT INTO [dbo].[Categories]
                           ([Description_Fr]
                           ,[Description_En]
                           ,[Cost]
                           ,[Retail]
                           ,[OrganizationId])
                        OUTPUT INSERTED.Id
                        VALUES
                           (@Description_Fr
                           ,@Description_En
                           ,@Cost
                           ,@Retail
                           ,@OrganizationId)";

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    var result = await connection.QuerySingleOrDefaultAsync<int>(sql,
                        new
                        {
                            Description_Fr = model.Description_Fr,
                            Description_En = model.Description_En,
                            Cost = model.Cost,
                            Retail = model.Retail,
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

        public async Task<int> Update(CategoryModel model)
        {
            try
            {
                var sql = @"UPDATE [dbo].[Categories]
                           SET [Description_Fr] = @Description_Fr
                           ,[Description_En] = @Description_En
                           ,[Cost] = @Cost
                           ,[Retail] = @Retail
                           WHERE Id = @Id";

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    var result = await connection.ExecuteScalarAsync<int>(sql,
                        new
                        {
                            Id = model.Id,
                            Description_Fr = model.Description_Fr,
                            Description_En = model.Description_En,
                            Cost = model.Cost,
                            Retail = model.Retail
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

        public async Task<int> Delete(int categoryId)
        {
            var sql = @"DELETE FROM [dbo].[Categories]
                        WHERE Id = @Id";

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var result = await connection.ExecuteAsync(sql,
                    new
                    {
                        Id = categoryId
                    },
                    commandType: CommandType.Text);

                return result;
            }
        }
    }
}
