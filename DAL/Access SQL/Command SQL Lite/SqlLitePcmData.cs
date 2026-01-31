using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DTO;

namespace DAL
{
    public class SqlLitePcmData : BaseRepositorySQL<PcmData>, IPcmRepository
    {
        private LoggerDebug logger = new LoggerDebug("SqlLitePcmData");
        private ISqlConnection sqlConnection;
        public SqlLitePcmData(ISqlConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }
        public override async Task<bool> Delete(int id)
        {
            try
            {
                using (var connection = await this.sqlConnection.GetConnection())
                {
                    var result = await connection.ExecuteAsync("Delete PcmData where Id=@Id", new { Id = id });
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                this.logger.Create("Delete error : " + ex.Message, LogLevel.Error);
            }
            return false;
        }
        public override async Task<IEnumerable<PcmData>> GetAll()
        {
            try
            {
                using (var connection = await this.sqlConnection.GetConnection())
                {
                    return await connection.QueryAsync<PcmData>("Select * From PcmData ORDER BY UpdateTime DESC");
                }
            }
            catch (Exception ex)
            {
                this.logger.Create("GetAll error : " + ex.Message, LogLevel.Error);
            }
            return null;
        }
        public override async Task<PcmData> GetById(int id)
        {
            try
            {
                using (var connection = await this.sqlConnection.GetConnection())
                {
                    return await connection.QueryFirstOrDefaultAsync<PcmData>("Select * From PcmData where Id=@Id", new { Id = id });
                }
            }
            catch (Exception ex)
            {
                this.logger.Create("GetById error : " + ex.Message, LogLevel.Error);
            }
            return null;
        }
        public override async Task<bool> Insert(PcmData entity)
        {
            try
            {
                using (var connection = await this.sqlConnection.GetConnection())
                {
                    var result = await connection.ExecuteScalarAsync<int>("Insert Into PcmData (LotId,Qr,Result,UpdateTime) Values (@LotId,@Qr,@Result,@UpdateTime)", entity);
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                this.logger.Create("Insert error : " + ex.Message, LogLevel.Error);
            }
            return false;
        }
        public override async Task<bool> Update(PcmData entity)
        {
            try
            {
                using (var connection = await this.sqlConnection.GetConnection())
                {
                    var result = await connection.ExecuteScalarAsync<int>("Update PcmData Set LotId=@LotId,Qr=@Qr,Result=@Result,UpdateTime=@UpdateTime Where Id='" + entity.Id + "'", entity);
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                this.logger.Create("Update error : " + ex.Message, LogLevel.Error);
            }
            return false;
        }
        public async Task<int> SpcCounter(int pcmResult, DateTime from, DateTime to)
        {
            try
            {
                string fromTime = from.ToString("yyyy-MM-dd HH:mm:ss");
                string toTime = to.ToString("yyyy-MM-dd HH:mm:ss");
                using (var connection = await this.sqlConnection.GetConnection())
                {
                    var result = await connection.QueryFirstOrDefaultAsync<int>("SELECT COUNT(Id) FROM PcmData WHERE Result = '"+ pcmResult + "' AND UpdateTime >= '"+ fromTime + "' AND UpdateTime <= '" + toTime + "'");
                    return result;
                }
            }
            catch (Exception ex)
            {
                this.logger.Create("SpcCounter error : " + ex.Message, LogLevel.Error);
            }
            return -1;
        }

        public async Task<int> SpcCounterByLot(string lotId, int pcmResult, DateTime from, DateTime to)
        {
            try
            {
                string fromTime = from.ToString("yyyy-MM-dd HH:mm:ss");
                string toTime = to.ToString("yyyy-MM-dd HH:mm:ss");
                using (var connection = await this.sqlConnection.GetConnection())
                {
                    var result = await connection.QueryFirstOrDefaultAsync<int>("SELECT COUNT(Id) FROM PcmData WHERE LotId = '" + lotId + "' AND Result = '" + pcmResult + "' AND UpdateTime >= '" + fromTime + "' AND UpdateTime <= '" + toTime + "'");
                    return result;
                }
            }
            catch (Exception ex)
            {
                this.logger.Create("SpcCounterByLot error : " + ex.Message,LogLevel.Error);
            }
            return -1;
        }

        public async Task<int> SpcTotal(DateTime from, DateTime to)
        {
            try
            {
                string fromTime = from.ToString("yyyy-MM-dd HH:mm:ss");
                string toTime = to.ToString("yyyy-MM-dd HH:mm:ss");
                using (var connection = await this.sqlConnection.GetConnection())
                {
                    var result = await connection.QueryFirstOrDefaultAsync<int>("SELECT COUNT(Id) FROM PcmData WHERE UpdateTime >= '" + fromTime + "' AND UpdateTime <= '" + toTime + "'");
                    return result;
                }
            }
            catch (Exception ex)
            {
                this.logger.Create("SpcTotal error : " + ex.Message, LogLevel.Error);
            }
            return -1;
        }

        public async Task<int> SpcTotalByLot(string lotId, DateTime from, DateTime to)
        {
            try
            {
                string fromTime = from.ToString("yyyy-MM-dd HH:mm:ss");
                string toTime = to.ToString("yyyy-MM-dd HH:mm:ss");
                using (var connection = await this.sqlConnection.GetConnection())
                {
                    var result = await connection.QueryFirstOrDefaultAsync<int>("SELECT COUNT(Id) FROM PcmData WHERE LotId = '" + lotId + "' AND UpdateTime >= '" + fromTime + "' AND UpdateTime <= '" + toTime + "'");
                    return result;
                }
            }
            catch (Exception ex)
            {
                this.logger.Create("SpcTotalByLot error : " + ex.Message, LogLevel.Error);
            }
            return -1;
        }
    }
}
