import { createHash } from "node:crypto";
import { readFile, mkdir, writeFile } from "node:fs/promises";
import { spawnSync } from "node:child_process";
import { dirname, join, resolve } from "node:path";
import { fileURLToPath } from "node:url";

const root = resolve(dirname(fileURLToPath(import.meta.url)), "..");
const levels = ["info", "low", "moderate", "high", "critical"];
const record = (value) =>
    value !== null && typeof value === "object" && !Array.isArray(value);
const requireValue = (condition, message) => {
    if (!condition) throw Error(message);
};
const hash = (value) => createHash("sha256").update(value).digest("hex");

export function evaluateAudit(report, lockText, policy, now = Date.now()) {
    requireValue(
        record(policy) &&
            policy.schemaVersion === 1 &&
            policy.advisory === "GHSA-fc5q-39xc-vjpf" &&
            policy.advisorySource === 1247710 &&
            policy.approvedBy === "juangcarmona" &&
            typeof policy.reason === "string" &&
            policy.reason.trim() &&
            /^https:\/\/github\.com\/juangcarmona\/raccord\/issues\/294#issuecomment-[1-9]\d*$/.test(
                policy.approvalUrl,
            ),
        "Missing or invalid owner-approved exception.",
    );
    const approved = Date.parse(policy.approvedAt),
        expires = Date.parse(policy.expiresAt);
    requireValue(
        Number.isFinite(now) &&
            Number.isFinite(approved) &&
            Number.isFinite(expires) &&
            approved <= now &&
            now < expires &&
            expires > approved &&
            expires === Date.parse("2026-11-24T20:45:00Z"),
        "Security exception is expired or has invalid dates.",
    );
    requireValue(
        hash(lockText) === policy.lockfileSha256,
        "Approved frontend dependency graph changed.",
    );
    requireValue(
        Array.isArray(policy.allowedNodes) &&
            policy.allowedNodes.length &&
            policy.allowedNodes.every((node) => typeof node === "string"),
        "Invalid approved dependency nodes.",
    );
    const lock = JSON.parse(lockText);
    requireValue(record(lock.packages), "Invalid frontend lockfile.");
    requireValue(
        record(report) &&
            report.auditReportVersion === 2 &&
            !report.error &&
            record(report.vulnerabilities) &&
            record(report.metadata?.vulnerabilities),
        "Invalid npm audit report.",
    );
    const counts = Object.fromEntries(levels.map((level) => [level, 0]));
    for (const [name, finding] of Object.entries(report.vulnerabilities)) {
        requireValue(
            record(finding) &&
                finding.name === name &&
                levels.includes(finding.severity) &&
                Array.isArray(finding.via) &&
                finding.via.length &&
                Array.isArray(finding.nodes) &&
                finding.nodes.length,
            "Malformed vulnerability entry.",
        );
        counts[finding.severity]++;
        for (const node of finding.nodes) {
            requireValue(
                typeof node === "string" &&
                    node.startsWith("node_modules/") &&
                    node.endsWith("node_modules/" + name) &&
                    record(lock.packages[node]) &&
                    !lock.packages[node].link,
                "Unknown dependency node.",
            );
            if (finding.severity === "high" || finding.severity === "critical")
                requireValue(
                    lock.packages[node].dev === true &&
                        policy.allowedNodes.includes(node),
                    "High or critical finding is outside the approved development graph.",
                );
        }
    }
    for (const level of levels)
        requireValue(
            report.metadata.vulnerabilities[level] === counts[level],
            "Audit vulnerability counts disagree.",
        );
    requireValue(
        report.metadata.vulnerabilities.total ===
            Object.values(counts).reduce((a, b) => a + b, 0),
        "Audit total disagrees.",
    );
    function advisories(name, visiting = new Set()) {
        requireValue(!visiting.has(name), "Cyclic audit dependency chain.");
        const finding = report.vulnerabilities[name];
        requireValue(record(finding), "Unknown audit dependency chain.");
        const next = new Set(visiting).add(name);
        return finding.via.flatMap((via) => {
            if (typeof via === "string") return advisories(via, next);
            requireValue(
                record(via) &&
                    levels.includes(via.severity) &&
                    Number.isSafeInteger(via.source) &&
                    typeof via.url === "string",
                "Malformed advisory.",
            );
            return via.severity === "high" || via.severity === "critical"
                ? [via]
                : [];
        });
    }
    requireValue(
        counts.critical === 0,
        "Critical findings are never excepted.",
    );
    for (const [name, finding] of Object.entries(report.vulnerabilities)) {
        const causes = advisories(name);
        requireValue(
            finding.via
                .filter((cause) => typeof cause === "object")
                .every(
                    (cause) =>
                        levels.indexOf(cause.severity) <=
                        levels.indexOf(finding.severity),
                ),
            "Audit finding understates its advisory severity.",
        );
        if (finding.severity !== "high") continue;
        requireValue(
            causes.length &&
                causes.every(
                    (cause) =>
                        cause.source === policy.advisorySource &&
                        cause.severity === "high" &&
                        cause.name === "picomatch" &&
                        cause.dependency === "picomatch" &&
                        cause.url ===
                            "https://github.com/advisories/" + policy.advisory,
                ),
            "Unapproved high or critical advisory.",
        );
    }
    return {
        counts,
        exceptionApplied: counts.high > 0,
        advisory: policy.advisory,
        expiresAt: policy.expiresAt,
    };
}

async function main() {
    const audit = spawnSync("npm", ["audit", "--json", "--audit-level=high"], {
        cwd: join(root, "src/Raccord.Frontend"),
        encoding: "utf8",
        timeout: 120000,
        maxBuffer: 16 * 1024 * 1024,
    });
    const directory = join(root, "obj/security");
    await mkdir(directory, { recursive: true });
    await writeFile(
        join(directory, "frontend-audit.json"),
        audit.stdout ?? "",
        { mode: 0o600 },
    );
    // Retain the unchanged raw report in native command logs as well as the CI artifact.
    process.stdout.write(audit.stdout ?? "");
    requireValue(
        !audit.error && !audit.signal && [0, 1].includes(audit.status),
        "npm audit did not complete normally.",
    );
    const report = JSON.parse(audit.stdout);
    const result = evaluateAudit(
        report,
        await readFile(
            join(root, "src/Raccord.Frontend/package-lock.json"),
            "utf8",
        ),
        JSON.parse(
            await readFile(
                join(root, ".github/frontend-audit-exception.json"),
                "utf8",
            ),
        ),
    );
    requireValue(
        (audit.status === 0) ===
            (result.counts.high + result.counts.critical === 0),
        "npm audit exit status disagrees with the report.",
    );
    console.log(
        "\nAudit gate passed. Raw findings: " +
            JSON.stringify(result.counts) +
            (result.exceptionApplied
                ? ". Owner-approved development exception " +
                  result.advisory +
                  " applied until " +
                  result.expiresAt +
                  "."
                : ". No exception needed."),
    );
}

if (
    process.argv[1] &&
    resolve(process.argv[1]) === fileURLToPath(import.meta.url)
)
    main().catch((error) => {
        console.error("Frontend audit gate failed: " + error.message);
        process.exitCode = 1;
    });
