namespace ContellectTask.Infrastructure;

public class ContactEntityConfiguration : BaseEntityConfiguration<Contact>
{
    public override void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder.ToTable(nameof(Contact));

        builder.Property(x => x.Name)
               .IsRequired(true)
               .HasMaxLength(200);

        builder.HasIndex(x => x.Name)
               .IsUnique(true);

        builder.Property(x => x.Phone)
               .IsRequired(true)
               .HasMaxLength(20);

        builder.HasIndex(x => x.Phone)
               .IsUnique(true);

        builder.Property(x => x.Address)
               .IsRequired(true)
               .HasMaxLength(500);

        builder.Property(x => x.Notes)
               .IsRequired(false)
               .HasMaxLength(1000);

        base.Configure(builder);
    }
}