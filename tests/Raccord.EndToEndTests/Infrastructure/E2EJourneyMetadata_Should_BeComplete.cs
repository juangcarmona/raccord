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
            Assert.True(
                new[] { E2ETraits.EvidenceModes.Api, E2ETraits.EvidenceModes.Browser }.Contains(
                    PlaywrightArtifacts.EvidenceModeFor(journeyType),
                    StringComparer.Ordinal
                )
            );
        }
    }

    [Trait(E2ETraits.TestKind, E2ETraits.TestKinds.Metadata)]
    [Fact]
    public void ApiOnlyJourneys_ShouldDeclareApiEvidenceMode()
    {
        var apiJourneys = new[]
        {
            typeof(Crew.CrewAvailabilityJourney_Should_ManageInvitationsAndJobBoard),
            typeof(Crew.CrewHoursJourney_Should_ApproveAndExportPayroll),
            typeof(Crew.CrewPlanningJourney_Should_PlanResources),
            typeof(Crew.CrewRecordsJourney_Should_ManageCrewVehiclesAndRates),
            typeof(Crew.WorkdayJourney_Should_RecordAuditableEvents),
            typeof(Documents.CommunicationJourney_Should_RecordDeliveryOutcomeThroughApi),
            typeof(Documents.DocumentGenerationJourney_Should_CreateLabels),
            typeof(Documents.DocumentTemplateJourney_Should_ConfigureBranding),
            typeof(Documents.DocumentTemplateJourney_Should_PrepareForeignLanguage),
            typeof(Equipment.WarehouseJourney_Should_BookEquipment),
            typeof(Integrations.ApiAccessJourney_Should_EnforceScopes),
            typeof(Integrations.CalendarFeedJourney_Should_PublishPrivateIcs),
            typeof(Integrations.CompanyRegistryJourney_Should_HandleUnavailableProvider),
            typeof(Integrations.PaymentReportingJourney_Should_UseScopedToken),
            typeof(Invoicing.InboundEInvoiceJourney_Should_TrackStatus),
            typeof(Invoicing.InvoiceCollectionsJourney_Should_RecordAndReportPayments),
            typeof(Projects.ProjectContextJourney_Should_KeepWorkingRecordsTogether),
            typeof(Projects.ProjectTimeJourney_Should_ReportPlannedAndActualWork),
        };

        foreach (var journeyType in apiJourneys)
        {
            Assert.Equal(
                E2ETraits.EvidenceModes.Api,
                PlaywrightArtifacts.EvidenceModeFor(journeyType)
            );
            Assert.False(
                PlaywrightArtifacts.ShouldCaptureVideo(
                    PlaywrightArtifacts.EvidenceModeFor(journeyType),
                    visualEvidenceEnabled: true
                )
            );
        }

        Assert.True(
            PlaywrightArtifacts.ShouldCaptureVideo(
                E2ETraits.EvidenceModes.Browser,
                visualEvidenceEnabled: true
            )
        );
    }
}
