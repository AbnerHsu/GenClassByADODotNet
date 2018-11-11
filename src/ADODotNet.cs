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
        private readonly object lockObj = new object(); 
        private IEnumerable<ColumnInfo> columns = null;
        private DbConnection connection = null;
        public ADODotNet()
        {
            var dbSetting = ConfigurationManager.ConnectionStrings["db"];
            var factory = DbProviderFactories.GetFactory(dbSetting.ProviderName);
            connection = factory.CreateConnection() as DbConnection;
            if (connection == null) throw new NotSupportedException(dbSetting.ProviderName + " is not support this feature.");
            connection.ConnectionString = dbSetting.ConnectionString;
            connection.Open();
        }

        /// <summary>
        /// 產生cs檔案
        /// </summary>
        public void Create()
        {
            var path = ConfigurationManager.AppSettings["OutputDir"];
            if (string.IsNullOrEmpty(path)) throw new Exception("未設定AppSetting Output值(輸出目錄)");
            var ns = ConfigurationManager.AppSettings["Namespance"] ?? "GenClass";
            var tableArray = GetTables().ToArray();
            Parallel.For(0, tableArray.Length, (index) =>
            {
                var fields = GetFields(tableArray[index]);
                if (fields.Any())
                {
                    Utility.WriteFile(path, ns, tableArray[index], fields);
                }
            });
        }

        public void Dispose()
        {
            if (connection != null && connection.State == ConnectionState.Open) connection.Dispose();
        }

        private IEnumerable<string> GetTables()
        {
            return connection.GetSchema("Tables").Rows.Cast<DataRow>().Select(row => (string)row["TABLE_NAME"]);
        }

        private Dictionary<string, string> GetFields(string table)
        {
            if (columns == null)
            {
                lock(lockObj)
                {
                    if(columns == null)
                    {
                        columns = connection.GetSchema("Columns").Rows.Cast<DataRow>().Select(row => new ColumnInfo() { Table = (string)row["TABLE_NAME"], Field = (string)row["COLUMN_NAME"], DataType = (string)row["DATA_TYPE"] });
                    }
                }
            }
            return columns.Where(c => table.Equals(c.Table)).ToDictionary(n => n.Field, n => n.DataType);
        }

        private class ColumnInfo
        {
            public string Table { get; set; }
            public string Field { get; set; }
            public string DataType { get; set; }
        }
    }
}
