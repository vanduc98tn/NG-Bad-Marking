using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DTO;

namespace DAL
{
    public class SqlLiteUserLogin : BaseRepositorySQL<UserLogin>, IUserLoginRepository
    {
        private LoggerDebug logger = new LoggerDebug("SqlLiteUserLogin");
        private ISqlConnection sqlConnection;
        public SqlLiteUserLogin(ISqlConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }

        public async Task<bool> ChangePassword(string userName, string newPassword)
        {
            try
            {
                using (var connection = await this.sqlConnection.GetConnection())
                {
                    var newUser = await connection.QueryFirstOrDefaultAsync<UserLogin>("SELECT Id, UserName, Password, Access FROM UserLogin WHERE UserName = @Username", new { Username = userName });
                    if (newUser == null) return false;
                    newUser.UserName = userName;
                    newUser.Password = newPassword;
                    return await this.Update(newUser);
                }
            }
            catch(Exception ex)
            {
                logger.Create("ChangePassword error : " + ex.Message, LogLevel.Error);
            }
            return false;
        }

        public async Task<int> GetAccess(string userName)
        {
            try
            {
                using (var connection = await this.sqlConnection.GetConnection())
                {
                    var result = await connection.QueryFirstOrDefaultAsync<UserLogin>("SELECT Id, UserName, Password, Access FROM UserLogin WHERE UserName = @Username", new { Username = userName });
                    return result.Access;
                }
            }
            catch (Exception ex)
            {
                logger.Create("CheckAccess error : " + ex.Message, LogLevel.Error);
            }
            return -1;
        }

        public async Task<bool> CheckPassword(string userName, string password)
        {
            try
            {
                using(var connection = await this.sqlConnection.GetConnection())
                {
                    var result = await connection.QueryFirstOrDefaultAsync<UserLogin>("SELECT Id, UserName, Password, Access FROM UserLogin WHERE UserName = @Username", new { Username = userName });
                    if (result == null) return false;
                    if (result.Password == password) return true;
                }
            }
            catch (Exception ex)
            {
                logger.Create("CheckPassword error : " + ex.Message, LogLevel.Error);
            }
            return false;
        }

        public override async Task<bool> Delete(int id)
        {
            try
            {
                using (var connection = await this.sqlConnection.GetConnection())
                {
                    var result = await connection.ExecuteAsync("Delete UserLogin where Id=@Id", new { Id = id });
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                this.logger.Create("Delete error : " + ex.Message, LogLevel.Error);
            }
            return false;
        }
        public override async Task<IEnumerable<UserLogin>> GetAll()
        {
            try
            {
                using (var connection = await this.sqlConnection.GetConnection())
                {
                    return await connection.QueryAsync<UserLogin>("Select * From UserLogin");
                }
            }
            catch (Exception ex)
            {
                this.logger.Create("GetAll error : " + ex.Message, LogLevel.Error);
            }
            return null;
        }
        public override async Task<UserLogin> GetById(int id)
        {
            try
            {
                using (var connection = await this.sqlConnection.GetConnection())
                {
                    return await connection.QueryFirstOrDefaultAsync<UserLogin>("Select * From UserLogin where Id=@Id", new { Id = id });
                }
            }
            catch (Exception ex)
            {
                this.logger.Create("GetById error : " + ex.Message, LogLevel.Error);
            }
            return null;
        }
        public override async Task<bool> Insert(UserLogin entity)
        {
            try
            {
                using (var connection = await this.sqlConnection.GetConnection())
                {
                    var result = await connection.ExecuteScalarAsync<int>("Insert Into UserLogin (UserName,Password,Access) Values (@UserName,@Password,@Access)", entity);
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                this.logger.Create("Insert error : " + ex.Message, LogLevel.Error);
            }
            return false;
        }
        public override async Task<bool> Update(UserLogin entity)
        {
            try
            {
                using (var connection = await this.sqlConnection.GetConnection())
                {
                    //var result = await connection.ExecuteScalarAsync<int>("Update UserLogin Set UserName=@UserName,Password=@Password,Access=@Access Where Id='" + entity.Id + "'", entity);
                    //return result > 0;
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "Update UserLogin Set UserName=@UserName,Password=@Password,Access=@Access Where Id=@Id";
                        command.Parameters.Add(new SQLiteParameter("@UserName", entity.UserName));
                        command.Parameters.Add(new SQLiteParameter("@Password", entity.Password));
                        command.Parameters.Add(new SQLiteParameter("@Access", entity.Access));
                        command.Parameters.Add(new SQLiteParameter("@Id", entity.Id));

                        var result = await ((SQLiteCommand)command).ExecuteNonQueryAsync();
                        return result > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                this.logger.Create("Update error : " + ex.Message, LogLevel.Error);
            }
            return false;
        }
    }
}
