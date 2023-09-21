namespace ContellectTask.Infrastructure;

public class ContactAddressEntityConfiguration : BaseEntityConfiguration<ContactAddress>
{
    public override void Configure(EntityTypeBuilder<ContactAddress> builder)
    {
        builder.ToTable(nameof(ContactAddress));

        builder.Property(x => x.ContactId)
               .IsRequired(true);

        builder.Property(x => x.AddressId)
               .IsRequired(true);

        builder.HasIndex(e => new { e.ContactId, e.AddressId })
               .IsUnique();

        builder.HasOne(x => x.Address)
               .WithMany()
               .HasForeignKey(e => e.AddressId)
               .OnDelete(DeleteBehavior.Restrict)
               .HasConstraintName("FK_ContactAddress_Address_AddressId");

        base.Configure(builder);
    }
}
