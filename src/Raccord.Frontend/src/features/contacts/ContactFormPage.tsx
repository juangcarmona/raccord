import { useState, type SubmitEvent } from "react";
import { Link, useNavigate, useParams } from "react-router";
import { api, unwrap, type Schemas } from "../../shared/api/client";
import { useQuery } from "../../shared/hooks/useQuery";
import { useI18n } from "../../shared/i18n/I18nProvider";
import { AddressFields, readAddress } from "../../shared/ui/AddressFields";
import { Field, FormError } from "../../shared/ui/Form";
import { PageHeader } from "../../shared/ui/PageHeader";
import { Async } from "../../shared/ui/States";
import { formText } from "../../shared/ui/formText";

const roles = ["Customer", "Supplier", "Venue"] as const;

export function ContactFormPage() {
    const { id } = useParams();
    const query = useQuery(`contact:${id ?? "new"}`, () =>
        id ? unwrap(api.GET("/api/contacts/{id}", { params: { path: { id } } })) : Promise.resolve(null),
    );

    return <Async query={query}>{(contact) => <ContactForm key={contact?.id ?? "new"} contact={contact} />}</Async>;
}

function ContactForm({ contact }: { contact: Schemas["ContactResponse"] | null }) {
    const { t } = useI18n();
    const navigate = useNavigate();
    const [error, setError] = useState<unknown>(null);
    const [busy, setBusy] = useState(false);

    async function submit(event: SubmitEvent<HTMLFormElement>) {
        event.preventDefault();
        const form = event.currentTarget;
        const data = new FormData(form);
        const optional = (name: string) => {
            const value = formText(data, name).trim();
            return value === "" ? null : value;
        };
        const body = {
            type: formText(data, "type"),
            legalName: formText(data, "legalName"),
            contactNumber: optional("contactNumber"),
            taxId: optional("taxId"),
            email: optional("email"),
            address: readAddress(data),
            roles: roles.filter((role) => data.get(role) === "on"),
        };
        setBusy(true);
        setError(null);
        try {
            if (contact) {
                await unwrap(api.PUT("/api/contacts/{id}", { params: { path: { id: contact.id } }, body }));
            } else {
                await unwrap(api.POST("/api/contacts", { body }));
            }
            await navigate("/contacts");
        } catch (failure) {
            setError(failure);
            setBusy(false);
        }
    }

    return (
        <>
            <PageHeader title={contact ? t("contacts.edit") : t("contacts.new")} />
            <form
                className="panel"
                onSubmit={(event) => {
                    void submit(event);
                }}
            >
                <div className="grid-form">
                    <label>
                        <span>{t("contacts.type")}</span>
                        <select name="type" defaultValue={contact?.type ?? "Company"}>
                            <option value="Company">{t("contacts.type.company")}</option>
                            <option value="PrivatePerson">{t("contacts.type.person")}</option>
                        </select>
                    </label>
                    <Field
                        label={t("common.legalName")}
                        name="legalName"
                        required
                        maxLength={200}
                        autoComplete="organization"
                        defaultValue={contact?.legalName}
                    />
                    <Field
                        label={t("contacts.number")}
                        name="contactNumber"
                        maxLength={64}
                        defaultValue={contact?.contactNumber}
                    />
                    <Field label={t("common.taxId")} name="taxId" maxLength={32} defaultValue={contact?.taxId ?? ""} />
                    <Field
                        label={t("common.email")}
                        name="email"
                        type="email"
                        maxLength={320}
                        autoComplete="email"
                        defaultValue={contact?.email ?? ""}
                    />
                </div>
                <fieldset>
                    <legend>{t("contacts.roles")}</legend>
                    <div className="actions">
                        {roles.map((role) => (
                            <label key={role}>
                                <input type="checkbox" name={role} defaultChecked={contact?.roles.includes(role)} />
                                {role === "Customer"
                                    ? t("contacts.role.customer")
                                    : role === "Supplier"
                                      ? t("contacts.role.supplier")
                                      : t("contacts.role.venue")}
                            </label>
                        ))}
                    </div>
                </fieldset>
                <AddressFields legend={t("common.address")} required={false} initial={contact?.address ?? null} />
                <div className="actions">
                    <button type="submit" className="button button--primary" disabled={busy}>
                        {t("common.save")}
                    </button>
                    <Link className="button button--quiet" to="/contacts">
                        {t("common.cancel")}
                    </Link>
                </div>
                <FormError error={error} />
            </form>
            {contact && <ContactPeopleSection contactId={contact.id} />}
        </>
    );
}

function ContactPeopleSection({ contactId }: { contactId: string }) {
    const { t } = useI18n();
    const [editing, setEditing] = useState<Schemas["ContactPersonResponse"] | null>(null);
    const [formVersion, setFormVersion] = useState(0);
    const [error, setError] = useState<unknown>(null);
    const [busy, setBusy] = useState(false);
    const query = useQuery(`contact-people:${contactId}`, () =>
        unwrap(api.GET("/api/contacts/{id}/people", { params: { path: { id: contactId } } })),
    );

    async function save(event: SubmitEvent<HTMLFormElement>) {
        event.preventDefault();
        const formData = new FormData(event.currentTarget);
        const optional = (name: string) => {
            const value = formText(formData, name).trim();
            return value === "" ? null : value;
        };
        const body = {
            name: formText(formData, "name").trim(),
            email: optional("email"),
            phone: optional("phone"),
            isDefault: formData.get("isDefault") === "on",
            isAdministrative: formData.get("isAdministrative") === "on",
        };
        setBusy(true);
        setError(null);
        try {
            if (editing) {
                await unwrap(
                    api.PUT("/api/contacts/{id}/people/{personId}", {
                        params: { path: { id: contactId, personId: editing.id } },
                        body,
                    }),
                );
            } else {
                await unwrap(
                    api.POST("/api/contacts/{id}/people", {
                        params: { path: { id: contactId } },
                        body,
                    }),
                );
            }
            setEditing(null);
            setFormVersion((value) => value + 1);
            query.reload();
        } catch (failure) {
            setError(failure);
        } finally {
            setBusy(false);
        }
    }

    return (
        <section className="panel" aria-labelledby="contact-people-heading">
            <h2 id="contact-people-heading">{t("contacts.people.title")}</h2>
            <Async query={query}>
                {(people) => (
                    <>
                        {people.length === 0 ? (
                            <p className="state state--empty">{t("contacts.people.empty")}</p>
                        ) : (
                            <ul>
                                {people.map((person) => (
                                    <li key={person.id}>
                                        <strong>{person.name}</strong>
                                        {person.email && <span> · {person.email}</span>}
                                        {person.phone && <span> · {person.phone}</span>}
                                        {person.isDefault && <span> · {t("contacts.people.default")}</span>}
                                        {person.isAdministrative && (
                                            <span> · {t("contacts.people.administrative")}</span>
                                        )}
                                        <button
                                            type="button"
                                            className="button button--quiet"
                                            onClick={() => {
                                                setEditing(person);
                                                setError(null);
                                            }}
                                        >
                                            {t("contacts.people.edit")}
                                        </button>
                                    </li>
                                ))}
                            </ul>
                        )}
                    </>
                )}
            </Async>
            <h3>{editing ? t("contacts.people.editHeading") : t("contacts.people.addHeading")}</h3>
            <form
                key={`${editing?.id ?? "new-person"}-${formVersion}`}
                className="grid-form"
                onSubmit={(event) => void save(event)}
            >
                <Field
                    label={t("contacts.people.name")}
                    name="name"
                    required
                    maxLength={200}
                    defaultValue={editing?.name ?? ""}
                />
                <Field
                    label={t("contacts.people.email")}
                    name="email"
                    type="email"
                    maxLength={320}
                    defaultValue={editing?.email ?? ""}
                />
                <Field
                    label={t("contacts.people.phone")}
                    name="phone"
                    type="tel"
                    maxLength={64}
                    defaultValue={editing?.phone ?? ""}
                />
                <div className="actions">
                    <label>
                        <input type="checkbox" name="isDefault" defaultChecked={editing?.isDefault ?? false} />
                        {t("contacts.people.default")}
                    </label>
                    <label>
                        <input
                            type="checkbox"
                            name="isAdministrative"
                            defaultChecked={editing?.isAdministrative ?? false}
                        />
                        {t("contacts.people.administrative")}
                    </label>
                </div>
                <div className="actions">
                    <button type="submit" className="button button--primary" disabled={busy}>
                        {editing ? t("contacts.people.save") : t("contacts.people.add")}
                    </button>
                    {editing && (
                        <button
                            type="button"
                            className="button button--quiet"
                            onClick={() => {
                                setEditing(null);
                            }}
                        >
                            {t("common.cancel")}
                        </button>
                    )}
                </div>
                <FormError error={error} />
            </form>
        </section>
    );
}
