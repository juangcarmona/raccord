<!-- pdac-scope: cited -->
<!-- pdac:cite id="UC-EQP-CONFIGURE-ITEM-DETAILS" digest="sha256:2fbfe3c2f9d575707e792b862fb3f7dbb17099ff35c3189f00d14f6d56242d38" -->
<!-- pdac:cite id="UC-EQP-ADD-ITEM-IMAGE" digest="sha256:3b1d8b73be5aa936bc9ee63d964abfc6d78635b030df13b6fb6a788e3ea779be" -->
<!-- pdac:cite id="UC-EQP-CONFIGURE-ACCESSORIES" digest="sha256:3b3faa3b81f4695a61822fdd9d34b0ee681e02a7731a92f2185565bd948efd39" -->
<!-- pdac:cite id="UC-EQP-SET-ALTERNATIVES" digest="sha256:4f20d3dee03e9fc2014288ac15c8acdc70bbfb074a11ff154ca46208be8fd75d" -->

# Design

Equipment owns the item metadata and stores relationships as JSON identifier collections until the related catalogue slices provide richer cross-item constraints. Images use the existing provider-agnostic document store; only the object key is persisted. The API validates image media type and size before storage.
