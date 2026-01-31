using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DTO;

namespace DAL
{
    public class SqlLiteLotStatus:BaseRepositorySQL<LotStatus>,ILotStatusRepository
    {
        private LoggerDebug logger = new LoggerDebug("SqlLiteLotStatus");
        private ISqlConnection sqlConnection;
        public SqlLiteLotStatus(ISqlConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }
        public override async Task<bool> Delete(int id)
        {
            try
            {
                using (var connection = await this.sqlConnection.GetConnection())
                {
                    var result = await connection.ExecuteAsync("Delete LotStatus where Id=@Id", new { Id = id });
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                this.logger.Create("Delete error : " + ex.Message, LogLevel.Error);
            }
            return false;
        }
        public override async Task<IEnumerable<LotStatus>> GetAll()
        {
            try
            {
                using (var connection = await this.sqlConnection.GetConnection())
                {
                    return await connection.QueryAsync<LotStatus>("Select * From LotStatus ORDER BY TimeUpdate DESC");
                }
            }
            catch (Exception ex)
            {
                this.logger.Create("GetAll error : " + ex.Message, LogLevel.Error);
            }
            return null;
        }
        public override async Task<LotStatus> GetById(int id)
        {
            try
            {
                using (var connection = await this.sqlConnection.GetConnection())
                {
                    return await connection.QueryFirstOrDefaultAsync<LotStatus>("Select * From LotStatus where Id=@Id", new { Id = id });
                }
            }
            catch (Exception ex)
            {
                this.logger.Create("GetById error : " + ex.Message, LogLevel.Error);
            }
            return null;
        }
        public override async Task<bool> Insert(LotStatus entity)
        {
            try
            {
                using (var connection = await this.sqlConnection.GetConnection())
                {
                    var result = await connection.ExecuteScalarAsync<int>("Insert Into LotStatus (LotId,TimeUpdate,InputCount,TotalCount,OKCount,NGCount,EMCount,TotalTime) Values (@LotId,@TimeUpdate,@InputCount,@TotalCount,@OKCount,@NGCount,@EMCount,@TotalTime)", entity);
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                this.logger.Create("Insert error : " + ex.Message, LogLevel.Error);
            }
            return false;
        }
        public override async Task<bool> Update(LotStatus entity)
        {
            try
            {
                using (var connection = await this.sqlConnection.GetConnection())
                {
                    var result = await connection.ExecuteScalarAsync<int>("Update LotStatus Set LotId=@LotId,TimeUpdate=@TimeUpdate,InputCount=@InputCount,TotalCount=@TotalCount,OKCount=@OKCount,NGCount=@NGCount,EMCount=@EMCount,TotalTime=@TotalTime Where Id='" + entity.Id + "'", entity);
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                this.logger.Create("Update error : " + ex.Message, LogLevel.Error);
            }
            return false;
        }
        public async Task<IEnumerable<LotStatus>> GetLotStatusByTime(DateTime from, DateTime to)
        {
            try
            {
                string timeFrom = from.ToString("yyyy-MM-dd HH:mm:ss");
                string timeTo = to.ToString("yyyy-MM-dd HH:mm:ss");
                using (var connection = await this.sqlConnection.GetConnection())
                {
                    return await connection.QueryAsync<LotStatus>("SELECT * FROM LotStatus WHERE TimeUpdate >= '"+ timeFrom + "' AND TimeUpdate <= '" + timeTo + "' ORDER BY TimeUpdate DESC");
                }
            }
            catch (Exception ex)
            {
                this.logger.Create("GetLotStatusByTime error : " + ex.Message, LogLevel.Error);
            }
            return null;
        }

        public async Task<LotStatus> GetMostRecent()
        {
            try
            {
                using (var connection = await this.sqlConnection.GetConnection())
                {
                    return await connection.QueryFirstOrDefaultAsync<LotStatus>("Select * From LotStatus ORDER BY TimeUpdate DESC LIMIT 1");
                }
            }
            catch (Exception ex)
            {
                this.logger.Create("GetMostRecent error : " + ex.Message, LogLevel.Error);
            }
            return null;
        }

        public async Task<LotStatus> GetLotStatusByLotId(string lotId)
        {
            try
            {
                using (var connection = await this.sqlConnection.GetConnection())
                {
                    return await connection.QueryFirstOrDefaultAsync<LotStatus>("Select * From LotStatus where LotId=@LotId", new { LotId = lotId });
                }
            }
            catch (Exception ex)
            {
                this.logger.Create("GetLotStatusByLotId error : " + ex.Message, LogLevel.Error);
            }
            return null;
        }
    }
}
