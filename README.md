# RaiUtils

    Random, Email, ParameterDictionary, JSON conversion helpers, and lightweight search expressions.

_formerly_ __RaiUtilsCore__

## 2.3.1

- Targets `net10.0` and packages as `RaiUtilsCore`.
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

## nuget

https://www.nuget.org/packages/RaiUtilsCore/

## diagram

- Source: [RaiUtils-ClassDiagram.puml](RaiUtils-ClassDiagram.puml)
- Rendered SVG: [RaiUtils-ClassDiagram.svg](RaiUtils-ClassDiagram.svg)
- CLI render (if PlantUML is installed): `plantuml RaiUtils-ClassDiagram.puml`
- VS Code: open the `.puml` file and use a PlantUML preview/render extension.

## detailed api

- Foldable class and method-level documentation: [API.md](API.md)

## unit tests

- Local unit tests are in [tests/RaiUtils.Tests](tests/RaiUtils.Tests).
- Run from `RaiUtils` root: `dotnet test RaiUtils.slnx`

## solution format

- Upgraded solution format is available as `RaiUtils.slnx` (generated with `dotnet solution migrate`).
