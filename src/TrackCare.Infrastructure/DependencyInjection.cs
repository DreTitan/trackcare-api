using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TrackCare.Application.Mapping;
using TrackCare.Domain.Interfaces;
using TrackCare.Infrastructure.Persistence;
using TrackCare.Infrastructure.Repositories;
using TrackCare.Infrastructure.Services;
using TrackCare.Infrastructure.Configuration;
using Npgsql;
using AutoMapper;

namespace TrackCare.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddDbContext<TrackCareDbContext>(options =>
        {
            if (connString.StartsWith("postgresql://", StringComparison.OrdinalIgnoreCase))
            {
                var uri = new Uri(connString);
                var userInfo = uri.UserInfo.Split(':');
                var csb = new NpgsqlConnectionStringBuilder
                {
                    Host = uri.Host,
                    Port = uri.Port > 0 ? uri.Port : 5432,
                    Database = uri.AbsolutePath.TrimStart('/'),
                    Username = userInfo.Length > 0 ? userInfo[0] : "postgres",
                    Password = userInfo.Length > 1 ? userInfo[1] : "",
                    SslMode = SslMode.Require
                };
                options.UseNpgsql(csb.ToString());
            }
            else
            {
                options.UseNpgsql(connString);
            }
        });

        services.AddScoped<IRecolhimentoRepository, RecolhimentoRepository>();

        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });
        services.AddScoped<IMapper>(sp => mapperConfig.CreateMapper());
        services.AddScoped<IHistoricoStatusRepository, HistoricoStatusRepository>();
        services.AddScoped<IAnexoRepository, AnexoRepository>();
        services.AddScoped<IComentarioRepository, ComentarioRepository>();

        var supabaseUrl = configuration["Supabase:Url"];
        var supabaseKey = configuration["Supabase:Key"];
        var supabaseBucket = configuration["Supabase:Bucket"] ?? "anexos";

        if (!string.IsNullOrEmpty(supabaseUrl) && !string.IsNullOrEmpty(supabaseKey))
        {
            services.AddScoped<IFileStorageService>(_ =>
                new SupabaseStorageService(new SupabaseSettings
                {
                    Url = supabaseUrl,
                    Key = supabaseKey,
                    Bucket = supabaseBucket
                }));
        }

        return services;
    }
}
