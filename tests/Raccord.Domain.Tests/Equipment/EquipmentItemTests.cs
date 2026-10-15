using Raccord.Domain.Equipment;
using Raccord.SharedKernel.Common;
using Shouldly;

namespace Raccord.Domain.Tests.Equipment;

public sealed class EquipmentItemTests
{
    [Fact]
    public void Scan_code_and_availability_Should_share_one_stock_state()
    {
        var item = EquipmentItem.Create("CAM-01", "Camera", 2);
        item.AssignScanCode("scan-CAM-01");
        item.BookOut(1);
        item.Receive(1);

        item.ScanCode.ShouldBe("scan-CAM-01");
        item.AvailableQuantity.ShouldBe(2);
    }

    [Fact]
    public void Booking_more_than_available_Should_be_rejected()
    {
        var item = EquipmentItem.Create("CAM-01", "Camera", 1);

        Should.Throw<BusinessRuleViolationException>(() => item.BookOut(2))
            .Code.ShouldBe("equipment.stock-unavailable");
    }
}
