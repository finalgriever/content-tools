namespace FinalGriever.ContentTools.PrepReplays
{
    public class UserException : System.Exception
    {
        public UserException() { }
        public UserException(string message) : base(message) { }
        public UserException(string message, System.Exception inner) : base(message, inner) { }
        protected UserException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}