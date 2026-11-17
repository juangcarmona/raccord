import assert from "node:assert/strict";
import { execFileSync } from "node:child_process";
import { mkdtempSync, mkdirSync, writeFileSync, rmSync } from "node:fs";
import { tmpdir } from "node:os";
import { join } from "node:path";
import { test } from "node:test";
import { createProductBlobCache } from "./product-model-api-cache.mjs";

const base = "https://api.github.com/repos/juangcarmona/raccord/git/blobs/";

test("immutable model reads preserve bytes while unrelated reads and writes use GitHub", async () => {
    const root = mkdtempSync(join(tmpdir(), "raccord-product-cache-"));
    try {
        const git = (args) =>
            execFileSync("git", ["-C", root, ...args], {
                encoding: "utf8",
            }).trim();
        git(["init", "-q"]);
        mkdirSync(join(root, "docs/product/model"), { recursive: true });
        writeFileSync(
            join(root, "docs/product/model/example.md"),
            "Canonical model\n",
        );
        writeFileSync(join(root, "outside.md"), "Outside model\n");
        git(["add", "."]);
        git([
            "-c",
            "user.name=Cache Test",
            "-c",
            "user.email=test@example.invalid",
            "commit",
            "-qm",
            "test: model",
        ]);
        const sha = git(["rev-parse", "HEAD:docs/product/model/example.md"]);
        const outside = git(["rev-parse", "HEAD:outside.md"]);
        const calls = [];
        const network = async (...args) => {
            calls.push(args);
            return new Response("network", { status: 403 });
        };
        const cache = createProductBlobCache(root, network);
        // A dirty working tree must never replace the requested immutable object.
        writeFileSync(
            join(root, "docs/product/model/example.md"),
            "Unapproved edit\n",
        );
        const response = await cache.fetch(base + sha, { method: "GET" });
        const body = await response.json();
        assert.equal(response.status, 200);
        assert.equal(body.sha, sha);
        assert.equal(
            Buffer.from(body.content, "base64").toString(),
            "Canonical model\n",
        );
        assert.equal(body.size, Buffer.byteLength("Canonical model\n"));
        assert.equal(calls.length, 0);
        for (const url of [
            base + outside,
            base + "0".repeat(40),
            base.replace("/raccord/", "/another/") + sha,
        ]) {
            assert.equal((await cache.fetch(url)).status, 403);
        }
        const options = { method: "POST", body: '{"untouched":true}' };
        assert.equal((await cache.fetch(base + sha, options)).status, 403);
        assert.equal(calls.at(-1)[1], options);
        assert.equal(cache.hits, 1);
        assert.equal(calls.length, 4);
    } finally {
        rmSync(root, { recursive: true, force: true });
    }
});

test("oversized model objects fail closed", async () => {
    const root = mkdtempSync(join(tmpdir(), "raccord-product-cache-size-"));
    try {
        const git = (args) =>
            execFileSync("git", ["-C", root, ...args], {
                encoding: "utf8",
            }).trim();
        git(["init", "-q"]);
        mkdirSync(join(root, "docs/product/model"), { recursive: true });
        writeFileSync(
            join(root, "docs/product/model/oversized.md"),
            "x".repeat(65537),
        );
        git(["add", "."]);
        git([
            "-c",
            "user.name=Cache Test",
            "-c",
            "user.email=test@example.invalid",
            "commit",
            "-qm",
            "test: large model",
        ]);
        const sha = git(["rev-parse", "HEAD:docs/product/model/oversized.md"]);
        const cache = createProductBlobCache(root, () => {
            throw Error("Unexpected network access");
        });
        await assert.rejects(cache.fetch(base + sha), /bound/);
    } finally {
        rmSync(root, { recursive: true, force: true });
    }
});
