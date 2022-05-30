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
    }
}
