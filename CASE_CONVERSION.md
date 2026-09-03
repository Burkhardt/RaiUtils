# Word-case conversion and display seams

`RaiUtils.WordCase` converts between PascalCase, lower camelCase, snake_case,
kebab-case, and word arrays. `RaiUtils.StringHelper.WordSeams()` separately
identifies safe soft-wrap positions without altering the supplied text.

## Case conversion

```csharp
using RaiUtils;

var name = new WordCase("San-Diego-State-09.24-212");

name.PascalCase;      // SanDiegoState0924212
name.LowerCamelCase;  // sanDiegoState0924212
name.SnakeCase;       // san_diego_state_09_24_212
name.KebabCase;       // san-diego-state-09-24-212
name.DashCase;        // san-diego-state-09-24-212
```

The string constructor detects mixed separator and camel/Pascal boundaries.
Case conversion intentionally treats separators as normalization hints:

```csharp
"nomsa-concert-167".WordSplit(); // ["nomsa", "concert", "167"]
"nomsa-concert-167".CamelSplit(); // compatibility alias
```

Punctuation is not preserved by `WordSplit`; that behavior is appropriate for
case conversion and must not be used to display an authoritative identifier.

## Lossless display seams

Use `WordSeams()` when a renderer needs break opportunities while preserving
every original character:

```csharp
var value = "DependsOn[ScheduleRehearsal_Nomsa]";
var seams = value.WordSeams(); // [7, 10, 18, 28]
```

The offsets are strictly increasing UTF-16 code-unit indices. Slice the
original value at those offsets; joining the slices always produces the exact
source value. `WordSeams()` does not normalize text, validate an identifier,
define an identifier grammar, or generate HTML.

Browser consumers should render the slices as text nodes separated by real
`<wbr />` elements. Do not insert zero-width characters into identifiers and do
not build untrusted HTML strings.

The normative delimiter set is `_ - . / \\ @ : [ ] ( ) { }`. Delimiters stay
on the left side of the seam. Apostrophes and whitespace do not themselves
create seams. Pascal/camel transitions, acronym endings, and digit-run openings
also create seams according to CR019.

Unicode text elements are indivisible: a seam never divides a surrogate pair
or separates a base character from its combining marks. Uppercase German and
Portuguese characters participate in the same casing rules as ASCII letters.

## Legacy RaiImage source

Before RAIkeep 4.2.6 these types and extensions lived in `RaiImage`. Recompiled
callers add:

```csharp
using RaiUtils;
```

`RaiImage` retains deprecated binary compatibility facades, but their static
methods are deliberately not extension methods. There is only one canonical
extension surface, in RaiUtils.

## Tests

Run the focused coverage from the RaiUtils repository:

```bash
dotnet test tests/RaiUtils.Tests/RaiUtils.Tests.csproj --filter FullyQualifiedName~WordCaseTests
```
