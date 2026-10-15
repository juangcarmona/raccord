using Raccord.Domain.Projects;
using Raccord.SharedKernel.Common;
using Shouldly;

namespace Raccord.Domain.Tests.Projects;

public sealed class PackingLineTests
{
    [Fact]
    public void A_packing_line_Should_require_pack_before_book_out_and_receive()
    {
        var line = PackingLine.Create(Guid.CreateVersion7(), 1);

        Should
            .Throw<BusinessRuleViolationException>(() =>
                line.BookOut("warehouse", DateTimeOffset.UtcNow)
            )
            .Code.ShouldBe("projects.book-out-not-packed");

        line.Pack("warehouse", DateTimeOffset.UtcNow);
        line.BookOut("warehouse", DateTimeOffset.UtcNow);
        line.Receive("warehouse", DateTimeOffset.UtcNow);

        line.IsPacked.ShouldBeTrue();
        line.IsBookedOut.ShouldBeTrue();
        line.IsReceived.ShouldBeTrue();
    }

    [Fact]
    public void A_packing_line_Should_reject_duplicate_booking()
    {
        var line = PackingLine.Create(Guid.CreateVersion7(), 1);
        line.Pack("warehouse", DateTimeOffset.UtcNow);
        line.BookOut("warehouse", DateTimeOffset.UtcNow);

        Should
            .Throw<BusinessRuleViolationException>(() =>
                line.BookOut("warehouse", DateTimeOffset.UtcNow)
            )
            .Code.ShouldBe("projects.book-out-already-complete");
    }
}
