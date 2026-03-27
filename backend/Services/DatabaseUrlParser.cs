using Npgsql;

namespace Backend.Services;

public static class DatabaseUrlParser
{
    public static string ToNpgsqlConnectionString(string databaseUrl)
    {
        var uri = new Uri(databaseUrl);
        var userInfo = uri.UserInfo.Split(':', 2);
        if (userInfo.Length != 2)
        {
            throw new InvalidOperationException("invalid DATABASE_URL: missing user or password.");
        }

        var database = uri.AbsolutePath.TrimStart('/');

        var builder = new NpgsqlConnectionStringBuilder
        {
            Host = uri.Host,
            Port = uri.Port,
            Username = Uri.UnescapeDataString(userInfo[0]),
            Password = Uri.UnescapeDataString(userInfo[1]),
            Database = Uri.UnescapeDataString(database),
            SslMode = SslMode.Disable
        };

        return builder.ConnectionString;
    }
}
