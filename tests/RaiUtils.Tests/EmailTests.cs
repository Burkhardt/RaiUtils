namespace RaiUtils.Tests;

public class EmailTests
{
    [Theory]
    [InlineData("test@example.com")]
    [InlineData("first.last+tag@contoso.co")]
    public void Valid_IsTrue_ForSyntacticallyValidAddress(string address)
    {
        var email = new Email(address);

        Assert.True(email.Valid);
        Assert.False(email.Invalid);
    }

    [Theory]
    [InlineData("not-an-email")]
    [InlineData("no-at-sign.example.com")]
    [InlineData("foo@")]
    public void Valid_IsFalse_ForInvalidAddress(string address)
    {
        var email = new Email(address);

        Assert.False(email.Valid);
        Assert.True(email.Invalid);
    }

    [Fact]
    public void ToString_ReturnsOriginalAddress()
    {
        const string address = "mailbox@domain.org";
        var email = new Email(address);

        Assert.Equal(address, email.ToString());
    }
}