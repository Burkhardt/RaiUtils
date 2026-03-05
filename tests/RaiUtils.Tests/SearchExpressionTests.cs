namespace RaiUtils.Tests;

public class SearchExpressionTests
{
    private sealed class SearchSample
    {
        public string Name { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public string Deleted { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    [Fact]
    public void ConditionsAsString_RoundTrips_WithQuotedValues()
    {
        var expr = new SearchExpression("Name=John* \"hello world\" Deleted=false");

        var serialized = expr.ConditionsAsString;

        Assert.Equal("Name=John* \"hello world\" Deleted=false", serialized);
    }

    [Fact]
    public void IsMatch_ReturnsTrue_ForFieldSpecificWildcard()
    {
        var expr = new SearchExpression("Name=Rain*");
        var obj = new SearchSample { Name = "Rainer" };

        Assert.True(expr.IsMatch(obj));
    }

    [Fact]
    public void IsMatch_ReturnsFalse_WhenNamedFieldDoesNotExist()
    {
        var expr = new SearchExpression("Unknown=abc");
        var obj = new SearchSample { Name = "abc" };

        Assert.False(expr.IsMatch(obj));
    }

    [Fact]
    public void IsMatch_UsesPlusAsAndWithinSingleValueCondition()
    {
        var expr = new SearchExpression("Notes=deleted+2012");
        var obj = new SearchSample { Notes = "the record was deleted in 2012" };

        Assert.True(expr.IsMatch(obj));
    }

    [Fact]
    public void IsMatch_CanMatchAnyProperty_WhenNoFieldNameSpecified()
    {
        var expr = new SearchExpression("*hse24*");
        var obj = new SearchSample { Email = "sales@hse24.com" };

        Assert.True(expr.IsMatch(obj));
    }

    [Fact]
    public void IsMatch_AppliesAndAcrossMultipleConditions()
    {
        var expr = new SearchExpression("Name=Rain* Deleted=false");
        var obj = new SearchSample
        {
            Name = "Rainer Burkhardt",
            Deleted = "false"
        };

        Assert.True(expr.IsMatch(obj));
    }

    [Fact]
    public void IsMatch_ReturnsFalse_IfOneOfMultipleConditionsFails()
    {
        var expr = new SearchExpression("Name=Rain* Deleted=true");
        var obj = new SearchSample
        {
            Name = "Rainer Burkhardt",
            Deleted = "false"
        };

        Assert.False(expr.IsMatch(obj));
    }
}