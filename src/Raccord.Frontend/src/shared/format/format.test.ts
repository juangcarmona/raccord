import { describe, expect, it } from "vitest";
import { createFormatters } from "./format";

// Intl inserts narrow/no-break spaces; normalize them so expectations stay readable.
const plain = (value: string) => value.replace(/\s/g, " ");

describe("formatters", () => {
    const es = createFormatters("es");
    const en = createFormatters("en");

    it("formats money in the invoice currency for the active locale", () => {
        expect(plain(es.money(1548.8, "EUR"))).toBe("1548,80 €");
        expect(plain(es.money(12548.8, "EUR"))).toBe("12.548,80 €");
        expect(en.money(1548.8, "EUR")).toBe("€1,548.80");
    });

    it("formats VAT rates and discounts as percentages without inventing decimals", () => {
        expect(plain(es.percent(21))).toBe("21 %");
        expect(plain(es.percent(5.5))).toBe("5,5 %");
        expect(en.percent(10)).toBe("10%");
    });

    it("formats quantities with up to four decimals", () => {
        expect(es.quantity(1.5)).toBe("1,5");
        expect(en.quantity(0.1234)).toBe("0.1234");
    });

    it("formats calendar dates without shifting them across time zones", () => {
        // A date-only value must never be parsed as UTC midnight and rendered as the previous day.
        expect(es.date("2026-05-01")).toBe("01/05/2026");
        expect(en.date("2026-12-31")).toBe("31 Dec 2026");
    });

    it("formats instants in the viewer time zone", () => {
        expect(es.dateTime("2026-05-11T09:30:00Z", "Europe/Madrid")).toBe("11/05/2026, 11:30");
    });

    it("rejects malformed calendar dates instead of guessing", () => {
        expect(() => es.date("11/05/2026")).toThrow("Invalid calendar date");
    });
});
