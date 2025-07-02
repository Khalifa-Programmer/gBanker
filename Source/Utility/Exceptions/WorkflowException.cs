using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utility.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    public class WorkflowException : BasicException
    {        
        /// <summary>
        /// Initializes a new instance of the <see cref="WorkflowException"/> class.
        /// </summary>
        public WorkflowException() {}

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkflowException"/> class.
        /// </summary>
        /// <param name="className">Name of the class.</param>
        public WorkflowException(string errorMessage)
            : base(errorMessage)
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkflowException"/> class.
        /// </summary>
        /// <param name="className">Name of the class.</param>
        /// <param name="errorMessage">The error message.</param>
        public WorkflowException(string errorMessage, Exception innerException)
            : base(errorMessage, innerException)
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkflowException"/> class.
        /// </summary>
        /// <param name="className">Name of the class.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="innerException">The inner exception.</param>
        public WorkflowException(string errorMessage, Exception innerException,string exceptionSource)
            : base(errorMessage, innerException, exceptionSource)
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
                if (ExceptionSource != "")
                {
                    return string.Format("{0}: {1}", ExceptionSource, base.Message);
                }
                return base.Message;
            }
        }
    }
}
