using Microsoft.EntityFrameworkCore;
using NextUni.Modules.Academic.Infrastructure.Database;
using NextUni.Modules.Contents.Infrastructure.Database;
using NextUni.Modules.Events.Infrastructure.Database;
using NextUni.Modules.Users.Infrastructure.Database;

namespace NextUni.Api.Extensions;

public static class MigrationExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();
        ApplyMigration<UserDbContext>(scope);
        ApplyMigration<EventDbContext>(scope);
        ApplyMigration<ContentDbContext>(scope);
        ApplyMigration<AcademicDbContext>(scope);
    }

    private static void ApplyMigration<TDbContext>(IServiceScope scope)
        where TDbContext : DbContext
    {
        using TDbContext context = scope.ServiceProvider.GetRequiredService<TDbContext>();

        context.Database.Migrate();
    }
}
