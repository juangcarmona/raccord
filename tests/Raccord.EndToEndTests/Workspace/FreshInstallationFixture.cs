using Raccord.EndToEndTests.Infrastructure;

namespace Raccord.EndToEndTests.Workspace;

public sealed class FreshInstallationFixture : DistributedAppFixture
{
    protected override bool FreshInstallation => true;
}
