using System.Collections.Specialized;

namespace RaiUtils.Tests;

public class ParameterDictionaryTests
{
	[Fact]
	public void Constructor_ImportsOnlyLowercaseKeys()
	{
		var source = new NameValueCollection
		{
			["alpha"] = "1",
			["Beta"] = "2",
			["gamma"] = "3"
		};

		var dict = new ParameterDictionary(source);

		Assert.Equal("1", dict["alpha"]);
		Assert.Equal("3", dict["gamma"]);
		Assert.Null(dict["Beta"]);
		Assert.Equal(2, dict.Count);
	}
}