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
            var odbcDriver = RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
                ? "MDBTools"
                : "Microsoft Access Driver (*.mdb, *.accdb)";

            string connectionString = $"Driver={{{odbcDriver}}};DBQ={tempPath};";
            string query = $"SELECT * FROM [{tableName ?? typeof(T).Name}]";

            var results = new List<T>();
            try
            {
                using (var conn = new OdbcConnection(connectionString))
                {
                    conn.Open();

                    using (var cmd = new OdbcCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        // Get column names and their indices
                        var columnIndices = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string columnName = reader.GetName(i);
                            columnIndices[columnName] = i;
                            Console.WriteLine($"Column {i}: '{columnName}'");
                        }

                        // Get properties of T
                        var properties = typeof(T).GetProperties()
                            .ToDictionary(p => p.Name, p => p, StringComparer.OrdinalIgnoreCase);

                        // Read data
                        while (reader.Read())
                        {
                            var item = new T();

                            foreach (var kvp in columnIndices)
                            {
                                string dbColumnName = kvp.Key;
                                int index = kvp.Value;

                                if (properties.TryGetValue(dbColumnName, out var prop))
                                {
                                    var value = reader.GetValue(index);
                                    if (value != DBNull.Value)
                                    {
                                        try
                                        {
                                            // Special handling for AutoNumber (int)
                                            if (prop.PropertyType == typeof(int))
                                            {
                                                if (value is int intValue)
                                                {
                                                    prop.SetValue(item, intValue);
                                                }
                                                else if (value is double doubleValue)
                                                {
                                                    prop.SetValue(item, Convert.ToInt32(doubleValue));
                                                }
                                                else
                                                {
                                                    prop.SetValue(item, Convert.ToInt32(value));
                                                }
                                            }
                                            else
                                            {
                                                var convertedValue = Convert.ChangeType(value, prop.PropertyType);
                                                prop.SetValue(item, convertedValue);
                                            }

                                            if (dbColumnName.Equals("id", StringComparison.OrdinalIgnoreCase))
                                            {
                                                Console.WriteLine($"Set id = {value}");
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine($"Error converting {dbColumnName}: {ex.Message}");
                                        }
                                    }
                                }
                                else
                                {
                                    // Only log once per missing property
                                    if (!_loggedMissingColumns.Contains(dbColumnName))
                                    {
                                        Console.WriteLine($"No property found for column: '{dbColumnName}'");
                                        _loggedMissingColumns.Add(dbColumnName);
                                    }
                                }
                            }
                            results.Add(item);
                        }

                        Console.WriteLine($"Total records read: {results.Count}");
                        if (results.Any())
                        {
                            Console.WriteLine($"First record id: {results.First().GetType().GetProperty("id")?.GetValue(results.First())}");
                        }
                    }
                }

                return results;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return results;
            }
        }

        public static async Task<List<CardExport>> ReadAccessFileAsync(string tempPath)
        {
            var results = new List<CardExport>();

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                // راه حل برای لینوکس: استفاده از اندیس ستون‌ها
                string connectionString = $"Driver={{MDBTools}};DBQ={tempPath};";
                string query = "SELECT * FROM CardExport";

                using (var conn = new OdbcConnection(connectionString))
                {
                    conn.Open();
                    using (var cmd = new OdbcCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var item = new CardExport
                            {
                                // بر اساس ترتیب ستون‌ها در جدول Access شما
                                id = reader.GetValue(0) != DBNull.Value ? Convert.ToInt32(reader.GetValue(0)) : 0,
                                BranchCode = reader.GetValue(1)?.ToString(),
                                AccCnt = reader.GetValue(2)?.ToString(),
                                PAN1 = reader.GetValue(3)?.ToString(),
                                PAN2 = reader.GetValue(4)?.ToString(),
                                PAN3 = reader.GetValue(5)?.ToString(),
                                PAN4 = reader.GetValue(6)?.ToString(),
                                FName = reader.GetValue(7)?.ToString(),
                                IMD = reader.GetValue(8)?.ToString(),
                                Track2 = reader.GetValue(9)?.ToString(),
                                Exp_Year = reader.GetValue(10)?.ToString(),
                                Exp_Month = reader.GetValue(11)?.ToString(),
                                CVV = reader.GetValue(12)?.ToString(),
                                CVV2 = reader.GetValue(13)?.ToString(),
                                CardType = reader.GetValue(14)?.ToString(),
                                Sahmiehcode = reader.GetValue(15)?.ToString(),
                                Customer_NTN = reader.GetValue(16)?.ToString(),
                                GiftCardText = reader.GetValue(17)?.ToString(),
                                GiftCardAmount = reader.GetValue(18)?.ToString(),
                                Sheba = reader.GetValue(19)?.ToString(),
                                ReferenceNo = reader.GetValue(20)?.ToString()
                            };
                            results.Add(item);
                        }
                    }
                }
            }
            else
            {
                // راه حل برای ویندوز: استفاده از نام ستون‌ها
                string connectionString = $"Driver={{Microsoft Access Driver (*.mdb, *.accdb)}};DBQ={tempPath};";
                string query = "SELECT * FROM CardExport";

                using (var conn = new OdbcConnection(connectionString))
                {
                    conn.Open();
                    using (var cmd = new OdbcCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        var properties = typeof(CardExport).GetProperties()
                            .ToDictionary(p => p.Name, p => p, StringComparer.OrdinalIgnoreCase);

                        while (reader.Read())
                        {
                            var item = new CardExport();
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

        private static HashSet<string> _loggedMissingColumns = new HashSet<string>();
    }
}
