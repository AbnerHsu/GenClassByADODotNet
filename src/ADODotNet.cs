using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenClass
{
    /// <summary>
    /// 使用ADO.Net的機制實作類別檔案產生器
    /// </summary>
    public class ADODotNet : IClassFileGenerator, IDisposable
    {
        private IEnumerable<DataRow> columns = null;
        private DbConnection connection = null;
        public ADODotNet()
        {
            var dbSetting = ConfigurationManager.ConnectionStrings["db"];
            var factory = DbProviderFactories.GetFactory(dbSetting.ProviderName);
            connection = factory.CreateConnection() as DbConnection;
            if (connection == null) throw new NotSupportedException(dbSetting.ProviderName + " is not support this feture.");
            connection.ConnectionString = dbSetting.ConnectionString;
            connection.Open();
        }

        /// <summary>
        /// 產生cs檔案
        /// </summary>
        public void Create()
        {
            foreach (var table in GetTables())
            {
                var fields = GetFields(table);
                if (fields.Any())
                {
                    var path = ConfigurationManager.AppSettings["OutputDir"];
                    if (string.IsNullOrEmpty(path)) throw new Exception("未設定AppSetting Output值(輸出目錄)");
                    var ns = ConfigurationManager.AppSettings["Namespance"] ?? "GenClass";
                    Utility.WriteFile(path, ns, table, fields);
                }
            }
        }

        private IEnumerable<string> GetTables()
        {
            return connection.GetSchema("Tables").Rows.Cast<DataRow>().Select(row => (string)row["TABLE_NAME"]);
        }

        private Dictionary<string, string> GetFields(string table)
        {
            if (columns == null)
            {
                columns = connection.GetSchema("Columns").Rows.Cast<DataRow>();
            }
            return columns.Where(row => table.Equals((string)row["TABLE_NAME"])).Select(row => new { name = (string)row["COLUMN_NAME"], type = (string)row["DATA_TYPE"] }).ToDictionary(n => n.name, n => n.type);
        }

        public void Dispose()
        {
            if(connection != null && connection.State == ConnectionState.Open) connection.Dispose();
        }
    }
}
