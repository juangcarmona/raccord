using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Raccord.Infrastructure.Persistence.Configurations;

/// <summary>Optimistic concurrency based on PostgreSQL's system column <c>xmin</c>.</summary>
internal static class ConcurrencyTokenExtensions
{
    private const string Version = "Version";

    public static void HasConcurrencyToken<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : class =>
        builder
            .Property<uint>(Version)
            .HasColumnName("xmin")
            .HasColumnType("xid")
            .ValueGeneratedOnAddOrUpdate()
            .IsConcurrencyToken();

    public static void HasConcurrencyToken<TOwner, TOwned>(
        this OwnedNavigationBuilder<TOwner, TOwned> builder
    )
        where TOwner : class
        where TOwned : class =>
        builder
            .Property<uint>(Version)
            .HasColumnName("xmin")
            .HasColumnType("xid")
            .ValueGeneratedOnAddOrUpdate()
            .IsConcurrencyToken();
}
