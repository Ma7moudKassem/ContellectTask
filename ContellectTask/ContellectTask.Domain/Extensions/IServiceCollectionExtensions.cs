namespace ContellectTask.Domain;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddLocalization();

        services.AddScoped<IValidator<Contact>, ContactValidator>();
        services.AddScoped<IValidator<LogInModel>, LogInModelValidator>();

        return services;
    }
}