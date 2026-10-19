using System.Data.Common;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Reflection;
using System.Runtime.InteropServices;

namespace readAccess
{
    public static class AccessReader<T> where T : new()
    {
        private static readonly Dictionary<string, PropertyInfo> _properties;
        private static readonly List<PropertyInfo> _orderedProperties;

        static AccessReader()
        {
            _properties = typeof(T)
                .GetProperties()
                .Select(p => new
                {
                    Property = p,
                    Attribute = p.GetCustomAttribute<ColumnNameAttribute>()
                })
                .Where(x => x.Attribute != null)
                .ToDictionary(
                    x => x.Attribute!.Name.Trim(),
                    x => x.Property,
                    StringComparer.OrdinalIgnoreCase
                );

            _orderedProperties = typeof(T)
                .GetProperties()
                .Select(p => new
                {
                    Property = p,
                    Attribute = p.GetCustomAttribute<ColumnNameAttribute>()
                })
                .Where(x => x.Attribute != null)
                .OrderBy(x => x.Attribute!.Order)
                .Select(x => x.Property)
                .ToList();
        }

        public static async Task<List<T>> ReadFromAccessAsync(string tempPath, string? tableName=null)
        {
            if (tableName is null)
            {
                tableName = nameof(T);
            }
            var results = new List<T>();

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                results = await ReadFromLinuxAsync(tempPath, tableName);
            }
            else
            {
                results = await ReadFromWindowsAsync(tempPath, tableName);
            }

            return results;
        }

        private static async Task<List<T>> ReadFromLinuxAsync(string tempPath, string tableName)
        {
            var results = new List<T>();
            string connectionString = $"Driver={{MDBTools}};DBQ={tempPath};";
            string query = $"SELECT * FROM {tableName}";

            using (var conn = new OdbcConnection(connectionString))
            {
                await conn.OpenAsync();
                using (var cmd = new OdbcCommand(query, conn))
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    int columnCount = reader.FieldCount;
                    while (await reader.ReadAsync())
                    {
                        var item = MapFromReaderByOrder((OdbcDataReader)reader, columnCount);
                        results.Add(item);
                    }
                }
            }

            return results;
        }

        private static async Task<List<T>> ReadFromWindowsAsync(string tempPath, string tableName)
        {
            var results = new List<T>();
            string connectionString = $"Driver={{Microsoft Access Driver (*.mdb, *.accdb)}};DBQ={tempPath};";
            string query = $"SELECT * FROM {tableName}";

            using (var conn = new OdbcConnection(connectionString))
            {
                await conn.OpenAsync();
                using (var cmd = new OdbcCommand(query, conn))
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var item = MapFromReaderByName((OdbcDataReader)reader);
                        results.Add(item);
                    }
                }
            }

            return results;
        }

        private static T MapFromReaderByOrder(
      OdbcDataReader reader,
      int columnCount)
        {
            var item = new T();

            for (int i = 0; i < columnCount; i++)
            {
                string columnName = reader.GetName(i).Trim();
                object value = reader.GetValue(i);

                Console.WriteLine(
                    $"Index={i} | Column=[{columnName}] | Value=[{value}]");

                if (_properties.TryGetValue(columnName, out var prop))
                {
                    SetPropertyValue(item, prop, value);
                }
                else
                {
                    Console.WriteLine($"ستون متناظر پیدا نشد: [{columnName}]");
                }
            }

            return item;
        }
        private static T MapFromReaderByName(OdbcDataReader reader)
        {
            var item = new T();

            for (int i = 0; i < reader.FieldCount; i++)
            {
                var columnName = reader.GetName(i);
                if (_properties.TryGetValue(columnName, out var prop))
                { 
                    var value = reader.GetValue(i);
                    SetPropertyValue(item, prop, value);
                }
            }

            return item;
        }

        private static void SetPropertyValue(T item, PropertyInfo prop, object value)
        {
            if (value != DBNull.Value)
            {
                try
                {
                    // تبدیل نوع خودکار
                    if (prop.PropertyType == typeof(int))
                        prop.SetValue(item, Convert.ToInt32(value));
                    else if (prop.PropertyType == typeof(int?))
                        prop.SetValue(item, value == null ? null : Convert.ToInt32(value));
                    else if (prop.PropertyType == typeof(long))
                        prop.SetValue(item, Convert.ToInt64(value));
                    else if (prop.PropertyType == typeof(long?))
                        prop.SetValue(item, value == null ? null : Convert.ToInt64(value));
                    else if (prop.PropertyType == typeof(decimal))
                        prop.SetValue(item, Convert.ToDecimal(value));
                    else if (prop.PropertyType == typeof(decimal?))
                        prop.SetValue(item, value == null ? null : Convert.ToDecimal(value));
                    else if (prop.PropertyType == typeof(DateTime))
                        prop.SetValue(item, Convert.ToDateTime(value));
                    else if (prop.PropertyType == typeof(DateTime?))
                        prop.SetValue(item, value == null ? null : Convert.ToDateTime(value));
                    else if (prop.PropertyType == typeof(bool))
                        prop.SetValue(item, Convert.ToBoolean(value));
                    else if (prop.PropertyType == typeof(bool?))
                        prop.SetValue(item, value == null ? null : Convert.ToBoolean(value));
                    else if (prop.PropertyType == typeof(string))
                        prop.SetValue(item, value.ToString());
                    else
                        prop.SetValue(item, Convert.ChangeType(value, prop.PropertyType));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"خطا در تبدیل مقدار برای {prop.Name}: {ex.Message}");
                }
            }
        }
    }
}
