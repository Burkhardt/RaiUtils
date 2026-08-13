using System;

namespace RaiUtils
{
	/// <summary>Base exception for failures reported through RAIkeep domain APIs.</summary>
	public class RaiException : Exception
	{
		public RaiException(string message) : base(message)
		{
		}

		public RaiException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}

	/// <summary>Thrown when a required external command-line tool cannot be located.</summary>
	public sealed class ToolNotFoundException : RaiException
	{
		public ToolNotFoundException(string toolName, string executablePath)
			: base($"Required tool '{toolName}' was not found at '{executablePath}'.")
		{
			ToolName = toolName;
			ExecutablePath = executablePath;
		}

		public ToolNotFoundException(string toolName, string executablePath, Exception innerException)
			: base($"Required tool '{toolName}' was not found at '{executablePath}'.", innerException)
		{
			ToolName = toolName;
			ExecutablePath = executablePath;
		}

		public string ToolName { get; }
		public string ExecutablePath { get; }
	}
}
