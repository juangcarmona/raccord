import { execFileSync } from "node:child_process";
import { createHash } from "node:crypto";
import { fileURLToPath } from "node:url";

// Branch-scoped recovery: keep the published harness and all authenticated writes unchanged.
export function createProductBlobCache(repository, network) {
    const git = (args) =>
        execFileSync("git", ["-C", repository, ...args], {
            maxBuffer: 1024 * 1024,
        });
    const rows = git([
        "ls-tree",
        "-r",
        "-z",
        "--format=%(objectmode) %(objecttype) %(objectname)",
        "HEAD",
        "--",
        "docs/product/model",
    ])
        .toString()
        .split("\0")
        .filter(Boolean);
    if (!rows.length || rows.length > 4096)
        throw Error("Product cache inventory exceeds bounds");
    const allowed = new Set(
        rows
            .filter((row) => /^(100644|100755) blob [a-f0-9]{40}$/.test(row))
            .map((row) => row.split(" ")[2]),
    );
    const cached = new Map();
    let hits = 0;
    let bytes = 0;
    return {
        get hits() {
            return hits;
        },
        async fetch(input, options) {
            const url = new URL(
                typeof input === "string" || input instanceof URL
                    ? input
                    : input.url,
            );
            const method = options?.method ?? input?.method ?? "GET";
            const match =
                /^\/repos\/juangcarmona\/raccord\/git\/blobs\/([a-f0-9]{40})$/.exec(
                    url.pathname,
                );
            if (
                url.origin !== "https://api.github.com" ||
                url.search ||
                method !== "GET" ||
                options?.body != null ||
                !match ||
                !allowed.has(match[1])
            )
                return network(input, options);
            const sha = match[1];
            if (!cached.has(sha)) {
                let content;
                try {
                    content = execFileSync(
                        "git",
                        ["-C", repository, "cat-file", "blob", sha],
                        { maxBuffer: 65537 },
                    );
                } catch {
                    throw Error(
                        "Product cache object unavailable or exceeds bounds",
                    );
                }
                if (
                    content.length > 65536 ||
                    bytes + content.length > 8 * 1024 * 1024
                )
                    throw Error("Product cache content exceeds bounds");
                if (
                    createHash("sha1")
                        .update(`blob ${content.length}\0`)
                        .update(content)
                        .digest("hex") !== sha
                )
                    throw Error("Product cache object identity mismatch");
                bytes += content.length;
                cached.set(sha, {
                    sha,
                    size: content.length,
                    encoding: "base64",
                    content: content.toString("base64"),
                });
            }
            hits++;
            return Response.json(cached.get(sha));
        },
    };
}

if (process.env.LIGHTHOUSE_PRODUCT_API_CACHE === "1") {
    const cache = createProductBlobCache(
        fileURLToPath(new URL("../", import.meta.url)),
        globalThis.fetch,
    );
    globalThis.fetch = cache.fetch;
    process.once("exit", () =>
        console.error(
            `Verified immutable product blob cache hits: ${cache.hits}`,
        ),
    );
}
