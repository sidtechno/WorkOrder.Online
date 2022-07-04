﻿using Dapper;
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
                         O.[Id]
                        ,O.[Name]
                        ,O.[Address]
                        ,O.[City]
                        ,O.[Province]
                        ,O.[PostalCode]
                        ,O.[Phone]
                        ,O.[Email]
                        ,O.[Language]
                        ,O.[NbrUsers]
                        ,O.[Notes]
                        ,O.[CreationDate]
                        ,O.[IsActive]
                        ,O.[IsWoPriceHidden]
	                    ,(SELECT COUNT(*) FROM AspNetUserClaims WHERE ClaimType = 'OrganizationId' AND ClaimValue = CAST(O.Id as varchar(10))) AS NbrActiveUsers
                    FROM [dbo].[Organizations] O
                    WHERE Id > 1";

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
                           ,[IsActive]
                           ,[IsWoPriceHidden])
                     OUTPUT INSERTED.Id
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
                           ,@IsActive
                           ,@IsWoPriceHidden)";

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
                        var insertedOrg = await connection.QuerySingleAsync<int>(sql,
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
                            IsActive = model.IsActive,
                            IsWoPriceHidden = model.IsWoPriceHidden
                        },
                        commandType: CommandType.Text,
                        transaction: transaction);

                        await connection.ExecuteAsync(sqlStartSequence,
                            new
                            {
                                Sequence = model.ProjectStartSequence,
                                Id = insertedOrg
                            },
                            commandType: CommandType.Text,
                            transaction: transaction);

                        transaction.Commit();

                        return insertedOrg;
                    }
                }
            }
            catch (Exception ex)
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
                           ,[IsWoPriceHidden] = @IsWoPriceHidden
                           WHERE Id = @Id";

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
                            IsActive = model.IsActive,
                            IsWoPriceHidden = model.IsWoPriceHidden
                        },
                        commandType: CommandType.Text,
                        transaction: transaction);
                        

                        await connection.ExecuteAsync(sqlStartSequence,
                            new
                            {
                                Sequence = model.ProjectStartSequence,
                                Id = model.Id
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
                          ,[IsWoPriceHidden]
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
