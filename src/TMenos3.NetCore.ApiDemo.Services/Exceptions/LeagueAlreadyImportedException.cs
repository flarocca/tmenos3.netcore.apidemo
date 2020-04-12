using System;
using System.Runtime.Serialization;

namespace TMenos3.NetCore.ApiDemo.Services.Exceptions
{
    public class LeagueAlreadyImportedException : Exception
    {
        public LeagueAlreadyImportedException()
        {
        }

        public LeagueAlreadyImportedException(string message) : base(message)
        {
        }

        public LeagueAlreadyImportedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected LeagueAlreadyImportedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
