using System;
using System.Runtime.Serialization;

namespace EDlib
{
    /// <summary>Represents errors that occur because no network is available and no data is cached.</summary>
    [Serializable]
    public class NoNetworkNoCacheException : Exception
    {
        /// <summary>Initializes a new instance of the <see cref="NoNetworkNoCacheException" /> class.</summary>
        public NoNetworkNoCacheException()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="NoNetworkNoCacheException" /> class with a specified error message.</summary>
        /// <param name="message">The message that describes the error.</param>
        public NoNetworkNoCacheException(string message) : base(message)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="NoNetworkNoCacheException" /> class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public NoNetworkNoCacheException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="NoNetworkNoCacheException" /> class with serialised data.</summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo">SerializationInfo</see> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext">StreamingContext</see> that contains contextual information about the source or destination.</param>
        protected NoNetworkNoCacheException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
