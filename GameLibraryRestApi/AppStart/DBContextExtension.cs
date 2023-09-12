using GameLibraryRestApi.Data.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace GameLibraryRestApi.AppStart
{
    public static class DBContextExtension
    {
        public static void AddCustomSqlContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<GameLibraryContext>(options =>
            {
                options.UseSqlServer(connectionString);
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });
            services.BuildServiceProvider().GetService<GameLibraryContext>().Database.Migrate();
        }
    }
}
