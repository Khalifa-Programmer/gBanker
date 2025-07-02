using System;
using System.Runtime.Serialization;

namespace Utility.Exceptions
{
    /// <summary>
    /// Represents an error while initializing the domain/sunspot layer
    /// </summary>
    public class InitializationException : BasicException
    {

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="InitializationException"/> class.
        /// </summary>
        public InitializationException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InitializationException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error. </param>
        public InitializationException( string message )
            : base( message )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InitializationException"/> class.
        /// </summary>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception. If the innerException parameter 
        /// is not a null reference, the current exception is raised in a catch block that handles 
        /// the inner exception.
        /// </param>
        public InitializationException( Exception innerException )
            : base( innerException.Message, innerException )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InitializationException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error. </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception. If the innerException parameter 
        /// is not a null reference, the current exception is raised in a catch block that handles 
        /// the inner exception.
        /// </param>
        public InitializationException( string message, Exception innerException )
            : base( message, innerException )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InitializationException"/> class 
        /// with serialized data.
        /// </summary>
        /// <param name="info">
        /// The <see cref="SerializationInfo"/> that holds the serialized object 
        /// data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The <see cref="StreamingContext"/> that contains contextual information about the source or destination.
        /// </param>
        protected InitializationException( SerializationInfo info, StreamingContext context )
            : base( info, context )
        {
        }

        #endregion

    }
}


