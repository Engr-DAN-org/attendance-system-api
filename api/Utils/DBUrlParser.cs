using System;
using Npgsql;

namespace api.Utils;

public static class DBUrlParser
{
    /// <summary>
    /// Parses DATABASE_URL (from Render) into a valid PostgreSQL connection string for Npgsql.
    /// </summary>
    public static string ParseDatabaseUrl(string databaseUrl)
    {
        var uri = new Uri(databaseUrl);
        var userInfo = uri.UserInfo.Split(':');
        var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
        var sslMode = query["sslmode"] ?? "disable";
        var builder = new NpgsqlConnectionStringBuilder
        {
            Host = uri.Host,
            Port = uri.IsDefaultPort ? 5432 : uri.Port,
            Username = userInfo[0],
            Password = userInfo[1],
            Database = uri.LocalPath.TrimStart('/'),
            SslMode = Enum.TryParse<SslMode>(sslMode, true, out var mode) ? mode : SslMode.Disable // Handle invalid values safely
        };

        return builder.ToString();
    }
}
