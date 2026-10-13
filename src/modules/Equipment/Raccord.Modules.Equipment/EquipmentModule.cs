using Microsoft.Extensions.DependencyInjection;

namespace Raccord.Modules.Equipment;

/// <summary>Composition root for the Equipment bounded context.</summary>
public static class EquipmentModule
{
    /// <summary>Registers the Equipment module's empty persistence seam.</summary>
    public static IServiceCollection AddEquipmentModule(this IServiceCollection services) =>
        services;
}
