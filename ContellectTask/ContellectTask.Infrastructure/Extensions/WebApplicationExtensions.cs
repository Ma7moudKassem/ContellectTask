namespace ContellectTask.Infrastructure;

public static class WebApplicationExtensions
{
    public static WebApplication UseInfrastructureLayer(this WebApplication app)
    {
        app.DatabaseMigrate();

        app.MapHub<NotificationsHub>("/CreateContactHub");
        app.MapHub<NotificationsHub>("/UpdateContactHub");
        app.MapHub<NotificationsHub>("/DeleteContactHub");

        return app;
    }

    public static WebApplication DatabaseMigrate(this WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateAsyncScope();

        ApplicationDbContext? dataContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        dataContext.Database.Migrate();

        return app;
    }
}