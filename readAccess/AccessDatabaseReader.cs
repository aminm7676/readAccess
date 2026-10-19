using readAccess;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

public static class AccessReader<T> where T : new()
{
    private static readonly Dictionary<string, PropertyInfo> _properties;

    static AccessReader()
    {
        _properties = typeof(T)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.CanWrite)
            .Select(p => new
            {
                Property = p,
                Attribute = p.GetCustomAttribute<ColumnNameAttribute>()
            })
            .Where(x => x.Attribute != null)
            .ToDictionary(
                x => x.Attribute!.Name,
                x => x.Property,
                StringComparer.OrdinalIgnoreCase
            );
    }

    public static async Task<List<T>> ReadFromAccessAsync(
        string tempPath,
        string? tableName = null)
    {
        tableName ??= typeof(T).Name;

        string driver = RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
            ? "MDBTools"
            : "Microsoft Access Driver (*.mdb, *.accdb)";

        string connectionString =
            $"Driver={{{driver}}};DBQ={tempPath};";

        // Escape کردن ] برای نام جدول در SQL
        string escapedTableName = tableName.Replace("]", "]]");
        string query = $"SELECT * FROM [{escapedTableName}]";

        var results = new List<T>();

        using var conn = new OdbcConnection(connectionString);
        await conn.OpenAsync();

        using var cmd = new OdbcCommand(query, conn);
        using var reader = (OdbcDataReader)await cmd.ExecuteReaderAsync();

        // نگاشت بر اساس نام ستون در هر دو سیستم‌عامل
        while (await reader.ReadAsync())
        {
            results.Add(MapFromReaderByName(reader));
        }

        return results;
    }

    private static T MapFromReaderByName(OdbcDataReader reader)
    {
        var item = new T();

        for (int i = 0; i < reader.FieldCount; i++)
        {
            string columnName = reader.GetName(i);

            if (!_properties.TryGetValue(columnName, out var prop))
                continue;

            object value = reader.GetValue(i);

            try
            {
                SetPropertyValue(item, prop, value);
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"خطا در نگاشت ستون '{columnName}' به پراپرتی " +
                    $"'{prop.Name}': {ex.Message}");
            }
        }

        return item;
    }

    private static void SetPropertyValue(
        T item,
        PropertyInfo prop,
        object value)
    {
        // مقدار NULL دیتابیس
        if (value == DBNull.Value)
        {
            if (!prop.PropertyType.IsValueType ||
                Nullable.GetUnderlyingType(prop.PropertyType) != null)
            {
                prop.SetValue(item, null);
            }

            return;
        }

        Type targetType =
            Nullable.GetUnderlyingType(prop.PropertyType)
            ?? prop.PropertyType;

        object convertedValue;

        if (targetType == typeof(string))
        {
            convertedValue = Convert.ToString(
                value, CultureInfo.InvariantCulture) ?? string.Empty;
        }
        else if (targetType == typeof(bool))
        {
            convertedValue = Convert.ToBoolean(
                value, CultureInfo.InvariantCulture);
        }
        else if (targetType == typeof(DateTime))
        {
            convertedValue = Convert.ToDateTime(
                value, CultureInfo.InvariantCulture);
        }
        else if (targetType.IsEnum)
        {
            convertedValue = Enum.ToObject(targetType, value);
        }
        else
        {
            convertedValue = Convert.ChangeType(
                value,
                targetType,
                CultureInfo.InvariantCulture);
        }

        prop.SetValue(item, convertedValue);
    }
} 
     
 