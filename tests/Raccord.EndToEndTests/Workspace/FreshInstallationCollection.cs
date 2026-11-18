using Xunit;

namespace Raccord.EndToEndTests.Workspace;

// AppHosts have independent databases but install into the same frontend directory.
// Run this collection alone so npm ci cannot remove another host's live dependencies.
[CollectionDefinition(Name, DisableParallelization = true)]
public sealed class FreshInstallationCollection : ICollectionFixture<FreshInstallationFixture>
{
    public const string Name = "fresh-installation";
}
