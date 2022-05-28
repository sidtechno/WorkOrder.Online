using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using WorkOrder.Online.Data.Interfaces;
using WorkOrder.Online.Data.Models;

namespace WorkOrder.Online.Data
{
    public class UserFactory : IUserFactory
    {
        private readonly IConfiguration _configuration;

        public UserFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public async Task<IEnumerable<UserFacadeModel>> GetUserFacade(int organizationId = 0)
        {
            try
            {
                var sql = string.Empty;

                if (organizationId != 0)
                {
                    sql = @"SELECT U.Id,
                        (SELECT ClaimValue FROM AspNetUserClaims WHERE UserId = U.Id and ClaimType = 'FirstName') AS FirstName,
                        (SELECT ClaimValue FROM AspNetUserClaims WHERE UserId = U.Id and ClaimType = 'LastName') AS LastName,
                        (SELECT ClaimValue FROM AspNetUserClaims WHERE UserId = U.Id and ClaimType = 'StartUpScreenId') AS StartUpScreenId,
                        U.Email,
                        U.UserName,
                        C.ClaimValue AS OrganizationId,
                        O.Name AS OrganizationName,
                        (SELECT STRING_AGG(AR.Name, ', ') FROM [dbo].[AspNetUserRoles] R INNER JOIN [dbo].[AspNetRoles] AR ON R.RoleId = AR.Id WHERE UserId = U.Id) AS Roles,
                        CASE WHEN U.LockoutEnd IS NULL THEN 0 ELSE 1 END AS LockedOut
                        FROM AspNetUsers U
                        INNER JOIN AspNetUserClaims C
	                        ON U.Id = C.UserId
                        LEFT JOIN Organizations O
                            ON O.Id = C.ClaimValue
                        WHERE C.ClaimType = 'OrganizationId'
                        AND C.ClaimValue = @OrganizationId";
                }
                else
                {
                    sql = @"SELECT U.Id,
                        (SELECT ClaimValue FROM AspNetUserClaims WHERE UserId = U.Id and ClaimType = 'FirstName') AS FirstName,
                        (SELECT ClaimValue FROM AspNetUserClaims WHERE UserId = U.Id and ClaimType = 'LastName') AS LastName,
                        (SELECT ClaimValue FROM AspNetUserClaims WHERE UserId = U.Id and ClaimType = 'StartUpScreenId') AS StartUpScreenId,
                        U.Email,
                        U.UserName,
                        C.ClaimValue AS OrganizationId,
                        O.Name AS OrganizationName,
                        (SELECT STRING_AGG(AR.Name, ', ') FROM [dbo].[AspNetUserRoles] R INNER JOIN [dbo].[AspNetRoles] AR ON R.RoleId = AR.Id WHERE UserId = U.Id) AS Roles,
                        CASE WHEN U.LockoutEnd IS NULL THEN 0 ELSE 1 END AS LockedOut
                        FROM AspNetUsers U
                        INNER JOIN AspNetUserClaims C
	                        ON U.Id = C.UserId
                        LEFT JOIN Organizations O
                            ON O.Id = C.ClaimValue
                        WHERE C.ClaimType = 'OrganizationId'";
                }

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    var result = await connection.QueryAsync<UserFacadeModel>(sql,
                        new
                        {
                            OrganizationId = organizationId
                        },
                        commandType: CommandType.Text);

                    return result;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
