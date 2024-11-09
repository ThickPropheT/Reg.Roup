using System;

namespace Reg.Roup
{

    [Serializable]
	public class SkepticalException : Exception
	{
		private static string DefaultMessage = "I'm skeptical that you can, yet intrigued that you may.";

		public SkepticalException() : base(DefaultMessage) { }
		public SkepticalException(Exception inner) : base(DefaultMessage, inner) { }
		protected SkepticalException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
