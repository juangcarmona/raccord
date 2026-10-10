using System.Reflection;
using System.Text.RegularExpressions;

namespace Raccord.Architecture.Tests;

/// <summary>
///     Executable version of the test-naming rule in docs/architecture/architecture-rules.md:
///     every xUnit test reads as Thing_Should_Outcome so a failure's name states the expectation
///     that broke.
/// </summary>
public sealed class TestNaming_Should_Constraint
{
    // Thing segments may be snake_case (long subject names read better split); the middle must be
    // the literal _Should_; the outcome is at least one segment. Kept hand-rolled rather than via
    // NetArchTest to stay consistent with the reflection style of DependencyRulesTests.
    private static readonly Regex Pattern = new(
        @"^[A-Za-z0-9]+(_[A-Za-z0-9]+)*_Should_[A-Za-z0-9]+(_[A-Za-z0-9]+)*$",
        RegexOptions.ExplicitCapture
    );

    [Fact]
    public void Every_test_Method_Should_read_as_Thing_Should_Outcome()
    {
        var assemblies = TestAssemblies();

        var offenders = assemblies
            .SelectMany(assembly => assembly.GetTypes())
            .SelectMany(type => type.GetMethods(BindingFlags.Public | BindingFlags.Instance))
            .Where(method =>
                method
                    .GetCustomAttributes()
                    .Any(attribute =>
                        attribute.GetType().FullName
                            is "Xunit.FactAttribute"
                                or "Xunit.TheoryAttribute"
                    )
            )
            .Where(method =>
                !Pattern.IsMatch(method.Name)
                && !Pattern.IsMatch(method.DeclaringType?.Name ?? string.Empty)
            )
            .Select(method => $"{method.DeclaringType?.Name}.{method.Name}")
            .Order(StringComparer.Ordinal)
            .ToList();

        offenders.ShouldBeEmpty(
            $"Test names must read as Thing_Should_Outcome; offending tests:{Environment.NewLine}"
                + string.Join(Environment.NewLine, offenders)
        );
    }

    private static IEnumerable<Assembly> TestAssemblies() =>
        Directory
            .EnumerateFiles(AppContext.BaseDirectory, "Raccord.*.Tests.dll")
            .Select(Assembly.LoadFrom);
}
