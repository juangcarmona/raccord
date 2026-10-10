import createClient from "openapi-fetch";
import type { components, paths } from "./schema";

/** Wire types generated from the backend's OpenAPI document (npm run api:generate). */
export type Schemas = components["schemas"];

export interface ApiIssue {
    code: string;
    message: string;
}

/** A failed API call, carrying the stable rule code and issues from RFC 9457 problem details. */
export class ApiError extends Error {
    constructor(
        readonly status: number,
        readonly code: string,
        message: string,
        readonly issues: readonly ApiIssue[] = [],
    ) {
        super(message);
        this.name = "ApiError";
    }

    get isNotFound(): boolean {
        return this.status === 404;
    }
}

export const api = createClient<paths>({
    baseUrl: globalThis.location.origin,
    // Resolve fetch per call so tests and future interceptors can replace it.
    fetch: (request: Request) => globalThis.fetch(request),
});

interface FetchResult<T> {
    data?: T;
    error?: unknown;
    response: Response;
}

/** Returns the response body or throws an ApiError describing the problem. */
export async function unwrap<T>(call: Promise<FetchResult<T>>): Promise<T> {
    const { data, error, response } = await call;
    if (response.ok) {
        return data as T;
    }
    throw toApiError(response.status, error);
}

function toApiError(status: number, body: unknown): ApiError {
    const problem = (typeof body === "object" && body !== null ? body : {}) as {
        code?: unknown;
        title?: unknown;
        detail?: unknown;
        issues?: unknown;
    };
    const issues = Array.isArray(problem.issues)
        ? problem.issues.filter(
              (issue): issue is ApiIssue => typeof issue === "object" && issue !== null && "code" in issue,
          )
        : [];
    const code = typeof problem.code === "string" ? problem.code : status === 404 ? "not-found" : "unknown";
    const message =
        typeof problem.detail === "string"
            ? problem.detail
            : typeof problem.title === "string"
              ? problem.title
              : `HTTP ${status}`;
    return new ApiError(status, code, message, issues);
}
