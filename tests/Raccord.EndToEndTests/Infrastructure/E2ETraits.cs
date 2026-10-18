namespace Raccord.EndToEndTests.Infrastructure;

/// <summary>
/// The trait vocabulary the per-PR suite is selected and quarantined by.
///
/// <para>
/// <b>Put every trait on the concrete journey class. Never on a shared base.</b> A class that
/// declares any trait of its own inherits <i>none</i> from its base — putting an axis on a base
/// works right up until a subclass declares anything, and then silently stops. That is exactly the
/// invisible regression a selection scheme exists to prevent, so the scheme must not be built on it.
/// </para>
/// <para>
/// <b>Every axis has a documented meaning when absent</b>, because tagging is adopted
/// incrementally. An absent <see cref="Area"/> never <i>excludes</i> a journey: if it did, every
/// area selection would quietly under-run until the last journey was tagged. An absent
/// <see cref="Depth"/> defaults to <see cref="Depths.Full"/> because that is the genuine majority
/// value, not an absence of information.
/// </para>
/// </summary>
public static class E2ETraits
{
    /// <summary>
    /// The product bounded context a journey exercises. Repeatable: a finalize journey is genuinely
    /// an invoicing journey <i>and</i> a projects journey, and forcing one value would misclassify it.
    /// Use <see cref="Areas.CrossCutting"/> for platform journeys with no product context.
    /// </summary>
    public const string Area = "Area";

    /// <summary>How much of the suite the journey belongs to. Absent means <see cref="Depths.Full"/>.</summary>
    public const string Depth = "Depth";

    /// <summary>
    /// The business rule this journey would catch a violation of — not every rule in force while
    /// it runs. Repeatable, because one journey commonly defends more than one rule at once.
    /// Absent means the journey guards no rule in particular.
    /// </summary>
    public const string Rule = "Rule";

    /// <summary>
    /// Present on a journey that is excluded from the blocking selection. Its value is always
    /// <see cref="Quarantined"/>; presence is the whole signal — xUnit filters match the pair
    /// (<c>Quarantine!=true</c>), so a journey quarantined without a value, or with another value,
    /// would still run and the quarantine would be decoration. Every excluded journey is named in
    /// the job summary of the run that excluded it, so a quarantine is never discoverable only by
    /// reading a filter expression.
    /// </summary>
    public const string Quarantine = "Quarantine";

    /// <summary>Distinguishes non-journey verification tests from reportable E2E journeys.</summary>
    public const string TestKind = "TestKind";

    /// <summary>The only value <see cref="Quarantine"/> takes — xUnit filters match a name/value pair.</summary>
    public const string Quarantined = "true";

    public static class TestKinds
    {
        public const string Metadata = "metadata";
    }

    public static class Areas
    {
        public const string Contacts = "contacts";
        public const string Projects = "projects";
        public const string Equipment = "equipment";
        public const string Warehouse = "warehouse";
        public const string Crew = "crew";
        public const string Commercial = "commercial";
        public const string Invoicing = "invoicing";
        public const string Compliance = "compliance";
        public const string Documents = "documents";
        public const string Workspace = "workspace";
        public const string CrossCutting = "cross-cutting";
    }

    public static class Depths
    {
        /// <summary>
        /// The capabilities whose failure makes every other journey meaningless: the AppHost boots
        /// and serves the SPA, a customer can be created, a billable line can be added, an invoice
        /// can be finalized. One journey each, and no more — a smoke subset that grows stops
        /// being one.
        /// </summary>
        public const string Smoke = "smoke";

        /// <summary>The default, and never written: a journey with no <see cref="Depth"/> is full.</summary>
        public const string Full = "full";
    }
}
