using System.Runtime.CompilerServices;

namespace RaiUtils.Tests;

public class WordCaseTests
{
	private const string PascalName = "NomsaConcert167";
	private const string CamelName = "nomsaConcert167";
	private const string SnakeName = "nomsa_concert_167";
	private const string KebabName = "nomsa-concert-167";

	[Theory]
	[InlineData("San-Diego-State-09.24-212", "SanDiegoState0924212", "sanDiegoState0924212", "san_diego_state_09_24_212", "san-diego-state-09-24-212")]
	[InlineData("SD-State-Sony-149", "SDStateSony149", "sdStateSony149", "sd_state_sony_149", "sd-state-sony-149")]
	[InlineData("nomsa-concert-167", PascalName, CamelName, SnakeName, KebabName)]
	[InlineData("Mixed_Snake.AndPascal-and-kebabCase", "MixedSnakeAndPascalAndKebabCase", "mixedSnakeAndPascalAndKebabCase", "mixed_snake_and_pascal_and_kebab_case", "mixed-snake-and-pascal-and-kebab-case")]
	public void StringConstructor_DetectsInputCaseAndConvertsToAllOutputCases(
		string name,
		string expectedPascalCase,
		string expectedLowerCamelCase,
		string expectedSnakeCase,
		string expectedKebabCase)
	{
		var sut = new WordCase(name);

		Assert.Equal(expectedPascalCase, sut.PascalCase);
		Assert.Equal(expectedLowerCamelCase, sut.LowerCamelCase);
		Assert.Equal(expectedSnakeCase, sut.SnakeCase);
		Assert.Equal(expectedKebabCase, sut.KebabCase);
	}

	[Fact]
	public void ArrayConstructor_ConvertsWordsToAllOutputCases()
	{
		var sut = new WordCase(["nomsa", "concert", "167"]);

		Assert.Equal(PascalName, sut.PascalCase);
		Assert.Equal(CamelName, sut.LowerCamelCase);
		Assert.Equal(SnakeName, sut.SnakeCase);
		Assert.Equal(KebabName, sut.KebabCase);
	}

	[Fact]
	public void CompatibilityAliases_RetainExistingBehavior()
	{
		var sut = new WordCase(SnakeName);

		Assert.Equal(CamelName, sut.CamelCaseString);
		Assert.Equal(sut.LowerCamelCase, sut.CamelCaseString);
		Assert.Equal(KebabName, sut.DashCase);
		Assert.Equal(sut.KebabCase, sut.DashCase);
		Assert.Equal(PascalName, sut.String);
		Assert.Equal(["nomsa", "Concert", "11"], "nomsa-Concert_11".WordSplit());
		Assert.Equal("nomsa-Concert_11".WordSplit(), "nomsa-Concert_11".CamelSplit());
	}

	[Theory]
	[MemberData(nameof(NormativeSeamCases))]
	public void WordSeams_ReturnsNormativeUtf16Offsets(
		string value,
		int[] expectedSeams,
		string[] expectedSegments)
	{
		var seams = value.WordSeams();

		Assert.Equal(expectedSeams, seams);
		Assert.Equal(expectedSegments, SliceAt(value, seams));
		Assert.Equal(value, string.Concat(SliceAt(value, seams)));
		Assert.All(seams, seam => Assert.InRange(seam, 1, value.Length - 1));
		Assert.True(seams.Zip(seams.Skip(1), (left, right) => left < right).All(result => result));
	}

	[Fact]
	public void WordSeams_PreservesDecomposedGraphemesAndSurrogatePairs()
	{
		const string decomposed = "Mu\u0308llerO\u0308zdemir";
		const string supplementary = "😀HelloWorld";

		Assert.Equal([7], decomposed.WordSeams());
		Assert.Equal(decomposed, string.Concat(SliceAt(decomposed, decomposed.WordSeams())));
		Assert.DoesNotContain(2, decomposed.WordSeams());
		Assert.DoesNotContain(8, decomposed.WordSeams());

		Assert.Equal([7], supplementary.WordSeams());
		Assert.Equal(supplementary, string.Concat(SliceAt(supplementary, supplementary.WordSeams())));
		Assert.DoesNotContain(1, supplementary.WordSeams());
	}

	[Fact]
	public void StringHelpers_AreExtensionsInRaiUtils()
	{
		var methods = typeof(StringHelper).GetMethods();

		Assert.Contains(methods, method => method.Name == nameof(StringHelper.WordSplit)
			&& method.IsDefined(typeof(ExtensionAttribute), inherit: false));
		Assert.Contains(methods, method => method.Name == nameof(StringHelper.WordSeams)
			&& method.IsDefined(typeof(ExtensionAttribute), inherit: false));
	}

	public static TheoryData<string, int[], string[]> NormativeSeamCases => new()
	{
		{ "", [], [""] },
		{ "x", [], ["x"] },
		{ "ScheduleRehearsal", [8], ["Schedule", "Rehearsal"] },
		{ "AIAWorkbench", [3], ["AIA", "Workbench"] },
		{ "ImageConfig_Yebo", [5, 12], ["Image", "Config_", "Yebo"] },
		{ "ScheduleRehearsal_Nomsa", [8, 18], ["Schedule", "Rehearsal_", "Nomsa"] },
		{ "DependsOn[ScheduleRehearsal_Nomsa]", [7, 10, 18, 28], ["Depends", "On[", "Schedule", "Rehearsal_", "Nomsa]"] },
		{ "Rock-'n'-Roll", [5, 9], ["Rock-", "'n'-", "Roll"] },
		{ "v1.6.4", [1, 3, 5], ["v", "1.", "6.", "4"] },
		{ "SchwäbischHall", [10], ["Schwäbisch", "Hall"] },
		{ "SãoPaulo", [3], ["São", "Paulo"] },
		{ "MüllerÖzdemir", [6], ["Müller", "Özdemir"] },
		{ "GrüßeÜberall", [5], ["Grüße", "Überall"] },
		{ "CoimbraÓbidos", [7], ["Coimbra", "Óbidos"] },
		{ "AçãoRápida", [4], ["Ação", "Rápida"] },
		{ "Version2Beta", [7, 8], ["Version", "2", "Beta"] },
		{ "abc2def", [3], ["abc", "2def"] },
		{ "__doubled__", [1, 2, 10], ["_", "_", "doubled_", "_"] },
		{ "_leading", [1], ["_", "leading"] },
		{ "trailing_", [], ["trailing_"] },
		{ "already spaced", [], ["already spaced"] }
	};

	private static string[] SliceAt(string value, int[] seams)
	{
		if (seams.Length == 0)
			return [value];

		var segments = new List<string>();
		var start = 0;
		foreach (var seam in seams)
		{
			segments.Add(value[start..seam]);
			start = seam;
		}
		segments.Add(value[start..]);
		return segments.ToArray();
	}
}
