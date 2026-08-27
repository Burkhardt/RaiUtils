# RaiUtils

RaiUtils change requests and release notes are centralized in the RAIkeep [`doc/`](https://github.com/Burkhardt/RAIkeep/tree/main/doc) directory under `RaiUtils_...` filenames; they are not stored separately in this child repository.

    Random, Email, ParameterDictionary, JSON conversion helpers, and lightweight search expressions.

_formerly_ __RaiUtilsCore__

## 4.2.3

- Aligns RaiUtils with the coordinated seven-package RAIkeep 4.2.3 release implementing CR015.
- Adds `RaiException`, the dependency-light base for RAIkeep domain exceptions.
- Adds `ToolNotFoundException` with stable tool-name and executable-path diagnostics.
- Establishes the shared exception boundary used by RaiImage and the upcoming RaiDiagram package.
- Keeps RaiUtils aligned with the `mkdir` polymorphism update line introduced in OsLib.
- Documents the supported cloud-backed provider claim used with OsLib and JsonPit: `Dropbox`, `OneDrive`, `GoogleDrive`, and `ICloudDrive`.
- Notes the JsonPit identifier migration from `Name` to `Id`, including legacy normalization behavior.
- Refreshes live package docs for the `4.2.3` line.
- Provides utility helpers for:
    - Email syntax validation (`Email`)
    - JSON token conversion to plain dictionaries/arrays (`JsonConversionExtensions`)
    - Lowercase key filtering for incoming name-value parameters (`ParameterDictionary`)
    - Randomization extensions for enumerable/list sources (`RandomExtensions`)
    - Wildcard-and expression matching against object properties (`SearchExpression`)

## namespace

RaiUtils

## classes

### Email: Validates an email address string using regex.

- Email: `Valid`, `Invalid`, `ToString`

### JsonConversionExtensions: Converts `JObject` and `JArray` to plain .NET dictionary/array trees.

- JsonConversionExtensions: `ToDictionary`, `ToArray`

### ParameterDictionary: StringDictionary filtered to lowercase keys from NameValueCollection.

- ParameterDictionary: constructor with lowercase-key import behavior

### RandomExtensions: Random element selection and shuffle helpers for `IEnumerable` and `IList`.

- RandomExtensions: `Random`, `Shuffle`, `TakeAny`

### SearchExpression: Parses text search patterns and evaluates matches against object properties.

- SearchExpression: `ConditionsAsString`, `IsMatch(object)`

## dependencies

- NuGet package dependency: `Newtonsoft.Json`.
- No direct code dependency on OsLib types is present in current RaiUtils source.

## cross-package cloud root convention

RaiUtils does not resolve cloud roots itself, but it is intended to stay compatible with the same machine-local configuration contract used by OsLib and JsonPit.

Recommended shared contract across .NET and upcoming Python packages:
- Use `RAIkeep.json5` for explicit cloud roots.
- Rely on `~/.config/RAIkeep.json5` unless the host application overrides the config bootstrap path.
- Reuse the same PascalCase `Cloud.*` keys: `Cloud.Dropbox`, `Cloud.OneDrive`, `Cloud.GoogleDrive`, `Cloud.ICloudDrive`.
- Prefer explicit Ubuntu Google Drive configuration over probe-only assumptions when packages are used together in development or deployment tooling.
- Treat `Dropbox`, `OneDrive`, `GoogleDrive`, and `ICloudDrive` as the current supported provider set for the packaged stack.

## nuget

https://www.nuget.org/packages/RaiUtils/

## diagram

- Source: [RaiUtils-ClassDiagram.puml](RaiUtils-ClassDiagram.puml)
- Rendered SVG: [RaiUtils-ClassDiagram.svg](RaiUtils-ClassDiagram.svg)
- CLI render (if PlantUML is installed): `plantuml RaiUtils-ClassDiagram.puml`
- VS Code: open the `.puml` file and use a PlantUML preview/render extension.

## detailed api

- Foldable class and method-level documentation: [API.md](https://github.com/Burkhardt/RaiUtils/blob/main/API.md)

## release notes

- Current release notes: [RaiUtils_RELEASE_NOTES_4.2.3.md](https://github.com/Burkhardt/RAIkeep/blob/main/doc/RaiUtils_RELEASE_NOTES_4.2.3.md)

## unit tests

- Local unit tests are in [tests/RaiUtils.Tests](tests/RaiUtils.Tests).
- Run from `RaiUtils` root: `dotnet test RaiUtils.slnx`

## solution format

- Upgraded solution format is available as `RaiUtils.slnx` (generated with `dotnet solution migrate`).
