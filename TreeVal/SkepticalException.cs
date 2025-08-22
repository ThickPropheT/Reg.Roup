namespace TreeVal;

[Serializable]
public class SkepticalException : Exception
{
	private static readonly string DefaultMessage = "I'm skeptical that you could, yet intrigued that you may";

	public SkepticalException() : base(DefaultMessage) { }
	public SkepticalException(string message) : base($"{DefaultMessage}: {message}") { }
	public SkepticalException(Exception inner) : base(DefaultMessage, inner) { }
}
