# RaiUtils

    Random, Email, ParameterDictionary, JSON conversion helpers, and lightweight search expressions.

_formerly_ __RaiUtilsCore__

## 3.7.2

- Patch: aligns with OsLibCore 3.7.2 in the current publish order.
- No API surface change in RaiUtils for this patch release.

## 3.7.1

- Patch: aligns with `OsLibCore 3.7.1` in the correct NuGet publish order.
- Documents the supported cloud-backed provider claim used with OsLib and JsonPit: `OneDrive`, `GoogleDrive`, and `Dropbox`.
- Notes the JsonPit identifier migration from `Name` to `Id`, including legacy normalization behavior.
- Provides utility helpers for:
    - Email syntax validation (`Email`)
    - JSON token conversion to plain dictionaries/arrays (`JsonConversionExtensions`)
    - Lowercase key filtering for incoming name-value parameters (`ParameterDictionary`)
    - Randomization extensions for enumerable/list sources (`RandomExtensions`)
    - Wildcard-and expression matching against object properties (`SearchExpression`)

## namespace

RaiUtils

## classes

<details>
<summary>Email: Validates an email address string using regex.</summary>

- Email: `Valid`, `Invalid`, `ToString`
</details>

<details>
<summary>JsonConversionExtensions: Converts `JObject` and `JArray` to plain .NET dictionary/array trees.</summary>

- JsonConversionExtensions: `ToDictionary`, `ToArray`
</details>

<details>
<summary>ParameterDictionary: StringDictionary filtered to lowercase keys from NameValueCollection.</summary>

- ParameterDictionary: constructor with lowercase-key import behavior
</details>

<details>
<summary>RandomExtensions: Random element selection and shuffle helpers for `IEnumerable` and `IList`.</summary>

- RandomExtensions: `Random`, `Shuffle`, `TakeAny`
</details>

<details>
<summary>SearchExpression: Parses text search patterns and evaluates matches against object properties.</summary>

- SearchExpression: `ConditionsAsString`, `IsMatch(object)`
</details>

## dependencies

- NuGet package dependency: `Newtonsoft.Json`.
- No direct code dependency on OsLib types is present in current RaiUtils source.

## cross-package cloud root convention

RaiUtils does not resolve cloud roots itself, but it is intended to stay compatible with the same machine-local configuration contract used by OsLib and JsonPit.

Recommended shared contract across .NET and upcoming Python packages:
- Use `osconfig.json` for explicit cloud roots.
- Rely on `~/.config/RAIkeep/osconfig.json` on macOS/Linux or `%APPDATA%\RAIkeep\osconfig.json` on Windows.
- Reuse the same `cloud` keys: `dropbox`, `onedrive`, `googledrive`.
- Prefer explicit Ubuntu Google Drive configuration over probe-only assumptions when packages are used together in development or deployment tooling.
- Treat `OneDrive`, `GoogleDrive`, and `Dropbox` as the current supported provider set for the packaged stack.

## nuget

https://www.nuget.org/packages/RaiUtils/

## diagram

- Source: [RaiUtils-ClassDiagram.puml](RaiUtils-ClassDiagram.puml)
- Rendered SVG: [RaiUtils-ClassDiagram.svg](RaiUtils-ClassDiagram.svg)
- CLI render (if PlantUML is installed): `plantuml RaiUtils-ClassDiagram.puml`
- VS Code: open the `.puml` file and use a PlantUML preview/render extension.

## detailed api

- Foldable class and method-level documentation: [API.md](API.md)

## release notes

- Current release notes: [RELEASE_NOTES_3.7.2.md](RELEASE_NOTES_3.7.2.md)

## unit tests

- Local unit tests are in [tests/RaiUtils.Tests](tests/RaiUtils.Tests).
- Run from `RaiUtils` root: `dotnet test RaiUtils.slnx`

## solution format

- Upgraded solution format is available as `RaiUtils.slnx` (generated with `dotnet solution migrate`).
