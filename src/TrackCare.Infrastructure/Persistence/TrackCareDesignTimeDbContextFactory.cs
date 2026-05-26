using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.IO;
using System.Text.Json;

namespace TrackCare.Infrastructure.Persistence;

public class TrackCareDesignTimeDbContextFactory : IDesignTimeDbContextFactory<TrackCareDbContext>
{
    public TrackCareDbContext CreateDbContext(string[] args)
    {
        // Tenta ler appsettings.json do diretório do WebApi
        // O assembly do Infrastructure está em: src/TrackCare.Infrastructure/bin/Debug/net8.0/
        // O appsettings.json está em: src/TrackCare.WebApi/
        var infraBin = Path.GetDirectoryName(typeof(TrackCareDesignTimeDbContextFactory).Assembly.Location)!;
        var webApiDir = Path.Combine(infraBin, "..", "..", "..", "..", "TrackCare.WebApi");
        var appsettingsPath = Path.GetFullPath(Path.Combine(webApiDir, "appsettings.json"));

        string connString = "postgresql://postgres:ewcldebtsrjfdkckvqin@db.ewcldebtsrjfdkckvqin.supabase.co:5432/postgres";

        if (File.Exists(appsettingsPath))
        {
            try
            {
                var json = JsonDocument.Parse(File.ReadAllText(appsettingsPath));
                var cs = json.RootElement
                    .GetProperty("ConnectionStrings")
                    .GetProperty("DefaultConnection")
                    .GetString();
                if (!string.IsNullOrEmpty(cs))
                    connString = cs;
            }
            catch { /* usa fallback */ }
        }

        var optionsBuilder = new DbContextOptionsBuilder<TrackCareDbContext>();
        optionsBuilder.UseNpgsql(connString);
        return new TrackCareDbContext(optionsBuilder.Options);
    }
}
