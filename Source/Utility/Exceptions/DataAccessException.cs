
using System;

namespace Utility.Exceptions
{
    public class DataAccessException : BasicException
    {        
        /// <summary>
        /// Initializes a new instance of the <see cref="DataAccessException"/> class.
        /// </summary>
        public DataAccessException() {}

        /// <summary>
        /// Initializes a new instance of the <see cref="DataAccessException"/> class.
        /// </summary>
        public DataAccessException(string message)
            : base(message)
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataAccessException"/> class.
        /// </summary>
        public DataAccessException(string errorMessage, Exception exception)
            : base(errorMessage, exception)
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataAccessException"/> class.
        /// </summary>
        public DataAccessException(string errorMessage, Exception exception, string exceptionSource)
            : base(errorMessage, exception, exceptionSource)
        {            
        }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value>The message.</value>
        public override string Message
        {
            get
            {
                return ExceptionSource != "" ? string.Format("{0}: {1}", ExceptionSource, base.Message) : base.Message;
            }
        }
    }
}
