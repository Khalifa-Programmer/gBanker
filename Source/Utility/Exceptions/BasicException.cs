using System;
using System.Runtime.Serialization;

namespace Utility.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class BasicException : ApplicationException
    {
        /// <summary>
        /// Gets or sets the name of the service.
        /// </summary>
        /// <value>The name of the service.</value>
        public string ExceptionSource { get; private set; }

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicException"/> class.
        /// </summary>
        public BasicException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error. </param>
        public BasicException( string message )
            : base( message )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicException"/> class.
        /// </summary>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception. If the innerException parameter 
        /// is not a null reference, the current exception is raised in a catch block that handles 
        /// the inner exception.
        /// </param>
        public BasicException( Exception innerException )
            : base( innerException.Message, innerException )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error. </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception. If the innerException parameter 
        /// is not a null reference, the current exception is raised in a catch block that handles 
        /// the inner exception.
        /// </param>
        public BasicException( string message, Exception innerException )
            : base( message, innerException )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicException"/> class 
        /// with serialized data.
        /// </summary>
        /// <param name="info">
        /// The <see cref="SerializationInfo"/> that holds the serialized object 
        /// data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The <see cref="StreamingContext"/> that contains contextual information about the source or destination.
        /// </param>
        protected BasicException( SerializationInfo info, StreamingContext context )
            : base( info, context )
        {
        }
        public BasicException(string errorMessage, Exception exception, string exceptionSource)
            : base(errorMessage, exception)
        {
            ExceptionSource = exceptionSource;
        }

        public override string Message
        {
            get
            {
                return String.Format("A error occurred while trying to process your request ({0}).", base.Message);
            }
        }
        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the hostname.
        /// </summary>
        /// <value>The hostname.</value>
        public string Hostname { get; set; }

        #endregion

    }
}