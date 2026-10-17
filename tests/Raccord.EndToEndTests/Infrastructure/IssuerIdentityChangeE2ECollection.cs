using Xunit;

namespace Raccord.EndToEndTests.Infrastructure;

/// <summary>Isolates issuer identity mutations from the shared journey database.</summary>
[CollectionDefinition(Name)]
public sealed class IssuerIdentityChangeE2ECollection : ICollectionFixture<DistributedAppFixture>
{
    public const string Name = "e2e-issuer-identity-change";
}
