using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace RaiUtils;

/// <summary>General-purpose string casing and word-boundary helpers.</summary>
public static class StringHelper
{
	private static readonly HashSet<char> WordSeamSeparators =
	[
		'_', '-', '.', '/', '\\', '@', ':', '[', ']', '(', ')', '{', '}'
	];

	public static string ToTitle(this string anyCase)
	{
		if (string.IsNullOrEmpty(anyCase))
			return anyCase;

		return char.ToUpperInvariant(anyCase[0]) + anyCase.Substring(1).ToLowerInvariant();
	}

	public static string[] CamelSplit(this string anyCase)
	{
		return anyCase.WordSplit();
	}

	public static string[] WordSplit(this string anyCase)
	{
		return new WordCase(anyCase).Array;
	}

	/// <summary>
	/// Returns strictly increasing UTF-16 offsets where the original string may be
	/// broken for display without changing or normalizing any source character.
	/// </summary>
	public static int[] WordSeams(this string value)
	{
		if (string.IsNullOrEmpty(value) || value.Length < 2)
			return [];

		var elements = TextElements(value);
		if (elements.Count < 2)
			return [];

		var seams = new List<int>();
		for (var index = 1; index < elements.Count; index++)
		{
			var previous = elements[index - 1];
			var current = elements[index];
			var next = index + 1 < elements.Count ? elements[index + 1] : (TextElement?)null;

			var followsSeparator = previous.IsSeparator;
			var pascalSeam = current.IsUpper && (previous.IsLower || previous.IsDigit);
			var acronymSeam = current.IsUpper && previous.IsUpper && next is { IsLower: true };
			var digitSeam = current.IsDigit && (previous.IsUpper || previous.IsLower);

			if (followsSeparator || pascalSeam || acronymSeam || digitSeam)
				seams.Add(current.Offset);
		}

		return seams.ToArray();
	}

	private static List<TextElement> TextElements(string value)
	{
		var elements = new List<TextElement>();
		var enumerator = StringInfo.GetTextElementEnumerator(value);
		while (enumerator.MoveNext())
		{
			var offset = enumerator.ElementIndex;
			var text = enumerator.GetTextElement();
			var category = CharUnicodeInfo.GetUnicodeCategory(value, offset);
			var separator = text.Length == 1 && WordSeamSeparators.Contains(text[0]);
			elements.Add(new TextElement(offset, category, separator));
		}
		return elements;
	}

	private readonly record struct TextElement(
		int Offset,
		UnicodeCategory Category,
		bool IsSeparator)
	{
		public bool IsUpper => Category == UnicodeCategory.UppercaseLetter;
		public bool IsLower => Category == UnicodeCategory.LowercaseLetter;
		public bool IsDigit => Category == UnicodeCategory.DecimalDigitNumber;
	}
}

/// <summary>Converts between token arrays and common identifier case conventions.</summary>
public class WordCase
{
	private static readonly Regex CamelOrPascalWordRegex = new(@"[\p{Lu}]+(?=[\p{Lu}][\p{Ll}]|\b)|[\p{Lu}]?[\p{Ll}]+|\d+", RegexOptions.Compiled);
	private static readonly Regex SeparatorRegex = new(@"[^\p{L}\p{Nd}]+", RegexOptions.Compiled);
	private string[] array;

	public string[] Array
	{
		get => array;
		set => array = CleanWords(value);
	}

	public string String
	{
		get => PascalCase;
		set => array = SplitAnyCase(value);
	}

	public string PascalCase => string.Concat(array.Select(FormatPascalWord));

	public string CamelCaseString
	{
		get
		{
			if (array.Length == 0)
				return string.Empty;

			return FormatCamelFirstWord(array[0]) + string.Concat(array.Skip(1).Select(FormatPascalWord));
		}
	}

	public string LowerCamelCase => CamelCaseString;
	public string SnakeCase => string.Join("_", array.Select(FormatSnakeWord));
	public string KebabCase => string.Join("-", array.Select(FormatSnakeWord));
	public string DashCase => KebabCase;

	public WordCase(string[] words)
	{
		Array = words;
	}

	public WordCase(string anyCase)
	{
		String = anyCase;
	}

	private static string[] SplitAnyCase(string value)
	{
		if (string.IsNullOrWhiteSpace(value))
			return [];

		return CleanWords(SeparatorRegex
			.Split(value)
			.SelectMany(SplitCamelOrPascalCase));
	}

	private static string[] SplitCamelOrPascalCase(string value)
	{
		if (string.IsNullOrWhiteSpace(value))
			return [];

		return CleanWords(CamelOrPascalWordRegex.Matches(value).Select(match => match.Value));
	}

	private static string[] CleanWords(IEnumerable<string> words)
	{
		return words
			.Where(word => !string.IsNullOrWhiteSpace(word))
			.Select(word => word.Trim())
			.ToArray();
	}

	private static string FormatPascalWord(string word)
	{
		if (char.IsDigit(word[0]) || IsAllUppercaseWord(word))
			return word;

		return word.ToTitle();
	}

	private static string FormatCamelFirstWord(string word)
	{
		return char.IsDigit(word[0]) ? word : word.ToLowerInvariant();
	}

	private static string FormatSnakeWord(string word)
	{
		return word.ToLowerInvariant();
	}

	private static bool IsAllUppercaseWord(string word)
	{
		return word.Any(char.IsLetter)
			&& word.Where(char.IsLetter).All(char.IsUpper);
	}
}
