using Xunit;

namespace Raccord.EndToEndTests.Infrastructure;

/// <summary>
/// Binds every E2E journey to the single shared <see cref="DistributedAppFixture"/>, so the AppHost
/// and the browser boot once per run (the AppHost alone takes minutes; per-class fixtures would
/// multiply that by the number of journeys).
/// </summary>
[CollectionDefinition(Name)]
public sealed class E2ECollection : ICollectionFixture<DistributedAppFixture>
{
    public const string Name = "e2e";
}
