using API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Confgurations
{
    public class UserConfigurations : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(x => x.Email).HasMaxLength(30).IsRequired();
            builder.HasIndex(x => x.Email);
            builder.Property(x => x.FirstName).HasMaxLength(30).IsRequired();
            builder.Property(x => x .LastName).HasMaxLength(30).IsRequired();
            builder.HasKey(x => x.Id);
        }
    }
}
