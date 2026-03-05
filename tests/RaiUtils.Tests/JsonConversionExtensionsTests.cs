using Newtonsoft.Json.Linq;

namespace RaiUtils.Tests;

public class JsonConversionExtensionsTests
{
    [Fact]
    public void ToDictionary_ConvertsNestedObjectsAndArrays()
    {
        var json = JObject.Parse(
            """
            {
              "name": "root",
              "meta": { "enabled": true, "score": 3 },
              "items": [
                { "id": 1, "tags": ["a", "b"] },
                5,
                [9, 10]
              ]
            }
            """);

        var result = json.ToDictionary();

        Assert.Equal("root", result["name"]);

        var meta = Assert.IsAssignableFrom<IDictionary<string, object>>(result["meta"]);
        Assert.Equal(true, meta["enabled"]);
        Assert.Equal(3L, meta["score"]);

        var items = Assert.IsType<object[]>(result["items"]);
        var firstItem = Assert.IsAssignableFrom<IDictionary<string, object>>(items[0]);
        Assert.Equal(1L, firstItem["id"]);

        var tags = Assert.IsType<object[]>(firstItem["tags"]);
        Assert.Equal(new object[] { "a", "b" }, tags);

        Assert.Equal(5L, items[1]);
        var nestedArray = Assert.IsType<object[]>(items[2]);
        Assert.Equal(new object[] { 9L, 10L }, nestedArray);
    }

    [Fact]
    public void ToArray_ConvertsMixedContentRecursively()
    {
        var array = JArray.Parse(
            """
            [
              { "x": 1 },
              [2, { "y": "z" }],
              "tail"
            ]
            """);

        var result = array.ToArray();

        var first = Assert.IsAssignableFrom<IDictionary<string, object>>(result[0]);
        Assert.Equal(1L, first["x"]);

        var second = Assert.IsType<object[]>(result[1]);
        Assert.Equal(2L, second[0]);

        var secondNested = Assert.IsAssignableFrom<IDictionary<string, object>>(second[1]);
        Assert.Equal("z", secondNested["y"]);

        Assert.Equal("tail", result[2]);
    }
}