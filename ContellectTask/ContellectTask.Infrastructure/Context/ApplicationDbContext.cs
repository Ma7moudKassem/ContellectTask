namespace ContellectTask.Infrastructure;

public class ApplicationDbContext : IdentityDbContext
{
    public DbSet<Contact> Contacts { get; set; }
    public ApplicationDbContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }
}