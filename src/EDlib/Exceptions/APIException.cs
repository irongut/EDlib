using EDlib.Platform;
using System;
using System.Runtime.Serialization;

namespace EDlib
{
    /// <summary>Represents errors from an API.</summary>
    [Preserve(AllMembers = true)]
    [Serializable]
    public class APIException : Exception
    {
        /// <summary>Gets the error code from the API.</summary>
        /// <value>The error code.</value>
        public int? StatusCode { get; }

        /// <summary>Initializes a new instance of the <see cref="APIException" /> class.</summary>
        public APIException()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="APIException" /> class with a specified error message.</summary>
        /// <param name="message">The message that describes the error.</param>
        public APIException(string message) : base(message)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="APIException" /> class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public APIException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="APIException" /> class with serialised data.</summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo">SerializationInfo</see> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext">StreamingContext</see> that contains contextual information about the source or destination.</param>
        protected APIException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="APIException" /> class with a specified error message and error code.</summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="code">The error code from the API.</param>
        public APIException(string message, int? code) : base(message)
        {
            StatusCode = code;
        }

        /// <summary>Initializes a new instance of the <see cref="APIException" /> class with a specified error message, error code and a reference to the inner exception that is the cause of this exception.</summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        /// <param name="code">The error code from the API.</param>
        public APIException(string message, Exception innerException, int? code) : base(message, innerException)
        {
            StatusCode = code;
        }
    }
}
