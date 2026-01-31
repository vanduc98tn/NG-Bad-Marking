using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DTO;

namespace DAL
{
    public class SqlServerITM_DEFLUX : BaseRepositorySQL<ITM_DEFLUX>,IITM_DEFLUX
    {
        private LoggerDebug logger = new LoggerDebug("SqlServerITM_DEFLUX");
        private ISqlConnection sqlConnection;
        public SqlServerITM_DEFLUX(ISqlConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }
        public override Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<ITM_DEFLUX>> GetAll()
        {
            throw new NotImplementedException();
        }

        public override Task<ITM_DEFLUX> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async override Task<bool> Insert(ITM_DEFLUX entity)
        {
            try
            {
                using (var connection = await this.sqlConnection.GetConnection())
                {
                    var result = await connection.ExecuteScalarAsync<int>("Insert Into ITM_DEFLUX (PLANT,TYPE,EQUIPMENT_ID,LOT_NUMBER,JIG_ID,PCB_QR1,PCB_QR2,DEFLUX_RESULT,IN_TIME,OUT_TIME,ETC1,ETC2,ETC3,ETC4,ETC5" +
                        ",CLEAN_TANK,RINSE_TANK1,RINSE_TANK2,RINSE_TANK3,DRYER,CLEANING_TOP,CLEANING_BOT,CMC_IS_TOP,CMC_IS_BOT,RINSING1_TOP,RINSING1_BOT,RINSING2_TOP,RINSING2_BOT,RINSING3_TOP,RINSING3_BOT,FINAL_SPARY_TOP,FINAL_SPARY_BOT" +
                        ",AIR_KNIFE_TOP,AIR_KNIFE_BOT,CON_MSR,CON_SPEED) Values (@PLANT,@TYPE,@EQUIPMENT_ID,@LOT_NUMBER,@JIG_ID,@PCB_QR1,@PCB_QR2,@DEFLUX_RESULT,@IN_TIME,@OUT_TIME,@ETC1,@ETC2,@ETC3,@ETC4,@ETC5,@CLEAN_TANK,@RINSE_TANK1,@RINSE_TANK2,@RINSE_TANK3,@DRYER,@CLEANING_TOP,@CLEANING_BOT,@CMC_IS_TOP,@CMC_IS_BOT,@RINSING1_TOP,@RINSING1_BOT,@RINSING2_TOP,@RINSING2_BOT,@RINSING3_TOP,@RINSING3_BOT,@FINAL_SPARY_TOP,@FINAL_SPARY_BOT)", entity);
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                this.logger.Create("Insert error : " + ex.Message, LogLevel.Error);
            }
            return false;
        }

        public override Task<bool> Update(ITM_DEFLUX entity)
        {
            throw new NotImplementedException();
        }
        public async Task<bool> Exists(string jigID)
        {
            try
            {
                using (var connection = await this.sqlConnection.GetConnection())
                {
                    var result = await connection.QueryFirstOrDefaultAsync<ITM_DEFLUX>("Select JIG_ID From ITM_DEFLUX where JIG_ID=@JIG_ID", new { JIG_ID = jigID });
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
        public async Task<bool> DeleteByJIG(string jigID)
        {
            try
            {
                using (var connection = await this.sqlConnection.GetConnection())
                {
                    var result = await connection.ExecuteAsync("Delete ITM_DEFLUX where JIG_ID=@JIG_ID", new { JIG_ID = jigID });
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                this.logger.Create("DeleteByJIG error : " + ex.Message, LogLevel.Error);
            }
            return false;
        }
        public async Task<bool> UpdateOutTime(string jigID, string outTime)
        {
            try
            {
                using (var connection = await this.sqlConnection.GetConnection())
                {
                    var ITM_DEFLUX = await connection.QueryFirstOrDefaultAsync<ITM_DEFLUX>("Select JIG_ID From ITM_DEFLUX where JIG_ID=@JIG_ID", new { JIG_ID = jigID });
                    if (ITM_DEFLUX == null) return false;
                    if (String.IsNullOrEmpty(ITM_DEFLUX.IN_TIME))
                    {
                        var result = await connection.ExecuteScalarAsync<int>("Update ITM_DEFLUX Set OUT_TIME='" + outTime + "', DEFLUX_RESULT=NG Where JIG_ID='" + jigID + "'");
                        return result > 0;
                    }
                    var result1 = await connection.ExecuteScalarAsync<int>("Update ITM_DEFLUX Set OUT_TIME='" + outTime + "', DEFLUX_RESULT=OK Where JIG_ID='" + jigID + "'");
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
