# Data portability

The self-hosting administrator can create a migration bundle from the `deploy/` folder:

```bash
bash scripts/export.sh ./exports
```

The bundle contains a PostgreSQL custom-format dump, the stored document volume archive and the checksum manifest produced by the backup tooling. The command publishes the archive and its checksum only after the source export succeeds.

This is deliberately a host-operator workflow. The current application has authenticated sessions but does not yet have role-scoped export permissions, so there is no broad HTTP export endpoint. Do not expose the bundle through the application or commit it to the repository.

Treat the bundle as sensitive installation data: it includes customer records, issued invoices, document files, password hashes and data-protection material required by the installation. Encrypt it before copying it off-host, restrict access to the installation administrator, and remove temporary copies after transfer. Verify the adjacent `.sha256` file before importing the bundle into another installation.

The bundle is complementary to the regular backup schedule: backups provide recovery points, while an export is an intentional migration artifact with a separately recorded checksum.
