namespace ContellectTask.Infrastructure;

public class AddressEntityConfiguration : BaseEntityConfiguration<Address>
{
    const int MAXLENGTH500 = 500;
    public override void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.ToTable(nameof(Address));

        builder.Property(x => x.Country)
               .IsRequired(true)
               .HasMaxLength(MAXLENGTH500);

        builder.Property(x => x.City)
               .IsRequired(true)
               .HasMaxLength(MAXLENGTH500);

        builder.Property(x => x.Street)
               .IsRequired(true)
               .HasMaxLength(MAXLENGTH500);

        builder.Property(x => x.MoreInfo)
               .IsRequired(false)
               .HasMaxLength(1000);

        base.Configure(builder);
    }
}
