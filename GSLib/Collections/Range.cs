using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GSLib.Collections
{
    /// <summary>
    /// Represents a contiguous sub-collection of items contained in an IList object.
    /// </summary>
    /// <typeparam name="T">Type of item</typeparam>
    /// VB: Public Class Range(Of T)
    ///         Inherits IEnumerable(Of T)
    public class Range<T> : IEnumerable<T>
    {
        #region Fields
        /***********************************************************************
         * Fields
         **********************************************************************/

        RangeInfo rangeInfo;    // contains the underlying RangeInfo object
        IList<T> source;        // contains the source list
        T[] values;             // contains the range values

        #endregion

        #region Events
        /***********************************************************************
         * Events
         **********************************************************************/
        /// <summary>
        /// Event listener. Is raised when the RangeInfo portion of the object is changed.
        /// </summary>
        private event EventHandler ChangedEvent;

        /// <summary>
        /// Initializes the event listeners.
        /// </summary>
        private void InitializeEventListener()
        {
            ChangedEvent += new EventHandler(RangeChanged);
        }

        #endregion

        #region EventHandlers
        /***********************************************************************
         * Event Handlers
         **********************************************************************/
        /// <summary>
        /// Handles the ChangedEvent event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="BoundaryOutOfRangeException">Occurs when the upper boundary of the range exceeds the index of the last item of the source collection.</exception>
        private void RangeChanged(object sender, EventArgs e)
        {
            try
            {
                CalculateValues();
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region Constructors
        /***********************************************************************
         * Constructors
         **********************************************************************/

        /// <summary>
        /// Creates a new range with the given offset and length, drawing items from the given IList.
        /// </summary>
        /// <param name="offset">Index offset from which to start the range.</param>
        /// <param name="length">Number of items to include in the range.</param>
        /// <param name="source">Source of items.</param>
        /// <param name="cloneSource">Determines whether or not to copy the IList.</param>
        /// <exception cref="BoundaryOutOfRangeException">Occurs when the upper boundary of the range exceeds the index of the last item of the source collection. This would
        /// happen if the length given was too long.</exception>
        /// <exception cref="IndexOutOfRangeException">Occurs if the offset given is less than zero.</exception>
        /// <remarks>If cloneSource is set to false (its default value), the source will be recorded as a reference, and any changes to the source will also affect
        /// values in the range. Also, any range value changes will affect the original source object. Conversely, if closeSource is set to true, all of the values
        /// contained in the provided source are copied, and changes made to the original source or the range values will not affect each other.</remarks>
        /// VB: Public Sub New(ByVal offset As Integer, ByVal length As Integer, ByVal source As IList(Of T), Optional ByVal cloneSource As Boolean = False)
        public Range(int offset, int length, IList<T> source, bool cloneSource = false)
        {
            try
            {
                rangeInfo = new RangeInfo(offset, length);

                if (cloneSource)
                    this.source = new List<T>(source.ToArray());
                else
                    this.source = source;

                InitializeEventListener();
                ChangedEvent(this, new EventArgs());
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Creates a new range based off of the given RangeInfo object and drawing items from the given IList.
        /// </summary>
        /// <param name="range">RangeInfo object to use for offset and length.</param>
        /// <param name="source">Source of items.</param>
        /// <param name="cloneSource">Determines whether or not to copy the IList.</param>
        /// <exception cref="BoundaryOutOfRangeException">Occurs when the upper boundary of the range exceeds the index of the last item of the source collection. This would
        /// happen if the length given was too long.</exception>
        /// <remarks>If cloneSource is set to false (its default value), the source will be recorded as a reference, and any changes to the source will also affect
        /// values in the range. Also, any range value changes will affect the original source object. Conversely, if closeSource is set to true, all of the values
        /// contained in the provided source are copied, and changes made to the original source or the range values will not affect each other.</remarks>
        /// VB: Public Sub New(ByVal range As RangeInfo, ByVal source As IList(Of T), Optional ByVal cloneSource As Boolean = False)
        public Range(RangeInfo range, IList<T> source, bool cloneSource = false)
        {
            rangeInfo = range;
            this.source = source;

            try
            {
                CalculateValues();
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region Properties
        /***********************************************************************
         * Properties
         **********************************************************************/
        /// <summary>
        /// Gets the length of the range.
        /// </summary>
        /// <exception cref="BoundaryOutOfRangeException">Thrown if the upper boundary exceeds the source Length, or Length is less than 1.</exception>
        /// VB: Public Property Length As Integer
        public int Length
        {
            get
            {
                return rangeInfo.Length;
            }
            set
            {
                try
                {
                    rangeInfo = new RangeInfo(rangeInfo.Offset, value);
                }
                catch
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets the offset of the range.
        /// </summary>
        /// <exception cref="BoundaryOutOfRangeException">Thrown if the upper boundary of Info exceeds the source length.</exception>
        /// <exception cref="IndexOutOfRangeException">Thrown if the offset is less than zero.</exception>
        /// VB: Public Property Offset As Integer
        public int Offset
        {
            get
            {
                return rangeInfo.Offset;
            }
            set
            {
                try
                {
                    rangeInfo = new RangeInfo(value, rangeInfo.Length);
                }
                catch
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets or sets the RangeInfo that describes the range parameters.
        /// </summary>
        /// <exception cref="BoundaryOutOfRangeException">Thrown if the upper boundary of Info exceeds the source length.</exception>
        /// VB: Public ReadOnly Property Info As RangeInfo
        public RangeInfo Info
        {
            get
            {
                return rangeInfo;
            }
            set
            {
                if (value.UpperBoundary < source.Count)
                {
                    rangeInfo = value;
                    try
                    {
                        ChangedEvent.Invoke(this, new EventArgs());
                    }
                    catch
                    {
                        throw;
                    }
                }
                else
                {
                    throw new BoundaryOutOfRangeException("The upper boundary of the Info exceeds Length.");
                }
            }
        }

        /// <summary>
        /// Gets the upper boundary of the range, which is the last index of the original IList that is represented by the range.
        /// </summary>
        /// VB: Public ReadOnly Property UpperBoundary As Integer
        public int UpperBoundary
        {
            get
            {
                return rangeInfo.UpperBoundary;
            }
        }

        /// <summary>
        /// Gets or sets an item value.
        /// </summary>
        /// <param name="index">Zero-based index of the value contained in the range.</param>
        /// <exception cref="IndexOutOfRangeException">Thrown if the index is less than zero or greater than the last index.</exception>
        /// <returns>Returns an item of type T containing the requested value.</returns>
        /// VB: Public Property Item(ByVal index As Integer) As T
        public T this[int index]
        {
            get
            {
                try
                {
                    CalculateValues();
                }
                catch
                {
                    throw;
                }
                if ((index < 0) | (index > values.Length - 1)) throw new IndexOutOfRangeException();
                return values[index];
            }
            set
            {
                try
                {
                    CalculateValues();
                }
                catch
                {
                    throw;
                }
                if ((index < 0) | (index > values.Length - 1)) throw new IndexOutOfRangeException();
                values[index] = value;
                source[index + Offset] = value;
            }
        }

        /// <summary>
        /// Gets an array containing all of the values contained in the Range.
        /// </summary>
        /// <exception cref="BoundaryOutOfRangeException">Occurs when the upper boundary of the range exceeds the index of the last item of the source collection.</exception>
        /// VB: Public ReadOnly Property Values As T()
        public T[] Values
        {
            get
            {
                try
                {
                    CalculateValues();
                }
                catch
                {
                    throw;
                }
                return values;
            }
        }

        #endregion

        #region Public Methods
        /***********************************************************************
         * Public Methods
         **********************************************************************/
        /// <summary>
        /// Determines whether the Range contains a specific value.
        /// </summary>
        /// <param name="value">Value to check for.</param>
        /// <returns>Returns true if the Range contains the value, otherwise returns false.</returns>
        /// VB: Public Function Contains(ByVal value As T) As Boolean
        public bool Contains(T value)
        {
            for (int i = 0; i < values.Length; i++) // linear search values
            {
                if (value.Equals(values[i])) return true;   // if match found, return true
            }
            return false;   // if no match found, return false
        }

        /// <summary>
        /// Increases the length of the Range by the indicated amount.
        /// </summary>
        /// <param name="amount">Amount to increase length by</param>
        /// <exception cref="BoundaryOutOfRangeException">Thrown if the UpperBoundary exceeds the highest index of the source file, or Length becomes less than 1.</exception>
        /// VB: Public Sub Grow(ByVal amount As Integer)
        public void Grow(int amount)
        {
            try
            {
                rangeInfo.Grow(amount);
                ChangedEvent.Invoke(this, new EventArgs());
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Decreases the length of the Range by the indicated amount.
        /// </summary>
        /// <param name="amount">Amount to decrease length by.</param>
        /// <exception cref="BoundaryOutOfRangeException">Thrown if the UpperBoundary exceeds the highest index of the source file, or Length becomes less than 1.</exception>
        /// VB: Public Sub Shrink(ByVal amount As Integer)
        public void Shrink(int amount)
        {
            try
            {
                rangeInfo.Shrink(amount);
                ChangedEvent.Invoke(this, new EventArgs());
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Retrieves at most the requested number of items from the source and increments the Range by that amount.
        /// </summary>
        /// <param name="count">Number of items to retrieve.</param>
        /// <param name="array">Array to store items.</param>
        /// <returns>Returns true if at least one item is retrieved. Otherwise returns false.</returns>
        /// <exception cref="ArgumentException">Thrown if count is less than one.</exception>
        /// <remarks>If more items are requested than can be retrieved, the maximum number of items is retrieved. You should not assume 
        /// that the number of items retrieved will match the number requested.</remarks>
        /// VB: Public Function Get(ByVal count As Integer, ByRef array() As T) As Boolean
        public bool Get(int count, out T[] array)
        {
            if (count <= 0) throw new ArgumentException();
            int c = count;
            if (Offset + count > source.Count) c = source.Count - Offset;

            if (c > 0)
            {
                Info = new RangeInfo(Offset, c);
                array = Values;
                if (Offset + c < source.Count) Info = new RangeInfo(Offset + c, 1);
                return true;
            }
            else
            {
                array = new T[0];
                return false;
            }
        }

        /// <summary>
        /// Retrieves the next individual item and increments the Range by one.
        /// </summary>
        /// <param name="item">Place to store the item.</param>
        /// <returns>Returns true if an item was returned. Otherwise, returns false.</returns>
        public bool GetNext(out T item)
        {
            T[] returnedItem;

            bool result = Get(1, out returnedItem);
            item = returnedItem[0];
            return result;
        }

        /// <summary>
        /// Gets the next block of at most the current Length and increments by that length.
        /// </summary>
        /// <param name="items">Place to store the retreived items.</param>
        /// <returns>Returns true if at least one item was returned. Otherwise, returns false.</returns>
        public bool GetNextBlock(out T[] items)
        {
            return Get(Length, out items);
        }

        /// <summary>
        /// Determines if the given index is within the Range indices.
        /// </summary>
        /// <param name="index">Index to check.</param>
        /// <returns>Returns true when the index is within the range, otherwise returns false.</returns>
        /// VB: Public Function IsInRange(ByVal index As Integer) As Boolean
        public bool IsInRange(int index)
        {
            return rangeInfo.IsInRange(index);
        }

        /// <summary>
        /// Gets the enumerator of the values.
        /// </summary>
        /// <returns>Returns an IEnumerator for the values.</returns>
        /// VB: Public Function GetEnumerator() As IEnumerator(Of T)
        public IEnumerator<T> GetEnumerator()
        {
            // iterate through the values array.
            for (int i = 0; i < values.Length; i++)
            {
                yield return values[i];
            }
        }

        /// <summary>
        /// Non-Generic IEnumerator.
        /// </summary>
        /// <returns></returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Operators
        /***********************************************************************
         * Operators
         **********************************************************************/
        /// <summary>
        /// Increments offset by 1. Decreases length if nessesary.
        /// </summary>
        /// <param name="range"></param>
        /// <returns></returns>
        public static Range<T> operator ++(Range<T> range)
        {
            if (range.UpperBoundary + 1 < range.source.Count)
            {
                range.Offset++;
            }
            else if (range.Length > 1)
            {
                range.Length--;
                range.Offset++;
            }
            return range;
        }

        /// <summary>
        /// Decrements offset by 1.
        /// </summary>
        /// <param name="range"></param>
        /// <returns></returns>
        public static Range<T> operator --(Range<T> range)
        {
            if (range.Offset > 0)
            {
                range.Offset--;
            }
            return range;
        }

        /// <summary>
        /// Increments offset by the given amount. Decreases length if nessesary.
        /// </summary>
        /// <param name="range"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static Range<T> operator +(Range<T> range, int offset)
        {
            if (offset > 0)
            {
                if (range.UpperBoundary + offset < range.source.Count)
                {
                    range.Offset += offset;
                }
                else if (range.Length > offset)
                {
                    range.Length -= offset;
                    range.Offset += offset;
                }
            }
            else
            {
                if (range.UpperBoundary + offset > range.source.Count)
                {
                    range.Offset += offset;
                }
                else if (range.Length < offset)
                {
                    range.Length -= offset;
                    range.Offset += offset;
                }
            }
            return range;
        }

        /// <summary>
        /// Decrements offset by the given amount.
        /// </summary>
        /// <param name="range"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static Range<T> operator -(Range<T> range, int offset)
        {
            if ((offset > 0) && (range.Offset >= offset))
            {
                range.Offset -= offset;
            }
            else if (range.Offset <= offset)
            {
                range.Offset -= offset;
            }
            return range;
        }

        #endregion

        #region Private Methods
        /***********************************************************************
         * Private Methods
         **********************************************************************/
        /// <summary>
        /// Updates the values array by drawing values from the source collection.
        /// </summary>
        /// <exception cref="BoundaryOutOfRangeException">Occurs when the upper boundary of the range exceeds the index of the last item of the source collection.</exception>
        private void CalculateValues()
        {
            List<T> newValues = new List<T>();

            if (UpperBoundary <= source.Count - 1)  // check if the upper boundary exceeds the max source index
            {
                T[] src = source.ToArray();         // get an array from source

                for (int i = 0; i < Length; i++)    // for each item of the length
                {
                    newValues.Add(src[i + Offset]); // get the item at the index + offset
                }
                values = newValues.ToArray();       // replace the values array
            }
            else
            {
                throw new BoundaryOutOfRangeException("Range upper boundary exceeds source collection upper boundary.");
            }
        }
        #endregion
    }
}
