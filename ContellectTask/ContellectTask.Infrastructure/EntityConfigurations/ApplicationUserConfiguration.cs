namespace ContellectTask.Infrastructure;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<IdentityUser>
{
    public void Configure(EntityTypeBuilder<IdentityUser> builder)
    {
        IdentityUser user1 = new() { UserName = "user1", NormalizedUserName = "user1".ToUpper(), };
        IdentityUser user2 = new() { UserName = "user2", NormalizedUserName = "user2".ToUpper(), };

        PasswordHasher<IdentityUser> password = new();

        string hashed = password.HashPassword(user1, "user1");
        string hashed2 = password.HashPassword(user1, "user2");

        user1.PasswordHash = hashed;
        user2.PasswordHash = hashed2;

        builder.HasData(user1, user2);
    }
}