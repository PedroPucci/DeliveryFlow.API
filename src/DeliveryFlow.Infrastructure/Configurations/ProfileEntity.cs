using DeliveryFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeliveryFlow.Infrastructure.Configurations
{
    public class ProfileConfiguration : IEntityTypeConfiguration<ProfileEntity>
    {
        public void Configure(EntityTypeBuilder<ProfileEntity> builder)
        {
            builder.HasData(
                new ProfileEntity
                {
                    Id = "1",
                    Name = "Usuario",
                    NormalizedName = "USUARIO"
                }
            );
        }
    }
}