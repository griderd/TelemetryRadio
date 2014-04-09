using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GSLib.Collections
{
    /// <summary>
    /// Represents a contiguous range of indices by using an offset and length.
    /// </summary>
    /// VB: Public Structure RangeInfo
    public struct RangeInfo
    {
        /***********************************************************************
         * Private Fields
         **********************************************************************/
        int offset;
        int length;

        /***********************************************************************
         * Constructors
         **********************************************************************/
        /// <summary>
        /// Creates a new RangeInfo instance.
        /// </summary>
        /// <param name="offset">Offset of the range.</param>
        /// <param name="length">Length of the range.</param>
        /// <exception cref="IndexOutOfRangeException">Thrown when the offset given is less than zero.</exception>
        /// <exception cref="BoundaryOutOfRangeException">Thrown when the length given is less than 1.</exception>
        /// VB: Public New(ByVal offset As Integer, ByVal length As Integer)
        public RangeInfo(int offset, int length)
        {
            if (offset >= 0)
                this.offset = offset;
            else
                throw new IndexOutOfRangeException("Offset must be non-negative.");

            if (length > 0)
                this.length = length;
            else
                throw new BoundaryOutOfRangeException("Length must be greater than zero.");
        }

        /***********************************************************************
         * Properties
         **********************************************************************/
        /// <summary>
        /// Gets the offset of the range.
        /// </summary>
        /// VB: Public ReadOnly Property Offset As Integer
        public int Offset
        {
            get
            {
                return offset;
            }
            private set
            {
                if (value >= 0)
                    offset = value;
                else
                    throw new IndexOutOfRangeException("Offset must be non-negative.");
            }
        }

        /// <summary>
        /// Gets the length of the range.
        /// </summary>
        /// VB: Public ReadOnly Property Length As Integer
        public int Length
        {
            get
            {
                return length;
            }
            private set
            {
                if (value > 0)
                    length = value;
                else
                    throw new BoundaryOutOfRangeException("Length must be greater than zero.");
            }
        }

        /// <summary>
        /// Gets the index representing the upper boundary of the range.
        /// </summary>
        /// VB: Public ReadOnly Property UpperBoundary As Integer
        public int UpperBoundary
        {
            get
            {
                return length + offset - 1;
            }
        }

        /***********************************************************************
         * Operators
         **********************************************************************/
        /// <summary>
        /// Increments the offset by 1.
        /// </summary>
        /// <param name="range">Range to increment.</param>
        /// <returns>Returns the updated range.</returns>
        /// <example>
        /// public void Main(string[] args)
        /// {
        ///     RangeInfo r = new RangeInfo(5, 10);
        ///     r++;
        ///     // r.Offset is now 6
        ///     // r.Length is still 10
        /// }
        /// </example>
        public static RangeInfo operator ++(RangeInfo range)
        {
            try
            {
                range.Offset++;
            }
            catch
            {
                throw;
            }
            return range;
        }

        /// <summary>
        /// Increases the offset by the given amount.
        /// </summary>
        /// <param name="range">Range to change</param>
        /// <param name="offset">Amount to increase the offset by</param>
        /// <returns>Returns the updated range.</returns>
        /// <example>
        /// public void Main(string[] args)
        /// {
        ///     RangeInfo r = new RangeInfo(5, 10);
        ///     r = r + 6;
        ///     // r.Offset is now 11
        ///     // r.Length is still 10
        /// }
        /// </example>
        public static RangeInfo operator +(RangeInfo range, int offset)
        {
            try
            {
                range.Offset = range.Offset + offset;
            }
            catch
            {
                throw;
            }
            return range;
        }

        /// <summary>
        /// Decreases the offset by the given amount.
        /// </summary>
        /// <param name="range">Range to change</param>
        /// <param name="offset">Amount to decrease the offset by</param>
        /// <returns>Returns the updated range.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown if offset becomes less than zero.</exception>
        /// <example>
        /// public void Main(string[] args)
        /// {
        ///     RangeInfo r = new RangeInfo(5, 10);
        ///     r = r - 4;
        ///     // r.Offset is now 1
        ///     // r.Length is still 10
        /// }
        /// </example>
        public static RangeInfo operator -(RangeInfo range, int offset)
        {
            try
            {
                range.Offset = range.Offset - offset;
            }
            catch
            {
                throw;
            }
            return range;
        }

        /// <summary>
        /// Decrements the offset by 1.
        /// </summary>
        /// <param name="range">Range to increment.</param>
        /// <returns>Returns the updated range.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown if offset becomes less than zero.</exception>
        /// <example>
        /// public void Main(string[] args)
        /// {
        ///     RangeInfo r = new RangeInfo(5, 10);
        ///     r--;
        ///     // r.Offset is now 4
        ///     // r.Length is still 10
        /// }
        /// </example>
        public static RangeInfo operator --(RangeInfo range)
        {
            try
            {
                range.Offset--;
            }
            catch
            {
                throw;
            }
            return range;
        }

        /***********************************************************************
         * Methods
         **********************************************************************/

        /// <summary>
        /// Increases the length of the range by the indicated amount.
        /// </summary>
        /// <param name="amount">Amount to increase the length by.</param>
        /// VB: Public Sub Grow(ByVal amount As Integer)
        public void Grow(int amount)
        {
            try
            {
                Length = Length + amount;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Decreases the length of the range by the indicated amount.
        /// </summary>
        /// <param name="amount">Amount to decrease the length by.</param>
        /// <exception cref="BoundaryOutOfRangeException">Thrown if the length becomes less than 1.</exception>
        /// Public Sub Shrink(ByVal amount As Integer)
        public void Shrink(int amount)
        {
            try
            {
                Length = Length - amount;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Determines if the given index is within the RangeInfo indices.
        /// </summary>
        /// <param name="index">Index to check.</param>
        /// <returns>Returns true when the index is within the range, otherwise returns false.</returns>
        /// VB: Public Function IsInRange(ByVal index As Integer) As Boolean
        public bool IsInRange(int index)
        {
            return ((index >= offset) && (index <= UpperBoundary));
        }

        /// <summary>
        /// Creates a Range instance from this RangeInfo and a given source list.
        /// </summary>
        /// <typeparam name="T">Type of items to hold in the Range.</typeparam>
        /// <param name="list">IList&lt;T&gt; that contains the source items.</param>
        /// <param name="clone">Indicates whether to clone the source list.</param>
        /// <returns>Returns a new instance of the Range class.</returns>
        /// <exception cref="BoundaryOutOfRangeException">Occurs when the upper boundary of the range exceeds the index of the last item of the source collection. This would
        /// happen if the length given was too long.</exception>
        /// <exception cref="IndexOutOfRangeException">Occurs if the offset given is less than zero.</exception>
        /// <example>
        /// Public Sub Main(string[] args)
        /// {
        ///     RangeInfo ri = new RangeInfo(2, 3);
        ///     int[] src = new int[] { 64, 74, 87, 461, 897, 441, 315 };
        ///     Range&lt;int&gt; r = ri.MakeRange&lt;int&gt;(src);
        ///     
        ///     foreach (int i in r)
        ///     {
        ///         Console.WriteLine(i);
        ///     }
        /// }
        /// 
        /// Console Output:
        /// 87
        /// 461
        /// 897
        /// </example>
        /// VB: Public Function MakeRange(Of T)(ByVal list As IList(Of T), Optional ByVal clone As Boolean = False) As Range(Of T)
        public Range<T> MakeRange<T>(IList<T> list, bool clone = false)
        {
            try
            {
                return new Range<T>(this, list, clone);
            }
            catch
            {
                throw;
            }
        }
    }
}
