using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GSLib.Collections
{
    /// <summary>
    /// Represents an exception that occurs when a range boundary exceeds the limits of its source.
    /// </summary>
    /// VB: Public Class BoundaryOutOfRangeException
    ///         Inherits Exception
    public class BoundaryOutOfRangeException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the BoundaryOutOfRangeException class.
        /// </summary>
        /// VB: Public Sub New()
        public BoundaryOutOfRangeException()
            : base()
        { }

        /// <summary>
        /// Initializes a new instance of the BoundaryOutOfRangeException class with the specified message.
        /// </summary>
        /// <param name="message">Message to use.</param>
        /// VB: Public Sub New(ByVal message As String)
        public BoundaryOutOfRangeException(string message)
            : base(message)
        { }
    }
}
