using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace XXX
{
    // SqlContext 擴充方法類別
    public static class DbExtensions
    {
        /// <summary>
        /// 大量批次新增
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db"></param>
        /// <param name="data"></param>
        /// <param name="batchSize"></param>
        public static bool BulkInsert<T>(this DbContext db, List<T> data, out string msg, int batchSize = 100) where T : class
        {
            using (var conn = new SqlConnection(db.Database.Connection.ConnectionString))
            {
                conn.Open();

                using (var transaction = conn.BeginTransaction()) // 開始交易
                {
                    using (var bulkCopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, transaction)) // 交易傳給 SqlBulkCopy
                    {
                        try
                        {
                            //  取資料表名(開發原則:實體資料模型命名須與SQL資料表名稱相同,可參考 SqlDataModel 類別)
                            bulkCopy.DestinationTableName = typeof(T).Name;

                            //  尋覽 T 有的屬性 自動建立ColumnMapping
                            foreach (PropertyInfo property in typeof(T).GetProperties())
                            {
                                Type propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

                                if (propertyType.IsPrimitive ||         //  基礎資料型別
                                    propertyType == typeof(string) ||
                                    propertyType == typeof(DateTime) ||
                                    propertyType == typeof(decimal))
                                {
                                    bulkCopy.ColumnMappings.Add(property.Name, property.Name);
                                }
                            }

                            bulkCopy.BatchSize = batchSize;
                            bulkCopy.WriteToServer(data.ToDataTable()); //  寫入SQL (傳入參數:List<T> 轉的DataTable)
                            transaction.Commit();   //  確認交易
                            msg = "成功";
                            return true;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback(); //  發生錯誤時回滾交易
                            msg = $"失敗：{ex}";
                            return false;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 將List<T> 轉為 DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this List<T> items) where T : class
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in Props)
            {
                Type propertyType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                if (propertyType.IsPrimitive ||
                    propertyType == typeof(string) ||
                    propertyType == typeof(DateTime) ||
                    propertyType == typeof(decimal))
                {
                    dataTable.Columns.Add(prop.Name);
                }
            }

            foreach (T item in items)
            {
                var values = new object[dataTable.Columns.Count];
                int i = 0;
                foreach (DataColumn column in dataTable.Columns)
                {
                    values[i++] = Props.FirstOrDefault(p => p.Name == column.ColumnName)?.GetValue(item, null);
                }

                dataTable.Rows.Add(values);
            }
            return dataTable;
        }
    }
}
