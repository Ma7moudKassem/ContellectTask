namespace ContellectTask.Infrastructure;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        services.AddSignalR();

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IContactRepository, ContactRepository>();

        services.AddApplicationDbContext(configuration,environment);
        services.AddJwt(configuration);

        return services;
    }

    public static IServiceCollection AddApplicationDbContext(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        string? connectionString = environment.IsDevelopment() ? 
            configuration.GetConnectionString("SqlConnection") :
            "workstation id=ContellectTaskDb.mssql.somee.com;packet size=4096;user id=modykassem_SQLLogin_1;pwd=tm9j7he2hc;data source=ContellectTaskDb.mssql.somee.com;persist security info=False;initial catalog=ContellectTaskDb;TrustServerCertificate=True";

        ArgumentException.ThrowIfNullOrEmpty(nameof(connectionString));

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(connectionString, e => e.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName));
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });

        return services;
    }

    public static IServiceCollection AddJwt(this IServiceCollection services, IConfiguration configuration)
    {

        services.Configure<JWT>(configuration.GetSection("JWT"));

        services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = configuration["JWT:Issuer"],
                ValidAudience = configuration["JWT:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"] ?? string.Empty))
            };
        });

        return services;
    }
}