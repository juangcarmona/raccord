# Fast Forward run #300

Status: blocked at the Product Change decision for the prerequisite finance catalogs. The Product Change for contact defaults was approved, applied and validated on PR #394. Three missing catalog issues were created and linked under the Invoicing and Commercial epics; #300 is natively blocked by those issues and existing #323.

PR #420 proposes the catalog Product Change. It leaves two product questions open: whether VAT classes are seeded, and whether rental/sale defaults are separate sets or per-group percentages shared across those categories. Catalog and #300 implementation have not started. Resume this run after the product owner resolves those questions; then complete the OpenSpec plan and Owner Planned gate before coding.

Scope audit: #320 covers quote/contract settings, so it is not a prerequisite. Independent review recommended reusing #323 for invoice moments, creating #417 payment terms and #418 VAT catalogs under #379, and #419 discount groups under #378. Keep #300 under #373.
