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
    public class SqlLiteAlarmLog : BaseRepositorySQL<AlarmLog>, IAlarmLogRepository
    {
        private LoggerDebug logger = new LoggerDebug("SqlLiteAlarmLog");
        private ISqlConnection sqlConnection;
        public SqlLiteAlarmLog(ISqlConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }
        public override async Task<bool> Delete(int id)
        {
            try
            {
                using (var connection = await this.sqlConnection.GetConnection())
                {
                    var result = await connection.ExecuteAsync("Delete AlarmLogs where Id=@Id", new { Id = id });
                    return result > 0;
                }
            }
            catch(Exception ex)
            {
                this.logger.Create("Delete error : " + ex.Message,LogLevel.Error);
            }
            return false;
        }
        public override async Task<IEnumerable<AlarmLog>> GetAll()
        {
            try
            {
                using(var connection = await this.sqlConnection.GetConnection())
                {
                    return await connection.QueryAsync<AlarmLog>("Select * From AlarmLogs ORDER BY CreatedTime DESC");
                }    
            }
            catch(Exception ex)
            {
                this.logger.Create("GetAll error : " + ex.Message, LogLevel.Error);
            }
            return null;
        }
        public override async Task<AlarmLog> GetById(int id)
        {
            try
            {
                using(var connection = await this.sqlConnection.GetConnection())
                {
                    return await connection.QueryFirstOrDefaultAsync<AlarmLog>("Select * From AlarmLogs where Id=@Id", new { Id = id });
                }
            }
            catch(Exception ex)
            {
                this.logger.Create("GetById error : " + ex.Message, LogLevel.Error);
            }
            return null;
        }

        public async Task<IEnumerable<AlarmLog>> GetLimitAlarms(int limit)
        {
            try
            {
                using (var connection = await this.sqlConnection.GetConnection())
                {
                    var queryResult = await connection.QueryAsync<AlarmLog>("SELECT * FROM AlarmLogs ORDER BY CreatedTime DESC LIMIT @Limit", new { Limit = limit });
                    return queryResult.ToList();
                }
            }
            catch (Exception ex)
            {
                this.logger.Create("GetLimitAlarms error : " + ex.Message, LogLevel.Error);
            }
            return null;
        }

        public override async Task<bool> Insert(AlarmLog entity)
        {
            try
            {
                using (var connection = await this.sqlConnection.GetConnection())
                {
                    //var result = await connection.ExecuteScalarAsync<int>("Insert Into AlarmLogs (CreatedTime,AlarmCode,Message,Solution,Mode) Values (@CreatedTime,@AlarmCode,@Message,@Solution,@Mode)", entity);
                    //return result > 0;
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "Insert Into AlarmLogs (CreatedTime,AlarmCode,Message,Solution,Mode) Values (@CreatedTime,@AlarmCode,@Message,@Solution,@Mode)";
                        command.Parameters.Add(new SQLiteParameter("@CreatedTime", entity.CreatedTime));
                        command.Parameters.Add(new SQLiteParameter("@AlarmCode", entity.AlarmCode));
                        command.Parameters.Add(new SQLiteParameter("@Message", entity.Message));
                        command.Parameters.Add(new SQLiteParameter("@Solution", entity.Solution));
                        command.Parameters.Add(new SQLiteParameter("@Mode", entity.Mode));

                        var result = await ((SQLiteCommand)command).ExecuteNonQueryAsync();
                        return result > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                this.logger.Create("Insert error : " + ex.Message, LogLevel.Error);
            }
            return false;
        }
        public override async Task<bool> Update(AlarmLog entity)
        {
            try
            {
                using (var connection = await this.sqlConnection.GetConnection())
                {
                    //var result = await connection.ExecuteScalarAsync<int>("Update AlarmLogs Set CreatedTime=@CreatedTime,AlarmCode=@AlarmCode,Message=@Message,Solution=@Solution,Mode=@Mode Where Id='"+ entity.Id+ "'", entity);
                    //return result > 0;
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "Update AlarmLogs Set CreatedTime=@CreatedTime,AlarmCode=@AlarmCode,Message=@Message,Solution=@Solution,Mode=@Mode Where Id=@Id";
                        command.Parameters.Add(new SQLiteParameter("@CreatedTime", entity.CreatedTime));
                        command.Parameters.Add(new SQLiteParameter("@AlarmCode", entity.AlarmCode));
                        command.Parameters.Add(new SQLiteParameter("@Message", entity.Message));
                        command.Parameters.Add(new SQLiteParameter("@Solution", entity.Solution));
                        command.Parameters.Add(new SQLiteParameter("@Mode", entity.Mode));
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
