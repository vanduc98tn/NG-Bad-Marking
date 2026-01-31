using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DTO;

namespace DAL
{
    public class SqlServerITM_AUTOCLEANING : BaseRepositorySQL<ITM_AUTOCLEANING>, IITM_AUTOCLEANING
    {
        private LoggerDebug logger = new LoggerDebug("SqlServerITM_AUTOCLEANING");
        private ISqlConnection sqlConnection;
        public SqlServerITM_AUTOCLEANING(ISqlConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }
        public async override Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Exists(string jigID)
        {
            try
            {
                using (var connection = await this.sqlConnection.GetConnection())
                {
                    var result = await connection.QueryFirstOrDefaultAsync<ITM_AUTOCLEANING>("Select JIG_ID From ITM_AUTOCLEANING where JIG_ID=@JIG_ID", new { JIG_ID = jigID });
                    if (result == null) return false;
                    if (result.JIG_ID == jigID) return true;
                }
            }
            catch (Exception ex)
            {
                this.logger.Create("Exists error : " + ex.Message, LogLevel.Error);
            }
            return false;
        }

        public async override Task<IEnumerable<ITM_AUTOCLEANING>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async override Task<ITM_AUTOCLEANING> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async override Task<bool> Insert(ITM_AUTOCLEANING entity)
        {
            try
            {
                using (var connection = await this.sqlConnection.GetConnection())
                {
                    var result = await connection.ExecuteScalarAsync<int>("Insert Into ITM_AUTOCLEANING (PLANT,EQUIPMENT_ID,JIG_ID,CLEAN_IN_TIME,CLEAN_OUT_TIME,CLEAN_RESULT,ETC1,ETC2,ETC3,ETC4,ETC5) Values (@PLANT,@EQUIPMENT_ID,@JIG_ID,@CLEAN_IN_TIME,@CLEAN_OUT_TIME,@CLEAN_RESULT,@ETC1,@ETC2,@ETC3,@ETC4,@ETC5)", entity);
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                this.logger.Create("Insert error : " + ex.Message, LogLevel.Error);
            }
            return false;
        }

        public async override Task<bool> Update(ITM_AUTOCLEANING entity)
        {
            try
            {
                using (var connection = await this.sqlConnection.GetConnection())
                {
                    var result = await connection.ExecuteScalarAsync<int>("Update ITM_AUTOCLEANING Set PLANT=@PLANT,EQUIPMENT_ID=@EQUIPMENT_ID,JIG_ID=@JIG_ID,CLEAN_IN_TIME=@CLEAN_IN_TIME,CLEAN_OUT_TIME=@CLEAN_OUT_TIME,CLEAN_RESULT=@CLEAN_RESULT,ETC1=@ETC1,ETC2=@ETC2,ETC3=@ETC3,ETC4=@ETC4,ETC5=@ETC5 Where JIG_ID='" + entity.JIG_ID + "'", entity);
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                this.logger.Create("Update error : " + ex.Message, LogLevel.Error);
            }
            return false;
        }

        public async Task<bool> UpdateInTime(string jigID, string inTime)
        {
            try
            {
                using (var connection = await this.sqlConnection.GetConnection())
                {
                    var result = await connection.ExecuteScalarAsync<int>("Update ITM_AUTOCLEANING Set CLEAN_IN_TIME='"+ inTime + "' Where JIG_ID='" + jigID + "'");
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                this.logger.Create("UpdateInTime error : " + ex.Message, LogLevel.Error);
            }
            return false;
        }

        public async Task<bool> UpdateOutTime(string jigID, string outTime)
        {
            try
            {
                using (var connection = await this.sqlConnection.GetConnection())
                {
                    var ITM_AUTOCLEANING = await connection.QueryFirstOrDefaultAsync<ITM_AUTOCLEANING>("Select JIG_ID From ITM_AUTOCLEANING where JIG_ID=@JIG_ID", new { JIG_ID = jigID });
                    if (ITM_AUTOCLEANING == null) return false;
                    if(String.IsNullOrEmpty(ITM_AUTOCLEANING.CLEAN_IN_TIME))
                    {
                        var result = await connection.ExecuteScalarAsync<int>("Update ITM_AUTOCLEANING Set CLEAN_OUT_TIME='" + outTime + "', CLEAN_RESULT=NG Where JIG_ID='" + jigID + "'");
                        return result > 0;
                    }
                    var result1 = await connection.ExecuteScalarAsync<int>("Update ITM_AUTOCLEANING Set CLEAN_OUT_TIME='" + outTime + "', CLEAN_RESULT=OK Where JIG_ID='" + jigID + "'");
                    return result1 > 0;
                }
            }
            catch (Exception ex)
            {
                this.logger.Create("UpdateOutTime error : " + ex.Message, LogLevel.Error);
            }
            return false;
        }
    }
}
