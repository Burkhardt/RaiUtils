# RaiUtils

## 4.5.10

Coordinated 4.5.10 release; public behavior is aligned with the synchronized platform.

Latest release notes: [RaiUtils 4.5.10 release notes](https://github.com/Burkhardt/RAIkeep/blob/main/doc/releases/v4.x/RaiUtils_RELEASE_NOTES_4.x.md#4510).

## Previous Versions

For release notes and changelogs of earlier releases, see the [RaiUtils 4.x Release Notes](https://github.com/Burkhardt/RAIkeep/blob/main/doc/releases/v4.x/RaiUtils_RELEASE_NOTES_4.x.md).

## Installation

### .NET CLI

```bash
dotnet add package RaiUtils --version 4.5.10
```

### PackageReference

```xml
<PackageReference Include="RaiUtils" Version="4.5.10" />
```

RaiUtils provides shared exceptions, word-case and Unicode helpers, JSON conversion, search, email, parameters, and randomization. It was formerly named RaiUtilsCore.

## namespace

RaiUtils

## classes

### StringHelper: general casing, word splitting, and lossless display-seam extensions.

- StringHelper: `ToTitle`, `WordSplit`, `CamelSplit`, `WordSeams`
- `WordSeams` returns UTF-16 offsets; callers slice the immutable original and choose their own presentation mechanism.

### WordCase: converts between PascalCase, lower camelCase, snake_case, kebab-case, and token arrays.

- WordCase: `Array`, `String`, `PascalCase`, `LowerCamelCase`, `SnakeCase`, `KebabCase`
- Usage and seam semantics: [CASE_CONVERSION.md](https://github.com/Burkhardt/RaiUtils/blob/main/CASE_CONVERSION.md)

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

## unit tests

- Local unit tests are in [tests/RaiUtils.Tests](tests/RaiUtils.Tests).
- Run from `RaiUtils` root: `dotnet test RaiUtils.slnx`

## solution format

- Upgraded solution format is available as `RaiUtils.slnx` (generated with `dotnet solution migrate`).
