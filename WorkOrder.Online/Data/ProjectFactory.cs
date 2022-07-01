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

        public async Task<int> Create(ProjectModel model)
        {
            try
            {
                var sql = @"INSERT INTO [dbo].[Projects]
                           ([ProjectNo]
                           ,[Description]
                           ,[OrganizationId])
                        OUTPUT INSERTED.Id
                        VALUES
                           (@ProjectNo
                           ,@Description
                           ,@OrganizationId)";

                var sqlStartSequence = @"IF NOT EXISTS (SELECT * FROM [dbo].[ProjectSequences] WHERE [OrganizationId] = @Id)
                            INSERT INTO [dbo].[ProjectSequences]
                             (Sequence, OrganizationId)
                             VALUES(@Sequence, @Id)
                        ELSE
                            UPDATE [dbo].[ProjectSequences]
                              SET [Sequence] = @Sequence
                              WHERE [OrganizationId] = @Id";

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {
                        var result = await connection.QuerySingleOrDefaultAsync<int>(sql,
                        new
                        {
                            ProjectNo = model.ProjectNo,
                            Description = model.Description,
                            OrganizationId = model.OrganizationId
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

        public async Task<int> Update(ProjectModel model)
        {
            try
            {
                var sql = @"UPDATE [dbo].[Projects]
                           SET [ProjectNo] = @ProjectNo
                           ,[Description] = @Description
                           WHERE Id = @Id";

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    var result = await connection.ExecuteScalarAsync<int>(sql,
                        new
                        {
                            Id = model.Id,
                            ProjectNo = model.ProjectNo,
                            Description = model.Description
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

        public async Task<int> Delete(int projectId)
        {
            var sql = @"DELETE FROM [dbo].[Projects]
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
