using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using DTO;

namespace DAL
{
    public class SqlServerConnection : ISqlConnection
    {
        private LoggerDebug logger = new LoggerDebug("SqlServerConnection");
        private string sqlConnection;
        public SqlServerConnection(string sqlConnectString)
        {
            this.sqlConnection = sqlConnectString;
        }
        public async Task<IDbConnection> GetConnection()
        {
            try
            {
                var connection = new SqlConnection(this.sqlConnection);
                await connection.OpenAsync();
                return connection;
            }
            catch(Exception ex)
            {
                this.logger.Create("GetConnection : " + ex.Message,LogLevel.Error);
            }
            return null;
        }
    }
}
