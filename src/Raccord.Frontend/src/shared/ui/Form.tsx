import { useId, type InputHTMLAttributes, type ReactNode } from "react";
import { useI18n } from "../i18n/I18nProvider";

type FieldProps = Omit<InputHTMLAttributes<HTMLInputElement>, "id"> & {
    label: string;
    hint?: string;
};

/** A labelled input. Required fields are marked by the browser; optional ones say so in the label. */
export function Field({ label, hint, required, ...input }: FieldProps) {
    const id = useId();
    const { t } = useI18n();
    return (
        <div className="field">
            <label htmlFor={id}>
                {label}
                {!required && <span className="field__optional"> ({t("common.optional")})</span>}
            </label>
            <input id={id} required={required} aria-describedby={hint ? `${id}-hint` : undefined} {...input} />
            {hint && (
                <p id={`${id}-hint`} className="field__hint">
                    {hint}
                </p>
            )}
        </div>
    );
}

export function SelectField({
    label,
    children,
    ...select
}: { label: string; children: ReactNode } & React.SelectHTMLAttributes<HTMLSelectElement>) {
    const id = useId();
    return (
        <div className="field">
            <label htmlFor={id}>{label}</label>
            <select id={id} {...select}>
                {children}
            </select>
        </div>
    );
}

export function FormError({ error }: { error: unknown }) {
    const { t, describe } = useI18n();
    if (!error) return null;
    const known = error as { code?: string; message?: string; issues?: readonly { code: string; message: string }[] };
    return (
        <div className="state state--error" role="alert">
            <p>{known.code ? describe("error", known.code, known.message) : t("error.generic")}</p>
            {known.issues && known.issues.length > 0 && (
                <ul>
                    {known.issues.map((issue) => (
                        <li key={issue.code}>{describe("issue", issue.code, issue.message)}</li>
                    ))}
                </ul>
            )}
        </div>
    );
}
