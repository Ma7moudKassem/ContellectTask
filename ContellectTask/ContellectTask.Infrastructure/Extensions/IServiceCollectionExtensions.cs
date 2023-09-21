namespace ContellectTask.Infrastructure;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApplicationDbContext(configuration);

        services.AddScoped<IContactRepository, ContactRepository>();

        return services;
    }

    public static IServiceCollection AddApplicationDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString("SqlConnection");

        ArgumentException.ThrowIfNullOrEmpty(nameof(connectionString));

        services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString,
        e => e.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName)));

        return services;
    }
}