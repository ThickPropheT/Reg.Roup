namespace TreeVal;

[Serializable]
public class SkepticalException : Exception
{
	private static readonly string DefaultMessage = "I'm skeptical that you can, yet intrigued that you may.";

	public SkepticalException() : base(DefaultMessage) { }
	public SkepticalException(Exception inner) : base(DefaultMessage, inner) { }
}
