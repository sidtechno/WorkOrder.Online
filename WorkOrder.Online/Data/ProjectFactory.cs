using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using WorkOrder.Online.Data.Interfaces;
using WorkOrder.Online.Data.Models;

namespace WorkOrder.Online.Data
{
    public class ProjectFactory : IProjectFactory
    {
        private readonly IConfiguration _configuration;

        public ProjectFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IEnumerable<ProjectModel>> GetProjects(int organizationId)
        {
            var sql = @"SELECT [Id]
                  ,[ProjectNo]
                  ,[Description]
                  ,[OrganizationId]
                  ,[CustomerId]
                  ,[IsDeleted]
              FROM [dbo].[Projects]
              WHERE OrganizationId = @OrganizationId";

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var result = await connection.QueryAsync<ProjectModel>(sql,
                             new
                             {
                                 OrganizationId = organizationId
                             },
                             commandType: CommandType.Text);

                return result;
            }
        }

        public async Task<IEnumerable<ProjectCategoryModel>> GetProjectCategories(int projectId)
        {
            var sql = @"SELECT PC.[Id]
                          ,PC.[ProjectId]
                          ,PC.[CategoryId]
	                      ,C.[Description_En]
	                      ,C.[Description_Fr]
                          ,PC.[Hours]
                          ,PC.[Sequence]
                      FROM [dbo].[Projects_Categories] PC
                      INNER JOIN [dbo].[Categories] C
	                    ON C.Id = PC.CategoryId
                      WHERE ProjectId = @ProjectId
                      ORDER BY Sequence";

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var result = await connection.QueryAsync<ProjectCategoryModel>(sql,
                             new
                             {
                                 ProjectId = projectId
                             },
                             commandType: CommandType.Text);

                return result;
            }
        }

        public async Task<int> Create(ProjectModel model)
        {
            try
            {
                var sql = @"INSERT INTO [dbo].[Projects]
                           ([ProjectNo]
                           ,[Description]
                           ,[OrganizationId]
                           ,[CustomerId])
                        OUTPUT INSERTED.Id
                        VALUES
                           (@ProjectNo
                           ,@Description
                           ,@OrganizationId
                           ,@CustomerId)";

                var sqlStartSequence = @"IF NOT EXISTS (SELECT * FROM [dbo].[ProjectSequences] WHERE [OrganizationId] = @Id)
                            INSERT INTO [dbo].[ProjectSequences]
                             (Sequence, OrganizationId)
                             VALUES(@Sequence, @Id)
                        ELSE
                            UPDATE [dbo].[ProjectSequences]
                              SET [Sequence] = @Sequence
                              WHERE [OrganizationId] = @Id";

                var sqlProjectCategory = @"INSERT INTO [dbo].[Projects_Categories]
                            ([ProjectId]
                           ,[CategoryId]
                           ,[Hours]
                           ,[Sequence])
                     VALUES
                           (@ProjectId
                           ,@CategoryId
                           ,@Hours
                           ,@Sequence)";

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {
                        var insertedProjectId = await connection.QuerySingleOrDefaultAsync<int>(sql,
                        new
                        {
                            ProjectNo = model.ProjectNo,
                            Description = model.Description,
                            OrganizationId = model.OrganizationId,
                            CustomerId = model.CustomerId
                        },
                        commandType: CommandType.Text,
                        transaction: transaction);

                        await connection.ExecuteAsync(sqlStartSequence,
                          new
                          {
                              Sequence = model.ProjectNo,
                              Id = model.OrganizationId
                          },
                          commandType: CommandType.Text,
                          transaction: transaction);

                        //Insert Project categories
                        if (model.ProjectsCategories.Any())
                        {
                            //var productList = new List<MaintenanceProductModel>();

                            foreach (var p in model.ProjectsCategories)
                            {
                                await connection.ExecuteAsync(sqlProjectCategory,
                                new
                                {
                                    ProjectId = insertedProjectId,
                                    CategoryId = p.CategoryId,
                                    Hours = p.Hours,
                                    Sequence = p.sequence
                                },
                                commandType: CommandType.Text,
                                transaction: transaction);
                            }
                        }

                        transaction.Commit();

                        return insertedProjectId;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> Update(ProjectModel model)
        {
            try
            {
                var sql = @"UPDATE [dbo].[Projects]
                           SET [ProjectNo] = @ProjectNo
                           ,[Description] = @Description
                           ,[CustomerId] = @CustomerId
                           WHERE Id = @Id";

                var sqlDeleteProjectCategory = @"DELETE FROM [dbo].[Projects_Categories] WHERE ProjectId = @ProjectId";

                var sqlProjectCategory = @"INSERT INTO [dbo].[Projects_Categories]
                            ([ProjectId]
                           ,[CategoryId]
                           ,[Hours]
                           ,[Sequence])
                     VALUES
                           (@ProjectId
                           ,@CategoryId
                           ,@Hours
                           ,@Sequence)";

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var transaction = connection.BeginTransaction())
                    {
                        var result = await connection.ExecuteScalarAsync<int>(sql,
                        new
                        {
                            Id = model.Id,
                            ProjectNo = model.ProjectNo,
                            Description = model.Description,
                            CustomerId = model.CustomerId
                        },
                        commandType: CommandType.Text,
                         transaction: transaction);

                        //delete all related Categories relation
                        var deleteProjectCategories = await connection.ExecuteAsync(sqlDeleteProjectCategory,
                            new
                            {
                                ProjectId = model.Id
                            },
                            commandType: CommandType.Text,
                            transaction: transaction);

                        //Insert Project categories
                        if (model.ProjectsCategories.Any())
                        {
                            //var productList = new List<MaintenanceProductModel>();

                            foreach (var p in model.ProjectsCategories)
                            {
                                await connection.ExecuteAsync(sqlProjectCategory,
                                new
                                {
                                    ProjectId = model.Id,
                                    CategoryId = p.CategoryId,
                                    Hours = p.Hours,
                                    Sequence = p.sequence
                                },
                                commandType: CommandType.Text,
                                transaction: transaction);
                            }
                        }

                        transaction.Commit();

                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Soft Delete
        public async Task<int> Delete(int projectId)
        {
            var sql = @"UPDATE [dbo].[Projects]
                        SET IsDeleted = 1
                        WHERE Id = @Id";

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var result = await connection.ExecuteAsync(sql,
                    new
                    {
                        Id = projectId
                    },
                    commandType: CommandType.Text);

                return result;
            }
        }
    }
}
