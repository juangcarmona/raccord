/** Reads a text form field; file inputs and missing fields read as an empty string. */
export function formText(data: FormData, name: string): string {
    const value = data.get(name);
    return typeof value === "string" ? value : "";
}
