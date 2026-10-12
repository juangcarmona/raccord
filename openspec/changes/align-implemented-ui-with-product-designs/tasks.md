## 1. Reference mapping and shared system

- [x] 1.1 Record each in-scope route’s closest design and behavior trace, unsupported concepts, and explicit gaps in `docs/product/design/implemented-screen-mapping.md`; verify every route in the router is covered.
- [x] 1.2 Align shared shell, responsive layout, and global tokens with `docs/design/`; verify frontend tests, keyboard navigation, and desktop/narrow screenshots.

## 2. Screen alignment

- [x] 2.1 Align project and customer routes to their closest references without adding unsupported workflow; verify existing route tests and both locales.
- [x] 2.2 Align drafts, invoices, and issuer settings to their references without changing API behavior; verify existing route tests and both locales.
- [x] 2.3 Add/update accessibility regression coverage for shared navigation and capture shared-layout desktop/narrow states; verify frontend test and lint/build commands.

## 3. Delivery verification

- [ ] 3.1 Run CI-equivalent frontend checks and review desktop/narrow screenshots; verify no API/schema/domain files changed.
- [x] 3.2 Verify PDaC citations and OpenSpec validation; acceptance-criteria review still requires runtime screenshots and Owner confirmation.

<!-- pdac-scope: cited -->
<!-- pdac:cite id="JRN-PRJ-RUN-RENTAL-PROJECT" digest="sha256:2fc29ff0184c294517c659ec8fc12660b4f1262d49a53426103d87058fbb51fe" -->
<!-- pdac:cite id="JRN-BILL-FINISHED-PROJECT" digest="sha256:b9530a2b0a9aac0f9cb3a5a9608494d08be1fc1190f395887fc7bb7fb30df28e" -->
<!-- pdac:cite id="JRN-SET-UP-INVOICING" digest="sha256:a47b84f924d17095df0ad8bc4e3436fa67d9d51bd1d241c7228a7624cb8c548e" -->
<!-- pdac:cite id="QR-ACCESSIBILITY" digest="sha256:65d1d92daf2189c837030c9eaaf7d0ba18a28a0f52cea624e919f6fbc80ac882" -->
<!-- pdac:cite id="QR-LOCALIZATION" digest="sha256:defb01ff947a5d42bdc3b6b2fe7d58c7d3aead62e91f759c0118f40749654ddf" -->
