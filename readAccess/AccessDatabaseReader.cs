using System.Data.OleDb;

namespace readAccess
{
    public static class AccessDatabaseReader
    {
        public static async Task<List<T>> ReadAsAsync<T>(this IFormFile file, string tempPath, string? tableName = null)
               where T : new()
        {
            // Replace with your database file path
            string connectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={tempPath};";
            string query = $"SELECT * FROM {tableName ?? typeof(T).Name}";
            var results = new List<T>();
            try
            {
            
                var properties = typeof(T).GetProperties()
                .ToDictionary(p => p.Name, p => p, StringComparer.OrdinalIgnoreCase);

                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    connection.Open();
                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        using (OleDbDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Access data by column name or index
                                Console.WriteLine($"Column 1: {reader[0]}, Column 2: {reader["id"]}");

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
