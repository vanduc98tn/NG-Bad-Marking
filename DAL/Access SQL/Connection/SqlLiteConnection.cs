using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;
using DTO;

namespace DAL
{
    public class SqlLiteConnection : ISqlConnection
    {
        private LoggerDebug logger = new LoggerDebug("SqlLiteConnection");
        private string sqlConnection;
        public SqlLiteConnection(string sqlConnectString)
        {
            this.sqlConnection = sqlConnectString;
        }
        public async Task<IDbConnection> GetConnection()
        {
            try
            {
                var connection = new SQLiteConnection(this.sqlConnection);
                await connection.OpenAsync();
                connection.DefaultTimeout = 10;
                return connection;
            }
            catch(Exception ex)
            {
                this.logger.Create("GetConnection : "+ex.Message,LogLevel.Error);
            }
            return null;
        }
    }
}
