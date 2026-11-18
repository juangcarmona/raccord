import { test } from "node:test";
import assert from "node:assert/strict";
import { createHash } from "node:crypto";
import { evaluateAudit } from "./check-frontend-audit.mjs";

const now = Date.parse("2026-11-18T16:00:00Z");
function fixture() {
    const lock = JSON.stringify({
        packages: {
            "node_modules/picomatch": { version: "4.0.7", dev: true },
            "node_modules/vite": { version: "7.3.1", dev: true },
        },
    });
    const advisory = {
        source: 1247710,
        name: "picomatch",
        dependency: "picomatch",
        url: "https://github.com/advisories/GHSA-fc5q-39xc-vjpf",
        severity: "high",
    };
    return {
        lock,
        policy: {
            schemaVersion: 1,
            advisory: "GHSA-fc5q-39xc-vjpf",
            advisorySource: 1247710,
            approvedBy: "juangcarmona",
            approvedAt: "2026-11-18T15:39:32Z",
            expiresAt: "2026-11-24T20:45:00Z",
            approvalUrl:
                "https://github.com/juangcarmona/raccord/issues/294#issuecomment-6983388300",
            reason: "Owner accepts the bounded development-tool risk.",
            lockfileSha256: createHash("sha256").update(lock).digest("hex"),
            allowedNodes: ["node_modules/picomatch", "node_modules/vite"],
        },
        report: {
            auditReportVersion: 2,
            vulnerabilities: {
                picomatch: {
                    name: "picomatch",
                    severity: "high",
                    via: [advisory],
                    nodes: ["node_modules/picomatch"],
                },
                vite: {
                    name: "vite",
                    severity: "high",
                    via: ["picomatch"],
                    nodes: ["node_modules/vite"],
                },
            },
            metadata: {
                vulnerabilities: {
                    info: 0,
                    low: 0,
                    moderate: 0,
                    high: 2,
                    critical: 0,
                    total: 2,
                },
            },
        },
    };
}
function check(f, time = now) {
    return evaluateAudit(f.report, f.lock, f.policy, time);
}
function changeLock(f, edit) {
    const lock = JSON.parse(f.lock);
    edit(lock);
    f.lock = JSON.stringify(lock);
    f.policy.lockfileSha256 = createHash("sha256").update(f.lock).digest("hex");
}

test("Accepts only the approved direct and transitive development findings", () => {
    const result = check(fixture());
    assert.equal(result.exceptionApplied, true);
    assert.equal(result.counts.high, 2);
});
test("Does not claim an exception when the report is clean", () => {
    const f = fixture();
    f.report.vulnerabilities = {};
    f.report.metadata.vulnerabilities.high =
        f.report.metadata.vulnerabilities.total = 0;
    assert.equal(check(f).exceptionApplied, false);
});
test("Refuses at the exact deadline and after expiry", () => {
    for (const time of ["2026-11-24T20:45:00Z", "2026-11-25T00:00:00Z"])
        assert.throws(() => check(fixture(), Date.parse(time)), /expired/);
});
test("Refuses missing approval, future approval and an extended deadline", () => {
    for (const edit of [
        (f) => {
            delete f.policy.approvalUrl;
        },
        (f) => {
            f.policy.approvedAt = "2026-11-19T00:00:00Z";
        },
        (f) => {
            f.policy.expiresAt = "2026-11-25T00:00:00Z";
        },
    ]) {
        const f = fixture();
        edit(f);
        assert.throws(() => check(f));
    }
});
test("Refuses a changed dependency graph", () => {
    const f = fixture();
    f.lock += "\n";
    assert.throws(() => check(f), /graph changed/);
});
test("Refuses the same advisory in a production dependency", () => {
    const f = fixture();
    changeLock(f, (lock) => {
        delete lock.packages["node_modules/picomatch"].dev;
    });
    assert.throws(() => check(f), /development graph/);
});
test("Refuses unknown and unapproved dependency nodes", () => {
    for (const path of ["node_modules/unknown", "node_modules/vite"]) {
        const f = fixture();
        f.report.vulnerabilities.picomatch.nodes = [path];
        f.policy.allowedNodes = ["node_modules/picomatch"];
        assert.throws(() => check(f), /Unknown dependency|development graph/);
    }
});
test("Refuses another advisory hidden inside the transitive chain", () => {
    const f = fixture();
    f.report.vulnerabilities.picomatch.via.push({
        source: 999,
        name: "picomatch",
        dependency: "picomatch",
        url: "https://github.com/advisories/GHSA-aaaa-bbbb-cccc",
        severity: "high",
    });
    assert.throws(() => check(f), /Unapproved/);
});
test("Refuses a substituted URL even with the approved numeric source", () => {
    const f = fixture();
    f.report.vulnerabilities.picomatch.via[0].url = "https://example.com";
    assert.throws(() => check(f), /Unapproved/);
});
test("Never excepts a critical finding", () => {
    const f = fixture();
    f.report.vulnerabilities.picomatch.severity = "critical";
    f.report.metadata.vulnerabilities.high = 1;
    f.report.metadata.vulnerabilities.critical = 1;
    assert.throws(() => check(f), /Critical/);
});
test("Refuses unknown or cyclic transitive chains", () => {
    for (const via of ["unknown", "vite"]) {
        const f = fixture();
        f.report.vulnerabilities.picomatch.via = [via];
        assert.throws(() => check(f), /Unknown audit|Cyclic/);
    }
});
test("Refuses malformed reports, advisory data and inconsistent counts", () => {
    for (const edit of [
        (f) => {
            f.report.error = { code: "EAUDIT" };
        },
        (f) => {
            f.report.auditReportVersion = 1;
        },
        (f) => {
            delete f.report.metadata;
        },
        (f) => {
            f.report.metadata.vulnerabilities.high = 0;
        },
        (f) => {
            f.report.metadata.vulnerabilities.total = 0;
        },
        (f) => {
            delete f.report.vulnerabilities.picomatch.nodes;
        },
        (f) => {
            f.report.vulnerabilities.picomatch.via = [{}];
        },
        (f) => {
            f.report.vulnerabilities.picomatch.severity = "unknown";
        },
        (f) => {
            f.report.vulnerabilities.picomatch.severity = "moderate";
            f.report.metadata.vulnerabilities.high = 1;
            f.report.metadata.vulnerabilities.moderate = 1;
        },
    ]) {
        const f = fixture();
        edit(f);
        assert.throws(() => check(f));
    }
});

test("Preserves npm severity for transitive findings without weakening direct advisories", () => {
    const f = fixture();
    f.report.vulnerabilities.vite.severity = "moderate";
    f.report.metadata.vulnerabilities.high = 1;
    f.report.metadata.vulnerabilities.moderate = 1;
    assert.equal(check(f).counts.high, 1);
});
