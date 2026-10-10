import type { Schemas } from "../api/client";
import { useI18n } from "../i18n/I18nProvider";

/** Displays server-calculated totals and VAT breakdown. */
export function Totals({ totals }: { totals: Schemas["InvoiceTotalsResponse"] }) {
    const { t, format } = useI18n();
    const money = (amount: number) => format.money(amount, totals.currency);
    return (
        <dl className="totals">
            {totals.vatBreakdown.map((group) => (
                <div key={group.rate} className="totals__group">
                    <dt>
                        {t("common.taxableBase")} ({format.percent(group.rate)})
                    </dt>
                    <dd>{money(group.taxableBase)}</dd>
                    <dt>
                        {t("common.vatAmount")} {format.percent(group.rate)}
                    </dt>
                    <dd>{money(group.vatAmount)}</dd>
                </div>
            ))}
            <div className="totals__grand">
                <dt>{t("common.total")}</dt>
                <dd>{money(totals.total)}</dd>
            </div>
        </dl>
    );
}

/** Table of invoice lines with server-calculated amounts. */
export function LinesTable({
    caption,
    lines,
    currency,
    renderAction,
}: {
    caption: string;
    lines: readonly Schemas["InvoiceLineResponse"][];
    currency: string;
    renderAction?: (line: Schemas["InvoiceLineResponse"]) => React.ReactNode;
}) {
    const { t, format } = useI18n();
    return (
        <div className="table-scroll">
            <table>
                <caption>{caption}</caption>
                <thead>
                    <tr>
                        <th scope="col">{t("common.description")}</th>
                        <th scope="col" className="num">
                            {t("common.quantity")}
                        </th>
                        <th scope="col" className="num">
                            {t("common.unitPrice")}
                        </th>
                        <th scope="col" className="num">
                            {t("common.discount")}
                        </th>
                        <th scope="col" className="num">
                            {t("common.vat")}
                        </th>
                        <th scope="col" className="num">
                            {t("common.netAmount")}
                        </th>
                        {renderAction && (
                            <th scope="col">
                                <span className="visually-hidden">{t("common.actions")}</span>
                            </th>
                        )}
                    </tr>
                </thead>
                <tbody>
                    {lines.map((line) => (
                        <tr key={line.position}>
                            <td>{line.description}</td>
                            <td className="num">{format.quantity(line.quantity)}</td>
                            <td className="num">{format.money(line.unitPrice, currency)}</td>
                            <td className="num">
                                {line.discountPercent === 0 ? "—" : format.percent(line.discountPercent)}
                            </td>
                            <td className="num">{format.percent(line.vatRate)}</td>
                            <td className="num">{format.money(line.netAmount, currency)}</td>
                            {renderAction && <td>{renderAction(line)}</td>}
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
}
