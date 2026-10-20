#!/usr/bin/env bash
# Pins the per-PR E2E selection against a committed manifest.
#
# The failure this guards is invisible without it: if the default filter ever narrows — a stray trait,
# an edited expression, a renamed collection — the suite simply runs fewer journeys and stays green.
# Nobody reads a passing job. So the *discovered set* is committed, and a difference is a build failure
# that names exactly which journeys appeared or vanished.
#
# Discovery, not execution, on purpose: a filter decides what is discovered. The executed count can be
# larger, because a theory is discovered once and run once per case, and that expansion is not
# something a filter can change.
#
# The manifest pins the *blocking* filter, quarantine included. Quarantining a journey is therefore a
# deliberate manifest edit somebody reviews, as well as a trait on the class — a journey cannot leave
# the gate by attribute alone.
#
#   usage: check-e2e-selection.sh [--update]
set -euo pipefail

PROJECT="tests/Raccord.EndToEndTests/Raccord.EndToEndTests.csproj"
MANIFEST="tests/Raccord.EndToEndTests/per-pr-selection.txt"
FILTER="Quarantine!=true&TestKind!=metadata"
ASSEMBLY="tests/Raccord.EndToEndTests/bin/Release/net10.0/Raccord.EndToEndTests.dll"

if [ ! -f "$ASSEMBLY" ]; then
  echo "No Release E2E assembly at $ASSEMBLY. Build the E2E project before checking or updating the manifest." >&2
  exit 1
fi

if find tests/Raccord.EndToEndTests -type f \( -name '*.cs' -o -name '*.csproj' \) -newer "$ASSEMBLY" -print -quit | grep -q .; then
  echo "E2E sources changed after $ASSEMBLY. Build the E2E project before checking or updating the manifest." >&2
  exit 1
fi

# Release and --no-build, matching the workflow exactly: a manifest generated from a different
# configuration would either disagree with CI or, worse, look for build output CI never produced.
discover() {
  dotnet test "$PROJECT" --no-build --configuration Release -t --filter "$FILTER" 2>/dev/null \
    | sed -n 's/^    \(Raccord\.EndToEndTests\..*\)$/\1/p' \
    | LC_ALL=C sort -u
}

actual="$(discover)"

if [ "${1:-}" = "--update" ]; then
  printf '%s\n' "$actual" > "$MANIFEST"
  echo "Updated $MANIFEST ($(printf '%s\n' "$actual" | wc -l | tr -d ' ') journeys)."
  exit 0
fi

if [ ! -f "$MANIFEST" ]; then
  echo "No manifest at $MANIFEST. Run: bash $0 --update" >&2
  exit 1
fi

if diff_output="$(diff <(LC_ALL=C sort -u "$MANIFEST") <(printf '%s\n' "$actual"))"; then
  echo "Per-PR E2E selection matches the manifest ($(printf '%s\n' "$actual" | wc -l | tr -d ' ') journeys)."
  exit 0
fi

cat >&2 <<EOF
The per-PR E2E selection has changed.

  < only in the manifest (a journey stopped being selected)
  > only in the run      (a journey started being selected)

$diff_output

If this is intended — a journey added, removed, or deliberately quarantined — run:

  bash $0 --update

and commit the manifest with the change that caused it. If it is not intended, the filter has
narrowed and the suite is now covering less than it reports.
EOF
exit 1
