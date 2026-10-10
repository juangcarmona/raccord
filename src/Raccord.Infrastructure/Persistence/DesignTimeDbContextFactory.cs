using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Raccord.Infrastructure.Persistence;

/// <summary>Lets <c>dotnet ef migrations add</c> build the model without starting the web host or a database.</summary>
internal sealed class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<RaccordDbContext>
{
    public RaccordDbContext CreateDbContext(string[] args) =>
        new(new DbContextOptionsBuilder<RaccordDbContext>().UseNpgsql("Host=localhost;Database=raccord").Options);
}
