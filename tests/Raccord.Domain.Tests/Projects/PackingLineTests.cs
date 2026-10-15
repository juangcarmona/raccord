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

    [Fact]
    public void A_planned_line_Should_allow_role_and_quantity_adjustments()
    {
        var line = PackingLine.Create(Guid.CreateVersion7(), 1, PackingLineRole.Additional);

        line.Adjust(2, PackingLineRole.Optional);

        line.Quantity.ShouldBe(2);
        line.Role.ShouldBe(PackingLineRole.Optional);
    }

    [Fact]
    public void A_processed_line_Should_reject_adjustments()
    {
        var line = PackingLine.Create(Guid.CreateVersion7(), 1);
        line.Pack("warehouse", DateTimeOffset.UtcNow);

        Should
            .Throw<BusinessRuleViolationException>(() => line.Adjust(2, PackingLineRole.Temporary))
            .Code.ShouldBe("projects.packing-adjustment-locked");
    }

    [Fact]
    public void A_received_line_Should_record_one_exception_with_trace_metadata()
    {
        var line = PackingLine.Create(Guid.CreateVersion7(), 1);
        line.Pack("warehouse", DateTimeOffset.UtcNow);
        line.BookOut("warehouse", DateTimeOffset.UtcNow);
        line.Receive("warehouse", DateTimeOffset.UtcNow);

        line.RecordException(
            "lost",
            "Missing on return count",
            "coordinator",
            DateTimeOffset.UtcNow
        );

        line.ExceptionKind.ShouldBe("lost");
        line.ExceptionReportedBy.ShouldBe("coordinator");
        Should
            .Throw<BusinessRuleViolationException>(() =>
                line.RecordException("found", "Found later", "coordinator", DateTimeOffset.UtcNow)
            )
            .Code.ShouldBe("projects.exception-already-recorded");
    }

    [Fact]
    public void A_booked_out_line_Should_record_cross_docking_disposition()
    {
        var line = PackingLine.Create(Guid.CreateVersion7(), 1);
        line.Pack("warehouse", DateTimeOffset.UtcNow);
        line.BookOut("warehouse", DateTimeOffset.UtcNow);

        line.SetDisposition(PackingLineDisposition.CrossDocked, "coordinator");

        line.Disposition.ShouldBe(PackingLineDisposition.CrossDocked);
    }
}
