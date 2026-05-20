using System.Data.Odbc;
using System.Data.OleDb;
using System.Runtime.InteropServices;

namespace readAccess
{
    public static class AccessDatabaseReader
    {
        public static async Task<List<T>> ReadAsAsync<T>(this IFormFile file, string tempPath, string? tableName = null)
               where T : new()
        {
            // Replace with your database file path

            var odbcDriver = RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
                ? "MDBTools"   // Linux driver name
                : "Microsoft Access Driver (*.mdb, *.accdb)";  // Windows driver


            string connectionString = $"Driver={{{odbcDriver}}};DBQ={tempPath};";
            string query = $"SELECT * FROM {tableName ?? typeof(T).Name}";
            var results = new List<T>();
            try
            {
            
                var properties = typeof(T).GetProperties()
                .ToDictionary(p => p.Name, p => p, StringComparer.OrdinalIgnoreCase);


                    using (var conn = new OdbcConnection(connectionString))
                    {
                        conn.Open();
                        using (var cmd = new OdbcCommand(query, conn))
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var item = new T();
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    var columnName = reader.GetName(i);
                                    if (properties.TryGetValue(columnName, out var prop))
                                    {
                                        var value = reader.GetValue(i);
                                        if (value != DBNull.Value)
                                        {
                                            prop.SetValue(item, Convert.ChangeType(value, prop.PropertyType));
                                        }
                                    }
                                }
                                results.Add(item);
                            }
                        }
                    }

                return results;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return results;
            }
        }
    }
}
