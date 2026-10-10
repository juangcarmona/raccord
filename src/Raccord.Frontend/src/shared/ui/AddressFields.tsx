import type { Schemas } from "../api/client";
import { useI18n } from "../i18n/I18nProvider";
import { Field } from "./Form";
import { formText } from "./formText";

/** Postal address inputs; `required` applies to the fields printed on invoices. */
export function AddressFields({
    legend,
    required,
    initial,
}: {
    legend: string;
    required: boolean;
    initial?: Schemas["AddressDto"] | null;
}) {
    const { t } = useI18n();
    return (
        <fieldset className="grid-form">
            <legend>{legend}</legend>
            <Field
                label={t("common.street")}
                name="street"
                required={required}
                maxLength={200}
                defaultValue={initial?.street ?? ""}
                autoComplete="street-address"
            />
            <Field
                label={t("common.postalCode")}
                name="postalCode"
                required={required}
                maxLength={20}
                defaultValue={initial?.postalCode ?? ""}
                autoComplete="postal-code"
            />
            <Field
                label={t("common.city")}
                name="city"
                required={required}
                maxLength={100}
                defaultValue={initial?.city ?? ""}
                autoComplete="address-level2"
            />
            <Field
                label={t("common.province")}
                name="province"
                maxLength={100}
                defaultValue={initial?.province ?? ""}
                autoComplete="address-level1"
            />
            <Field
                label={t("common.country")}
                name="countryCode"
                required={required}
                minLength={2}
                maxLength={2}
                defaultValue={initial?.countryCode ?? "ES"}
                autoComplete="country"
            />
        </fieldset>
    );
}

/** Reads the address inputs; returns null when the street is empty. */
export function readAddress(data: FormData): Schemas["AddressDto"] | null {
    const value = (name: string) => formText(data, name).trim();
    if (value("street") === "") return null;
    return {
        street: value("street"),
        postalCode: value("postalCode"),
        city: value("city"),
        province: value("province"),
        countryCode: value("countryCode").toUpperCase(),
    };
}
