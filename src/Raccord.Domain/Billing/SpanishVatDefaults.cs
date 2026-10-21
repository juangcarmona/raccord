namespace Raccord.Domain.Billing;

/// <summary>Source-dated setup defaults for a new Spanish installation.</summary>
public static class SpanishVatDefaults
{
    /// <summary>Initial general-rate setup value, verified against official sources.</summary>
    public static readonly VatRateBootstrap StandardRate = new(
        "ES",
        21m,
        "https://sede.agenciatributaria.gob.es/Sede/ayuda/manuales-videos-folletos/manuales-practicos/manual-iva-2026/capitulo-04-sujetos-pasivos-repercusion-impositivo/tipo-impositivo.html",
        new DateOnly(2026, 10, 21)
    );
}
