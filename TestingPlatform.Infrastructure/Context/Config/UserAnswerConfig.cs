using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestingPlatform.Domain.Entities;

namespace TestingPlatform.Infrastructure.Context.Config;

public class UserAnswerConfig : IEntityTypeConfiguration<UserAnswer>
{
    public void Configure(EntityTypeBuilder<UserAnswer> builder)
    {
        builder
            .ToTable("UserAnswer");
    }
}