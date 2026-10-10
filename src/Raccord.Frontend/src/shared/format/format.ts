import type { Locale } from '../i18n/messages';

const intlLocale: Record<Locale, string> = { es: 'es-ES', en: 'en-GB' };
const calendarDate = /^(\d{4})-(\d{2})-(\d{2})$/;

/**
 * Locale-aware presentation of values computed by the server. Nothing here calculates
 * amounts, taxes or totals: the API is the single source of those numbers.
 */
export interface Formatters {
  money: (amount: number, currency: string) => string;
  percent: (value: number) => string;
  quantity: (value: number) => string;
  /** Formats an ISO calendar date (yyyy-mm-dd) exactly as written, in any time zone. */
  date: (value: string) => string;
  /** Formats an ISO instant in the given (default: viewer's) time zone. */
  dateTime: (value: string, timeZone?: string) => string;
}

export function createFormatters(locale: Locale): Formatters {
  const tag = intlLocale[locale];
  const percent = new Intl.NumberFormat(tag, { style: 'percent', maximumFractionDigits: 2 });
  const quantity = new Intl.NumberFormat(tag, { maximumFractionDigits: 4 });
  const dateFormat = new Intl.DateTimeFormat(tag, locale === 'es'
    ? { day: '2-digit', month: '2-digit', year: 'numeric', timeZone: 'UTC' }
    : { day: '2-digit', month: 'short', year: 'numeric', timeZone: 'UTC' });

  return {
    money: (amount, currency) =>
      new Intl.NumberFormat(tag, { style: 'currency', currency, minimumFractionDigits: 2, maximumFractionDigits: 2 }).format(amount),
    percent: (value) => percent.format(value / 100),
    quantity: (value) => quantity.format(value),
    date: (value) => {
      const match = calendarDate.exec(value);
      if (!match) {
        throw new RangeError(`Invalid calendar date: ${value}`);
      }
      const [, year, month, day] = match.map(Number) as [number, number, number, number];
      return dateFormat.format(new Date(Date.UTC(year, month - 1, day)));
    },
    dateTime: (value, timeZone) =>
      new Intl.DateTimeFormat(tag, {
        day: '2-digit',
        month: '2-digit',
        year: 'numeric',
        hour: '2-digit',
        minute: '2-digit',
        ...(timeZone ? { timeZone } : {}),
      }).format(new Date(value)),
  };
}
