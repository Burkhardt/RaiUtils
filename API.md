# RaiUtils API Reference

This document provides a detailed, foldable API overview.

## 3.6.1 scope note

- RaiUtils aligns with the `3.6.1` `RAIkeep` package line.
- Patch release: no API changes; corrects publish order relative to `OsLibCore 3.6.1`.

## core types

- <details>
	<summary>Email: regex-based email syntax validator.</summary>

	- Responsibilities: retain original address string and expose quick validity checks.
	- <details>
		<summary>Email(address): create validator for an address string.</summary>

		- Stores the raw `address` for validation and string output.
		</details>
	- <details>
		<summary>Valid: evaluate if address matches the email regex.</summary>

		- Returns `true` when the constructor input passes syntactical validation.
		</details>
	- <details>
		<summary>Invalid: inverse of `Valid`.</summary>

		- Convenience property equivalent to `!Valid`.
		</details>
	- <details>
		<summary>ToString(): return original address value.</summary>

		- Useful when embedding `Email` instance in messages/logging.
		</details>
	</details>

- <details>
	<summary>ParameterDictionary: lowercase-filtered parameter map.</summary>

	- Responsibilities: import only lowercase keys from a `NameValueCollection` into a `StringDictionary`.
	- Base class: `System.Collections.Specialized.StringDictionary`.
	- <details>
		<summary>ParameterDictionary(paramArray): import lowercase entries only.</summary>

		- Iterates `AllKeys` and adds only keys where first char is lowercase.
		</details>
	</details>

- <details>
	<summary>SearchExpression: pattern parser and object matcher.</summary>

	- Responsibilities: parse search expression text and match conditions against object properties.
	- Supported pattern concepts:
		- `*` wildcard segments
		- `+` AND-within-token matching
		- whitespace/comma/`+` as condition separators in parser
		- `field=value` scoped conditions
	- <details>
		<summary>SearchExpression(pattern): parse and store conditions.</summary>

		- Initializes internal condition list through `ConditionsAsString` setter.
		</details>
	- <details>
		<summary>ConditionsAsString: serialize/deserialize parsed conditions.</summary>

		- Getter rebuilds expression text with quote handling for values containing spaces.
		- Setter parses expression text into condition tokens.
		</details>
	- <details>
		<summary>IsMatch(obj): evaluate all conditions against object properties.</summary>

		- For `field=value` conditions, checks only the named property.
		- For single-token conditions, checks all properties until one matches.
		- Returns true only if all conditions succeed.
		</details>
	</details>

## JSON conversion extensions

- <details>
	<summary>JsonConversionExtensions: recursive JSON token conversion.</summary>

	- Responsibilities: convert `JObject`/`JArray` trees into plain dictionary/object[] trees.
	- <details>
		<summary>ToDictionary(json): convert `JObject` to `IDictionary&lt;string, object&gt;` recursively.</summary>

		- Converts direct object values and recursively processes nested objects/arrays.
		</details>
	- <details>
		<summary>ToArray(array): convert `JArray` to `object[]` recursively.</summary>

		- Each entry is normalized to dictionary, array, or scalar value.
		</details>
	</details>

## randomization extensions

- <details>
	<summary>RandomExtensions: convenience random/shuffle methods for collections.</summary>

	- Responsibilities: select random element, shuffle sequences, and sample arbitrary elements.
	- <details>
		<summary>Random&lt;T&gt;(list, rand): random item from enumerable.</summary>

		- Returns `default(T)` for null/empty source.
		</details>
	- <details>
		<summary>Shuffle&lt;T&gt;(source) and Shuffle&lt;T&gt;(source, rand): return shuffled copy.</summary>

		- Converts source to list, shuffles in place, returns resulting sequence.
		</details>
	- <details>
		<summary>Shuffle&lt;T&gt;(list) and Shuffle&lt;T&gt;(list, rand): in-place list shuffle.</summary>

		- Uses Fisher-Yates style index swapping from tail to head.
		</details>
	- <details>
		<summary>TakeAny&lt;T&gt;(source, take): yield random elements by index.</summary>

		- Emits `take` elements by random index selection (duplicates possible).
		</details>
	</details>

## OsLib relationship

- Current RaiUtils source does not inherit from or reference OsLib types directly.
- No OsLib base classes are present in this API surface.

## Shared Cloud Root Contract

- RaiUtils does not discover cloud roots directly.
- When RaiUtils is used alongside OsLib or JsonPit, companion packages should share the same machine-local convention for cloud-backed storage paths.
- Recommended shared convention: `osconfig.json` plus the `cloud` keys `dropbox`, `onedrive`, and `googledrive`.
- On Ubuntu development machines, prefer explicit Google Drive configuration over probe-only behavior so the .NET and Python package families resolve the same root consistently.