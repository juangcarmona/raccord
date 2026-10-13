<!-- pdac-scope: cited -->
<!-- pdac:cite id="UC-EQP-CONFIGURE-ITEM-DETAILS" digest="sha256:2fbfe3c2f9d575707e792b862fb3f7dbb17099ff35c3189f00d14f6d56242d38" -->
<!-- pdac:cite id="UC-EQP-ADD-ITEM-IMAGE" digest="sha256:3b1d8b73be5aa936bc9ee63d964abfc6d78635b030df13b6fb6a788e3ea779be" -->
<!-- pdac:cite id="UC-EQP-CONFIGURE-ACCESSORIES" digest="sha256:3b3faa3b81f4695a61822fdd9d34b0ee681e02a7731a92f2185565bd948efd39" -->
<!-- pdac:cite id="UC-EQP-SET-ALTERNATIVES" digest="sha256:4f20d3dee03e9fc2014288ac15c8acdc70bbfb074a11ff154ca46208be8fd75d" -->

# Equipment item details

## ADDED Requirements

### Requirement: Equipment items expose descriptive metadata and relationships

Equipment items MUST persist optional details, accessory identifiers, alternative identifiers, and an image key.

#### Scenario: Update item details

- **WHEN** a client updates an existing item's details and relationships
- **THEN** the API persists and returns the supplied values

### Requirement: Equipment images use the document store

The image endpoint MUST accept supported image content, reject empty, non-image, or oversized content, and persist only the document key on the item.

#### Scenario: Store a valid image

- **WHEN** a client uploads an image no larger than 5 MB
- **THEN** the document store receives the content and the item returns its stored key

#### Scenario: Reject invalid image content

- **WHEN** a client uploads empty, non-image, or over-limit content
- **THEN** the document store is not called and the item is not updated
