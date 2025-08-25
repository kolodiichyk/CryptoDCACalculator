using Microsoft.EntityFrameworkCore;

namespace CryptoDCACalculator.EF;

public static class Extensions
{
    public static MauiAppBuilder UseEF(this MauiAppBuilder builder, string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            throw new ArgumentException("File name cannot be null or empty.", nameof(fileName));
        }

        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite($"Filename={GetPath(fileName)}"));

        builder.Services.AddDbContextFactory<AppDbContext>(options =>
            options.UseSqlite($"Filename={GetPath(fileName)}"));

        return builder;
    }

    private static string GetPath(string dbName)
    {
        var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        var fileName = Path.Combine(folderPath, dbName);
        if (!File.Exists(fileName))
            File.Create(fileName);

        return fileName;
    }

    public static async Task InitializeDatabase(this Application app)
    {
        var dbContext = app.Handler?.MauiContext?.Services.GetService<AppDbContext>();
        if (dbContext != null)
        {
            await dbContext.Database.EnsureCreatedAsync();
            await dbContext.Database.MigrateAsync();
        }
    }
}
