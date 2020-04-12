using System;
using System.Runtime.Serialization;

namespace TMenos3.NetCore.ApiDemo.Services.Exceptions
{
    public class LeagueDoesNotExistException : Exception
    {
        public LeagueDoesNotExistException()
        {
        }

        public LeagueDoesNotExistException(string message) : base(message)
        {
        }

        public LeagueDoesNotExistException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected LeagueDoesNotExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
