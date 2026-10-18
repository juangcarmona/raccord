using System.Reflection;
using Xunit;

namespace Raccord.EndToEndTests.Infrastructure;

public sealed class E2EJourneyMetadata_Should_BeComplete
{
    [Trait(E2ETraits.TestKind, E2ETraits.TestKinds.Metadata)]
    [Fact]
    public void EveryConcreteJourney_ShouldDeclareCanonicalAreas()
    {
        var journeyTypes = typeof(BrowserJourneyBase)
            .Assembly.GetTypes()
            .Where(type =>
                type.IsClass
                && !type.IsAbstract
                && typeof(BrowserJourneyBase).IsAssignableFrom(type)
            )
            .Append(typeof(PublicSite.PublicHomepageJourney_Should_IntroduceTheProject))
            .ToArray();
        var canonicalAreas = typeof(E2ETraits.Areas)
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .Where(field => field.IsLiteral)
            .Select(field => (string)field.GetRawConstantValue()!)
            .ToHashSet(StringComparer.Ordinal);

        Assert.NotEmpty(journeyTypes);

        foreach (var journeyType in journeyTypes)
        {
            var areas = PlaywrightArtifacts.AreasFor(journeyType);

            Assert.True(
                areas.Length > 0,
                $"{journeyType.FullName} must declare at least one bounded-context Area trait."
            );
            Assert.All(areas, area => Assert.Contains(area, canonicalAreas));
        }
    }
}
