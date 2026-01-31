using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using DTO;

namespace DAL
{
    public class SqlLiteEventLog : BaseRepositorySQL<EventLog>, IEventLogRepository
    {
        private LoggerDebug logger = new LoggerDebug("SqlLiteEventLog");
        private ISqlConnection sqlConnection;
        private SemaphoreSlim modbusSemaphore = new SemaphoreSlim(1, 1);
        public SqlLiteEventLog(ISqlConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }
        public override async Task<bool> Delete(int id)
        {
            try
            {
                using (var connection = await this.sqlConnection.GetConnection())
                {
                    var result = await connection.ExecuteAsync("Delete EventLogs where Id=@Id", new { Id = id });
                    return result >= 0;
                }
            }
            catch (Exception ex)
            {
                this.logger.Create("Delete error : " + ex.Message, LogLevel.Error);
            }
            return false;
        }
        public override async Task<IEnumerable<EventLog>> GetAll()
        {
            try
            {
                using (var connection = await this.sqlConnection.GetConnection())
                {
                    return await connection.QueryAsync<EventLog>("Select * From EventLogs ORDER BY CreatedTime DESC");
                }
            }
            catch (Exception ex)
            {
                this.logger.Create("GetAll error : " + ex.Message, LogLevel.Error);
            }
            return null;
        }
        public override async Task<EventLog> GetById(int id)
        {
            try
            {
                using (var connection = await this.sqlConnection.GetConnection())
                {
                    return await connection.QueryFirstOrDefaultAsync<EventLog>("Select * From EventLogs where Id=@Id", new { Id = id });
                }
            }
            catch (Exception ex)
            {
                this.logger.Create("GetById error : " + ex.Message, LogLevel.Error);
            }
            return null;
        }
        public override async Task<bool> Insert(EventLog entity)
        {
            await modbusSemaphore.WaitAsync();
            try
            {
                using (var connection = await this.sqlConnection.GetConnection())
                {
                    //var result = await connection.ExecuteScalarAsync<int>("Insert Into EventLogs (CreatedTime,Message,EventType) Values (@CreatedTime,@Message,@EventType)", entity);
                    //return result >= 0;
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "Insert Into EventLogs (CreatedTime, Message, EventType) Values (@CreatedTime, @Message, @EventType)";
                        command.Parameters.Add(new SQLiteParameter("@CreatedTime", entity.CreatedTime));
                        command.Parameters.Add(new SQLiteParameter("@Message", entity.Message));
                        command.Parameters.Add(new SQLiteParameter("@EventType", entity.EventType));

                        var result = await ((SQLiteCommand)command).ExecuteNonQueryAsync();
                        return result > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                this.logger.Create("Insert error : " + ex.Message, LogLevel.Error);
            }
            finally
            {
                modbusSemaphore.Release();
            }
            return false;
        }
        public override async Task<bool> Update(EventLog entity)
        {
            try
            {
                using (var connection = await this.sqlConnection.GetConnection())
                {
                    //var result = await connection.ExecuteScalarAsync<int>("Update EventLogs Set CreatedTime=@CreatedTime,Message=@Message,EventType=@EventType Where Id='" + entity.Id + "'", entity);
                    //return result > 0;
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "Update EventLogs Set CreatedTime=@CreatedTime,Message=@Message,EventType=@EventType Where Id=@Id";
                        command.Parameters.Add(new SQLiteParameter("@CreatedTime", entity.CreatedTime));
                        command.Parameters.Add(new SQLiteParameter("@Message", entity.Message));
                        command.Parameters.Add(new SQLiteParameter("@EventType", entity.EventType));
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
        public async Task<int> GetCount()
        {
            try
            {
                using (var connection = await this.sqlConnection.GetConnection())
                {
                    var result = await connection.QueryFirstOrDefaultAsync<int>("SELECT COUNT(*) FROM EventLogs");
                    return result;
                }
            }
            catch (Exception ex)
            {
                this.logger.Create("GetCount error : " + ex.Message, LogLevel.Error);
            }
            return -1;
        }
        public async Task<IEnumerable<EventLog>> GetPage(int pageIndex, int pageSize)
        {
            try
            {
                using (var connection = await this.sqlConnection.GetConnection())
                {
                    int offset = pageIndex * pageSize;
                    return await connection.QueryAsync<EventLog>("SELECT * FROM EventLogs LIMIT '" + pageIndex + "' OFFSET '" + offset + "'");
                }
            }
            catch (Exception ex)
            {
                this.logger.Create("GetPage error : " + ex.Message, LogLevel.Error);
            }
            return null;
        }
    }
}
