namespace RaiUtils.Tests;

public class RaiExceptionTests
{
	[Fact]
	public void ToolNotFoundException_IsAStableRaiDomainException()
	{
		var cause = new InvalidOperationException("lookup failed");
		var error = new ToolNotFoundException("PlantUML", "/tools/plantuml", cause);

		Assert.IsAssignableFrom<RaiException>(error);
		Assert.Equal("PlantUML", error.ToolName);
		Assert.Equal("/tools/plantuml", error.ExecutablePath);
		Assert.Same(cause, error.InnerException);
		Assert.Contains("PlantUML", error.Message);
	}
}
