<!-- pdac-scope: cited -->

## Evidence source

Use successful Playwright checkpoints from the per-PR E2E workflow. Those journeys start the real application with isolated synthetic test data, record a passed result, and upload screenshots for seven days. The committed public asset is a reviewed selection, not a failure screenshot, video, trace or live application endpoint.

## Publication

Keep the selected PNG files in `src/public-site/images/`. The static build copies them into `dist/images/`. The invoice capture masks the test login, recipient name and tax identifier in the browser before saving the publication artifact. Each image has concise alt text and a caption naming the visible workflow. Re-capture and inspect an image when the corresponding screen changes.

<!-- pdac:cite id="UC-OBTAIN-INVOICE-DOCUMENT" digest="sha256:ba891b44976370dba63dc987cbe9fd4ba8d78de034684e9e5beb4cbfbfb07933" -->
<!-- pdac:cite id="JRN-BILL-FINISHED-PROJECT" digest="sha256:b9530a2b0a9aac0f9cb3a5a9608494d08be1fc1190f395887fc7bb7fb30df28e" -->
