using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using WorkOrder.Online.Data.Interfaces;
using WorkOrder.Online.Data.Models;

namespace WorkOrder.Online.Data
{
    public class ProjectSequenceFactory : IProjectSequenceFactory
    {
        private readonly IConfiguration _configuration;

        public ProjectSequenceFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }
               
        public async Task<ProjectSequenceModel> GetProjectSequence(int organizationId)
        {
            var sql = @"SELECT [Id]
                  ,[Sequence]
                  ,[OrganizationId]
              FROM [dbo].[ProjectSequences]
              WHERE OrganizationId = @OrganizationId";

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var result = await connection.QueryFirstOrDefaultAsync<ProjectSequenceModel>(sql,
                             new
                             {
                                 OrganizationId = organizationId
                             },
                             commandType: CommandType.Text);

                return result;
            }
        }

        //public async Task<int> Create(ProjectModel model)
        //{
        //    try
        //    {
        //        var sql = @"INSERT INTO [dbo].[Projects]
        //                   ([ProjectNo]
        //                   ,[Description]
        //                   ,[OrganizationId])
        //                OUTPUT INSERTED.Id
        //                VALUES
        //                   (@ProjectNo
        //                   ,@Description
        //                   ,@OrganizationId)";

        //        using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
        //        {
        //            connection.Open();

        //            var result = await connection.QuerySingleOrDefaultAsync<int>(sql,
        //                new
        //                {
        //                    ProjectNo = model.ProjectNo,
        //                    Description = model.Description,
        //                    OrganizationId = model.OrganizationId
        //                },
        //                commandType: CommandType.Text);

        //            return result;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public async Task<int> Update(ProjectModel model)
        //{
        //    try
        //    {
        //        var sql = @"UPDATE [dbo].[Projects]
        //                   SET [ProjectNo] = @ProjectNo
        //                   ,[Description] = @Description
        //                   WHERE Id = @Id";

        //        using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
        //        {
        //            connection.Open();

        //            var result = await connection.ExecuteScalarAsync<int>(sql,
        //                new
        //                {
        //                    Id = model.Id,
        //                    ProjectNo = model.ProjectNo,
        //                    Description = model.Description
        //                },
        //                commandType: CommandType.Text);

        //            return result;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public async Task<int> Delete(int projectId)
        //{
        //    var sql = @"DELETE FROM [dbo].[Projects]
        //                WHERE Id = @Id";

        //    using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
        //    {
        //        connection.Open();

        //        var result = await connection.ExecuteAsync(sql,
        //            new
        //            {
        //                Id = projectId
        //            },
        //            commandType: CommandType.Text);

        //        return result;
        //    }
        //}
    }
}
