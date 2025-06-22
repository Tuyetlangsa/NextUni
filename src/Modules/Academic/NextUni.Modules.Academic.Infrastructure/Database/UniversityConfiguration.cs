using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NextUni.Modules.Academic.Domain.Universities;

namespace NextUni.Modules.Academic.Infrastructure.Database;

public class UniversityConfiguration : IEntityTypeConfiguration<University>
{
    public void Configure(EntityTypeBuilder<University> builder)
    {
        builder.ToTable("universities");
        
        builder.HasKey(u => u.Id);
        
        builder.Property(u => u.Id)
            .IsRequired();
        
        builder.Property(u => u.Code)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.HasIndex(u => u.Code)
            .IsUnique();
        
        builder.Property(u => u.Name)
            .IsRequired()
            .HasMaxLength(255);
        
        builder.Property(u => u.Region)
            .HasConversion<byte>()
            .IsRequired();
        
        builder.Property(u => u.UniversityType)
            .HasConversion<byte>() 
            .IsRequired();
        
        builder.Property(u => u.Address)
            .HasMaxLength(500);
        
        builder.Property(u => u.Email)
            .HasMaxLength(255);
        
        builder.Property(u => u.WebsiteUrl)
            .HasMaxLength(255);
        
        builder.Property(u => u.FacebookUrl)
            .HasMaxLength(255);
        
        builder.Property(u => u.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

    }
}