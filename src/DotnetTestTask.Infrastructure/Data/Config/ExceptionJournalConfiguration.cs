using DotnetTestTask.Core.JournalAggregate;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
namespace DotnetTestTask.Infrastructure.Data.Config;

internal sealed class ExceptionJournalConfiguration 
    : IEntityTypeConfiguration<ExceptionJournal>
{
    public void Configure(EntityTypeBuilder<ExceptionJournal> builder)
    {
        builder.ToTable("exception_journals");

        builder.HasKey(j => j.Id);

        builder.Property(j => j.Id)
            .HasConversion(id => id.Value, value => JournalId.Create(value))
            .ValueGeneratedOnAdd();

        builder.Property(j => j.EventId)
            .IsRequired();

        builder.Property(j => j.Timestamp)
            .IsRequired();

        builder.Property(j => j.Type)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(j => j.Message)
            .IsRequired();

        builder.Property(j => j.Parameters)
            .IsRequired();

        builder.Property(j => j.StackTrace)
            .IsRequired();
    }
}