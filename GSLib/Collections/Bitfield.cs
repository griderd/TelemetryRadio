using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GSLib.Collections
{
    /// <summary>
    /// Represents an array of Boolean values that can be easily modified.
    /// </summary>
    public class Bitfield : IList<bool>
    {
        #region Private Fields
        /***********************************************************************
         * Private Fields
         **********************************************************************/
        List<bool> bits;                // stores the bits of the bitfield
        Endianness endianness;      // stores the endiness of the bitfield

        #endregion

        #region Enumerations
        /***********************************************************************
         * Enumerations
         **********************************************************************/
        /// <summary>
        /// Endianness options for the Bitfield class. LittleEndian is default,
        /// and determines that the left-most (zero-index) bit is the least-significant.
        /// BigEndian determines that the left-most (zero-index) bit is the most-significant.
        /// </summary>
        /// 
        /// VB:
        /// Public Enum Endianness
        public enum Endianness
        {
            /// <summary>
            /// Represents a little-endian bitfield
            /// </summary>
            LittleEndian,
            /// <summary>
            /// Represents a big-endian bitfield
            /// </summary>
            BigEndian
        }
        #endregion

        #region Constructors
        /***********************************************************************
         * Constructors
         **********************************************************************/

        /// <summary>
        /// Instantiates a new Bitfield object.
        /// </summary>
        /// <param name="endianness">Endianness of the Bitfield.</param>
        public Bitfield(Endianness endianness = Endianness.LittleEndian)
        {
            this.endianness = endianness;
            bits = new List<bool>();
        }

        /// <summary>
        /// Instantiates a new Bitfield object with the given number of bits.
        /// </summary>
        /// <param name="bitCount">Number of bits.</param>
        /// <param name="endianness">Endianness of the Bitfield.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if bitCount is less than zero.</exception>
        public Bitfield(int bitCount, Endianness endianness = Endianness.LittleEndian)
            : this(endianness)
        {
            if (bitCount < 0) throw new ArgumentOutOfRangeException();

            for (int i = 0; i < bitCount; i++)
            {
                bits.Add(false);
            }
        }

        /// <summary>
        /// Instantiates a new Bitfield object with the given bits.
        /// </summary>
        /// <param name="bits">Bits to add.</param>
        /// <param name="endianness">Endianness of the Bitfield.</param>
        /// <exception cref="ArgumentNullException">Thrown if bits is null.</exception>
        public Bitfield(bool[] bits, Endianness endianness = Endianness.LittleEndian)
            : this(endianness)
        {
            if (bits == null) throw new ArgumentNullException();

            this.bits.AddRange(bits);
        }

        /// <summary>
        /// Instantiates a Bitfield object using the given Bitfield as a source.
        /// </summary>
        /// <param name="bitfield"></param>
        /// <param name="endianness"></param>
        public Bitfield(Bitfield bitfield, Endianness endianness = Endianness.LittleEndian)
            : this(endianness)
        {
            if (bitfield == null) throw new ArgumentNullException();

            bits.AddRange(bitfield.Bits);
        }

        #endregion

        #region Properties
        /***********************************************************************
         * Properties
         **********************************************************************/

        /// <summary>
        /// Gets the number of bits in the bitfield.
        /// </summary>
        /// VB: Public ReadOnly Property Count As Integer
        public int Count
        {
            get
            {
                return bits.Count;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the Bitfield is read-only.
        /// </summary>
        /// VB: Public ReadOnly Property IsReadOnly As Boolean
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the endianness of the bitfield.
        /// </summary>
        /// VB: Public ReadOnly Property Endian As Endianness
        public Endianness Endian
        {
            get
            {
                return endianness;
            }
        }

        /// <summary>
        /// Gets or sets a boolean array as bits to the Bitfield.
        /// </summary>
        /// VB: Public Property Bits As Boolean()
        public bool[] Bits
        {
            get
            {
                return bits.ToArray();
            }
            set
            {
                if (value == null) throw new ArgumentNullException();
                if (value.Length == 0) throw new ArgumentException("Bitfield length cannot be 0.");
                bits = new List<bool>(value);
            }
        }

        /// <summary>
        /// Gets a value determining if the bitfield can be evenly divided into bytes.
        /// </summary>
        /// VB: Public ReadOnly Property ByteDivisible As Boolean
        public bool ByteDivisible
        {
            get
            {
                return Count % 8 == 0;
            }
        }

        /// <summary>
        /// Gets the number of bytes that can be created from this Bitfield.
        /// </summary>
        /// <remarks>This does not guarantee that a Bitfield is byte-divible. Use the <ilink>ByteDivisible Property</ilink>
        /// to determine if a Bitfield count is divisible by 8.</remarks>
        /// VB: Public ReadOnly Property ByteCount As Integer
        public int ByteCount
        {
            get
            {
                return Count / 8;
            }
        }

        /// <summary>
        /// Gets the number of bits required for the Bitfield to be byte-divisible.
        /// </summary>
        public int CountToByteDivisible
        {
            get
            {
                if (!ByteDivisible) return (8 - (Count % 8));
                return 0;
            }
        }

        /// <summary>
        /// Gets or sets the bit at the particular index.
        /// </summary>
        /// <param name="index">Index of the bit.</param>
        /// <returns>Returns the bit.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if index is not valid for this Bitfield.</exception>
        /// VB: Public Property Item(ByVal index As Integer)
        public bool this[int index]
        {
            get
            {
                if ((index < 0) | (index >= bits.Count)) throw new ArgumentOutOfRangeException();
                return bits[index];
            }
            set
            {
                if ((index < 0) | (index >= bits.Count)) throw new ArgumentOutOfRangeException();
                bits[index] = value;
            }
        }

        #endregion

        #region Public Instance Methods
        /***********************************************************************
         * Public Instance Methods
         **********************************************************************/

        #region Bitfield Methods
        /// <summary>
        /// Reverses the order of the bitfield, and inverts the endianness.
        /// </summary>
        /// VB: Public Sub Reverse()
        public void Reverse()
        {
            bits.Reverse();
            if (endianness == Endianness.BigEndian)
                endianness = Endianness.LittleEndian;
            else
                endianness = Endianness.BigEndian;
        }

        /// <summary>
        /// Gets the IEnumerator for this class.
        /// </summary>
        /// <returns>Returns the IEnumerator for this class.</returns>
        /// VB: Public Function GetEnumerator() As IEnumerator(Of Boolean)
        public IEnumerator<bool> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return bits[i];
            }
        }

        /// <summary>
        /// Flips the value of the bit at the given index.
        /// </summary>
        /// <param name="index">Index of bit to flip.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if index is less than zero or greater than the highest Bitfield index.</exception>
        /// VB: Public Sub Flip(ByVal index As Integer)
        public void Flip(int index)
        {
            if ((index >= 0) & (index < bits.Count))
                bits[index] = !bits[index];
            else
                throw new ArgumentOutOfRangeException();
        }

        /// <summary>
        /// Pads the Bitfield with the given number of bits of the given value. 
        /// </summary>
        /// <param name="count">Number of bits to pad with.</param>
        /// <param name="bit">Value of pad.</param>
        /// <remarks>The side that is padded is dependent upon the endianness of the Bitfield. Little-endian bitfields 
        /// will be padded on the right, while big-endian bitfields will be padded on the left.</remarks>
        /// VB: Public Sub Pad(ByVal count As Integer, Optional ByVal bit As Boolean = False)
        public void Pad(int count, bool bit = false)
        {
            List<bool> bitList = new List<bool>();
            Action addBits = delegate() { for (int i = 0; i < count; i++) bitList.Add(bit); };

            if (Endian == Endianness.BigEndian) addBits.Invoke();
            bitList.AddRange(bits);
            if (Endian == Endianness.LittleEndian) addBits.Invoke();
            bits = bitList;
        }

        /// <summary>
        /// Pads the Bitfield with zeros (false) until it can be evenly divided into bytes.
        /// </summary>
        /// VB: Public Sub PadToByte()
        public void PadToByte()
        {
            if (!ByteDivisible) Pad(CountToByteDivisible);
        }

        /// <summary>
        /// Trims the left side of the bitfield of all bits with the given value.
        /// </summary>
        /// <param name="bit">Value to trim.</param>
        /// VB: Public Sub TrimStart(Optional ByVal bit As Boolean = False)
        public void TrimStart(bool bit = false)
        {
            List<bool> bitList = new List<bool>(bits);
            while (bitList[0] == bit)
            {
                bitList.RemoveAt(0);
            }
        }

        /// <summary>
        /// Trims the left side of the Bitfield of the given number of bits.
        /// </summary>
        /// <param name="count">Number of bits to remove.</param>
        /// VB: Public Sub TrimStart(ByVal count As Integer)
        public void TrimStart(int count)
        {
            bits.RemoveRange(0, count);
        }

        /// <summary>
        /// Trims the right side of the Bitfield of all bits with the given value.
        /// </summary>
        /// <param name="bit">Value to trim.</param>
        /// VB: Public Sub TrimEnd(Optional ByVal bit As Boolean = False)
        public void TrimEnd(bool bit = false)
        {
            List<bool> bitList = new List<bool>(bits);
            int last = bitList.Count - 1;

            while (bitList[last] == bit)
            {
                bitList.RemoveAt(last);
                last = bitList.Count - 1;
            }
        }

        /// <summary>
        /// Trims the right side of the Bitfield of the given number of bits.
        /// </summary>
        /// <param name="count">Number of bits to remove.</param>
        /// VB: Public Sub TrimEnd(ByVal count As Integer)
        public void TrimEnd(int count)
        {
            bits.RemoveRange(bits.Count - count - 1, count);
        }

        /// <summary>
        /// Trims both sides of the Bitfield of the given value of bit.
        /// </summary>
        /// <param name="bit">Value to remove.</param>
        /// <remarks>This is equivilent to calling TrimStart(bool) and TrimEnd(bool).</remarks>
        /// VB: Public Sub Trim(Optional ByVal bit As Boolean = False)
        public void Trim(bool bit = false)
        {
            TrimStart(bit);
            TrimEnd(bit);
        }

        /// <summary>
        /// Trims the Bitfield based on endianness.
        /// </summary>
        /// <param name="bit">Value to remove.</param>
        /// <remarks>If the Bitfield's Endian property is Endianness.LittleEndian, this method will call TrimEnd(bool). Otherwise, it will call TrimStart(bool).</remarks>
        /// VB: Public Sub TrimEndian(Optional ByVal bit As Boolean = False)
        public void TrimEndian(bool bit = false)
        {
            if (endianness == Endianness.LittleEndian) TrimEnd(bit);
            else TrimStart(bit);
        }

        /// <summary>
        /// Trims the Bitfield based on endianness.
        /// </summary>
        /// <param name="count">Number of bits to remove.</param>
        /// <remarks>If the Bitfield's Endian property is Endianness.LittleEndian, this method will call TrimEnd(int). Otherwise, it will call TrimStart(int).</remarks>
        /// VB: Public Sub TrimEndian(ByVal count As Integer)
        public void TrimEndian(int count)
        {
            if (endianness == Endianness.LittleEndian) TrimEnd(count);
            else TrimStart(count);
        }

        /// <summary>
        /// Returns the entire contents as a byte array.
        /// </summary>
        /// <returns>Returns the contents as a byte array.</returns>
        /// VB: Public Function ToByteArray() As Byte()
        public byte[] ToByteArray()
        {
            Bitfield b = new Bitfield(Bits, Endian);
            b.PadToByte();
            return b.ToByteArray(b.Count, 0);
        }

        /// <summary>
        /// Returns the given range of bits as a byte array.
        /// </summary>
        /// <param name="bitCount">Number of bits to use. Must be divisible by 8.</param>
        /// <param name="offset">Offset to start from.</param>
        /// <returns>Returns a byte array representing the given range.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the bitCount is not divisible by 8.</exception>
        /// <exception cref="IndexOutOfRangeException">Thrown if the range given exceeds the bounds of the Bitfield.</exception>
        /// <exception cref="ArgumentException">Thrown if the bitCount is greater than the Bitfield size or less than zero.</exception>
        /// VB: Public Function ToByteArray(ByVal bitCount As Integer, ByVal offset As Integer) As Byte()
        public byte[] ToByteArray(int bitCount, int offset)
        {
            if (bitCount % 8 != 0) throw new ArgumentOutOfRangeException("bitCount must be divisible by 8");

            int byteCount = bitCount / 8;
            if ((bitCount <= Count) & (bitCount > 0))
            {
                if (Count - offset < bitCount) throw new ArgumentException("Offset too large.");
                if (Endian == Endianness.BigEndian) bits.Reverse();

                byte[] bytes = new byte[byteCount];

                for (int i = 0; i < byteCount; i++)
                {
                    Range<bool> b = new Range<bool>(i * 8, 8, this, true);
                    bytes[i] = GetByte(b.Values);
                    //bytes[i] = GetByte(bits.ToArray(), (i * 8) + offset);
                }
                return bytes;
            }
            else
            {
                throw new IndexOutOfRangeException("Too many or to few bits selected.");
            }
        }

        /// <summary>
        /// Converts a portion of the Bitfield to a byte.
        /// </summary>
        /// <param name="offset">Index to start from.</param>
        /// <returns>Returns a byte representing the Bitfield.</returns>
        /// <exception cref="BoundaryOutOfRangeException">Thrown when there aren't enough bits after the offset.</exception>
        /// <exception cref="IndexOutOfRangeException">Thrown if the offset given is less than zero.</exception>
        /// VB: Public Function ToByte(Optional ByVal offset As Integer = 0) As Byte
        public byte ToByte(int offset = 0)
        {
            try
            {
                Range<bool> range = new Range<bool>(offset, 8, this, true);
                return Bitfield.GetByte(range.Values);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Converts a portion of the Bitfield into a signed byte.
        /// </summary>
        /// <param name="offset">Index to start from.</param>
        /// <returns>Returns a signed byte representing the Bitfield.</returns>
        /// <exception cref="BoundaryOutOfRangeException">Thrown when there aren't enough bits after the offset.</exception>
        /// <exception cref="IndexOutOfRangeException">Thrown if the offset given is less than zero.</exception>
        public sbyte ToSByte(int offset = 0)
        {
            try
            {
                return (sbyte)ToByte(offset);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Converts a portion of the Bitfield to an Int16.
        /// </summary>
        /// <param name="offset">Index to start from.</param>
        /// <returns>Returns an Int16.</returns>
        /// <remarks><warning>Make sure that you have 16 bits available from the offset. If not, be sure to use the 
        /// <ilink>Pad() function</ilink> to pad bits onto the correct end.</warning></remarks>
        /// <exception cref="BoundaryOutOfRangeException">Thrown if offset is less than zero, or if the sum of the offset 
        /// and length exceeds the size of the Bitfield.</exception>
        /// VB: Public Function ToInt16(Optional ByVal offset As Integer = 0) As Short
        public short ToInt16(int offset = 0)
        {
            try { return ToIntegral<short>(offset, sizeof(short), new Func<byte[], int, short>(BitConverter.ToInt16)); }
            catch { throw; }
        }

        /// <summary>
        /// Converts a portion of the Bitfield to an Int32.
        /// </summary>
        /// <param name="offset">Index to start from.</param>
        /// <returns>Returns an Int32.</returns>
        /// <remarks><warning>Make sure that you have 32 bits available from the offset. If not, be sure to use the 
        /// <ilink>Pad() function</ilink> to pad bits onto the correct end.</warning></remarks>
        /// <exception cref="BoundaryOutOfRangeException">Thrown if offset is less than zero, or if the sum of the offset 
        /// and length exceeds the size of the Bitfield.</exception>
        /// VB: Public Function ToInt32(Optional ByVal offset As Integer = 0) As Integer
        public int ToInt32(int offset = 0)
        {
            try { return ToIntegral<int>(offset, sizeof(int), new Func<byte[], int, int>(BitConverter.ToInt32)); }
            catch { throw; }
        }

        /// <summary>
        /// Converts a portion of the Bitfield to an Int64.
        /// </summary>
        /// <param name="offset">Index to start from.</param>
        /// <returns>Returns an Int64.</returns>
        /// <remarks><warning>Make sure that you have 64 bits available from the offset. If not, be sure to use the 
        /// <ilink>Pad() function</ilink> to pad bits onto the correct end.</warning></remarks>
        /// <exception cref="BoundaryOutOfRangeException">Thrown if offset is less than zero, or if the sum of the offset 
        /// and length exceeds the size of the Bitfield.</exception>
        /// VB: Public Function ToInt64(Optional ByVal offset As Integer = 0) As Long
        public long ToInt64(int offset = 0)
        {
            try { return ToIntegral<long>(offset, sizeof(long), new Func<byte[], int, long>(BitConverter.ToInt64)); }
            catch { throw; }
        }

        /// <summary>
        /// Converts a portion of the Bitfield to a UInt16.
        /// </summary>
        /// <param name="offset">Index to start from.</param>
        /// <returns>Returns an UInt16.</returns>
        /// <remarks><warning>Make sure that you have 16 bits available from the offset. If not, be sure to use the 
        /// <ilink>Pad() function</ilink> to pad bits onto the correct end.</warning></remarks>
        /// <exception cref="BoundaryOutOfRangeException">Thrown if offset is less than zero, or if the sum of the offset 
        /// and length exceeds the size of the Bitfield.</exception>
        /// VB: Public Function ToUInt16(Optional ByVal offset As Integer = 0) As UShort
        public ushort ToUInt16(int offset = 0)
        {
            try { return ToIntegral<ushort>(offset, sizeof(ushort), new Func<byte[], int, ushort>(BitConverter.ToUInt16)); }
            catch { throw; }
        }

        /// <summary>
        /// Converts a portion of the Bitfield to a UInt32.
        /// </summary>
        /// <param name="offset">Index to start from.</param>
        /// <returns>Returns an UInt32.</returns>
        /// <remarks><warning>Make sure that you have 32 bits available from the offset. If not, be sure to use the 
        /// <ilink>Pad() function</ilink> to pad bits onto the correct end.</warning></remarks>
        /// <exception cref="BoundaryOutOfRangeException">Thrown if offset is less than zero, or if the sum of the offset 
        /// and length exceeds the size of the Bitfield.</exception>
        /// VB: Public Function ToUInt32(Optional ByVal offset As Integer = 0) As UInteger
        public uint ToUInt32(int offset = 0)
        {
            try { return ToIntegral<uint>(offset, sizeof(uint), new Func<byte[], int, uint>(BitConverter.ToUInt32)); }
            catch { throw; }
        }

        /// <summary>
        /// Convert a portion of the Bitfield to a UInt64.
        /// </summary>
        /// <param name="offset">Index to start from.</param>
        /// <returns>Returns an UInt64.</returns>
        /// <remarks><warning>Make sure that you have 64 bits available from the offset. If not, be sure to use the 
        /// <ilink>Pad() function</ilink> to pad bits onto the correct end.</warning></remarks>
        /// <exception cref="BoundaryOutOfRangeException">Thrown if offset is less than zero, or if the sum of the offset 
        /// and length exceeds the size of the Bitfield.</exception>
        /// VB: Public Function ToUInt64(Optional ByVal offset As Integer = 0) As ULong
        public ulong ToUInt64(int offset = 0)
        {
            try { return ToIntegral<ulong>(offset, sizeof(ulong), new Func<byte[], int, ulong>(BitConverter.ToUInt64)); }
            catch { throw; }
        }

        /// <summary>
        /// Converts the Bitfield to an array of characters.
        /// </summary>
        /// <param name="encoding">Encoding to use.</param>
        /// <param name="offset">Index to start at.</param>
        /// <returns>Returns an array of characters representing the Bitfield.</returns>
        /// VB: Public Function ToChar(ByVal encoding As Encoding, Optional ByVal offset As Integer = 0) As Char()
        public char[] ToChars(Encoding encoding, int offset = 0)
        {
            Bitfield b = this;
            b.PadToByte();
            if (b.Endian != Endianness.LittleEndian) b.Reverse();
            byte[] bytes = b.ToByteArray(8, 0);
            return encoding.GetChars(bytes);
        }

        /// <summary>
        /// Converts the Bitfield into a string.
        /// </summary>
        /// <param name="encoding">Encoding to use.</param>
        /// <param name="offset">Index to start at.</param>
        /// <returns>Returns a string representing the Bitfield.</returns>
        /// VB: Public Overloads Function ToString(ByVal encoding As Encoding, Optional ByVal offset As Integer = 0) As String
        public string ToString(Encoding encoding, int offset = 0)
        {
            return new string(ToChars(encoding, offset));
        }

        /// <summary>
        /// Appends the given bits onto the Bitfield.
        /// </summary>
        /// <param name="source">Source Bitfield.</param>
        /// <remarks>This method will append the bits to the appropriate side of the Bitfield, accounting for endianness. 
        /// If the source Bitfield has a different <ilink>Endian</ilink> value than the destination Bitfield, the order of the source
        /// is reversed so both Bitfields match.</remarks>
        /// <exception cref="ArgumentNullException">Thrown if the source Bitfield is null.</exception>
        /// VB: Public Sub Append(ByVal source As Bitfield)
        public void Append(Bitfield source)
        {
            if (source == null) throw new ArgumentNullException();

            bool[] b = source.Bits;
            if (source.Endian != Endian) b.Reverse();
            Append(b);
        }

        /// <summary>
        /// Appends the given bits onto the Bitfield.
        /// </summary>
        /// <param name="bitArray">Source bit array.</param>
        /// <remarks>This method assumes that the source array has the same <ilink>endianness</ilink> as the Bitfield. </remarks>
        /// VB: Public Sub Append(ByVal bitArray() As Boolean)
        public void Append(bool[] bitArray)
        {
            if (bitArray == null) throw new ArgumentNullException();

            bits.AddRange(bitArray);
        }
        #endregion

        #region IList Methods
        /// <summary>
        /// Appends one bit to the right side of the Bitfield.
        /// </summary>
        /// <param name="item">Value to append.</param>
        /// VB: Public Sub Add(ByVal item As Boolean)
        public void Add(bool item)
        {
            bits.Add(item);
        }

        /// <summary>
        /// Not supported. Bitfields cannot contain zero bits.
        /// </summary>
        /// <exception cref="NotSupportedException">Thrown on calling Clear(). Bitfields cannot contains zero bits.</exception>
        /// VB: Public Sub Clear()
        public void Clear()
        {
            bits.Clear();
        }

        /// <summary>
        /// Determines if the given item exists in the Bitfield.
        /// </summary>
        /// <param name="item">Item to look for.</param>
        /// <returns>Returns true if the item exists. Otherwise, returns false.</returns>
        /// VB: Public Function Contains(ByVal item As Boolean) As Boolean
        public bool Contains(bool item)
        {
            return bits.Contains(item);
        }

        /// <summary>
        /// Copies items to the given array starting at the given index.
        /// </summary>
        /// <param name="array">Array to copy to.</param>
        /// <param name="index">Index to start at.</param>
        /// <exception cref="ArgumentNullException">Thrown if array is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if index is less than zero.</exception>
        /// <exception cref="ArgumentException">Thrown if array is multidimensional, 
        /// the number of elements required is fewer than the number of those available,
        /// or the type of the source collection cannot be automatically casted into the destination array.</exception>
        /// VB: Public Sub CopyTo(ByVal array() As Boolean, ByVal index as Integer)
        public void CopyTo(bool[] array, int index)
        {
            try
            {
                bits.CopyTo(array, index);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Returns the index of the given item.
        /// </summary>
        /// <param name="item">Item to look for.</param>
        /// <returns>Returns an integer representing the index of the given item. If the item does not exist, returns -1.</returns>
        /// <remarks>This function is not very useful for a Bitfield because of the two possible states per bit. However, it is required by IList.</remarks>
        /// VB: Public Function IndexOf(ByVal item As Boolean) As Integer
        public int IndexOf(bool item)
        {
            return bits.IndexOf(item);
        }

        /// <summary>
        /// Inserts the given item at the given index.
        /// </summary>
        /// <param name="index">Index to insert at.</param>
        /// <param name="item">Item to insert.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if index is not a valid index of the Bitfield.</exception>
        /// VB: Public Sub Insert(ByVal index As Integer, ByVal item As Boolean) As Integer
        public void Insert(int index, bool item)
        {
            try
            {
                bits.Insert(index, item);
            }
            catch
            {
                throw;  // check MSDN for exceptions for this function
            }
        }

        /// <summary>
        /// Removes the first item matching the given value.
        /// </summary>
        /// <param name="item">Item to remove.</param>
        /// <returns>Returns true if the item was found and removed. Otherwise, returns false.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the item found is the last item in the Bitfield.</exception>
        /// VB: Public Function Remove(ByVal item As Boolean) As Boolean
        public bool Remove(bool item)
        {
            if ((bits.Count == 1) && (bits.Contains(item))) throw new InvalidOperationException();
            return bits.Remove(item);
        }

        /// <summary>
        /// Removes the item at the given index.
        /// </summary>
        /// <param name="index">Index of item to remove.</param>
        /// <exception cref="InvalidOperationException">Thrown if the item removed is the last item in the Bitfield.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if index is not a valid index of the Bitfield.</exception>
        /// VB: Public Sub RemoveAt(ByVal index As Integer)
        public void RemoveAt(int index)
        {
            if (bits.Count == 1) throw new InvalidOperationException();
            try
            {
                bits.RemoveAt(index);
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #endregion

        #region Private Instance Methods
        /***********************************************************************
         * Private Instance Methods
         **********************************************************************/
        /// <summary>
        /// Converts a portion of the Bitfield to an integral type.
        /// </summary>
        /// <typeparam name="T">Type of integral.</typeparam>
        /// <param name="offset">Bitwise offset</param>
        /// <param name="byteSize">Number of bytes to grab</param>
        /// <param name="bitconverter">Bitconverter function</param>
        /// <returns>Returns the requested integral type</returns>
        /// <exception cref="BoundaryOutOfRangeException">Thrown if offset is less than zero, or if the sum of the offset 
        /// and length exceed the size of the Bitfield.</exception>
        private T ToIntegral<T>(int offset, int byteSize, Func<byte[], int, T> bitconverter)
        {
            if ((offset < 0) | (offset + (byteSize * 8) > Count)) throw new BoundaryOutOfRangeException();
            try
            {
                Bitfield b = new Bitfield(Bits, endianness);
                b.PadToByte();
                if (b.Endian != Endianness.LittleEndian) b.Reverse();
                byte[] bytes = b.ToByteArray(byteSize * 8, offset);
                return bitconverter.Invoke(bytes, 0);
            }
            catch
            {
                throw new BoundaryOutOfRangeException();
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

        #region Public Static Methods
        /***********************************************************************
         * Public Static Methods
         **********************************************************************/

        /// <summary>
        /// Gets a Bitfield representing the given byte.
        /// </summary>
        /// <param name="value">Byte to convert to a Bitfield.</param>
        /// <returns>Returns the byte as Bitfield.</returns>
        /// VB: Public Shared Function FromByte(ByVal value As Byte) As Bitfield
        public static Bitfield FromByte(byte value)
        {
            Endianness end;
            if (BitConverter.IsLittleEndian) end = Endianness.LittleEndian;
            else end = Endianness.BigEndian;
            return new Bitfield(GetBits(value), end);
        }

        /// <summary>
        /// Gets a Bitfield representing a given byte array.
        /// </summary>
        /// <param name="value">Byte array to convert to a Bitfield.</param>
        /// <returns>Returns the byte array as a Bitfield.</returns>
        /// <exception cref="ArgumentNullException">Thrown if value is null.</exception>
        public static Bitfield FromByteArray(byte[] value)
        {
            if (value == null) throw new ArgumentNullException();

            Endianness end;
            if (BitConverter.IsLittleEndian) end = Endianness.LittleEndian;
            else end = Endianness.BigEndian;

            Bitfield b = new Bitfield(Bitfield.GetBits(value), end);
            return b;
        }

        /// <summary>
        /// Gets a boolean array representing the given byte in little endian.
        /// </summary>
        /// <param name="value">Byte to convert into a boolean array.</param>
        /// <returns>Returns a boolean array representing the given byte in little endian.</returns>
        /// <remarks>If you are planning on assigning the output of this function to a Bitfield
        /// remember to take endianness into account. The output of this function is always in
        /// little endian, which is the default for Bitfields. However, to assign it to a big
        /// endian Bitfield, you will have to reverse the array beforehand.</remarks>
        /// VB: Public Shared Function GetBits(ByVal value As Byte) As Boolean()
        public static bool[] GetBits(byte value)
        {
            bool[] bits = new bool[8];

            byte v = value;
            for (int i = 0; i < 8; i++)
            {
                v = value;
                v = (byte)(v >> (7 - i));
                v = (byte)(v << 7);
                v = (byte)(v >> 7);
                if (v == 0) bits[7 - i] = false;
                else bits[7 - i] = true;
            }
            return bits;
        }

        /// <summary>
        /// Gets the byte value represented by a little endian boolean array.
        /// </summary>
        /// <param name="value">Boolean array to translate</param>
        /// <param name="offset">Offset of index.</param>
        /// <returns>Returns a byte representing the boolean array.</returns>
        /// <exception cref="ArgumentNullException">Thrown if value is null.</exception>
        /// <exception cref="ArgumentException">Thrown if the value does not contain at least 8 elements.</exception>
        /// <exception cref="IndexOutOfRangeException">Thrown if the an index is not valid. Occurs when the offset is too high.</exception>
        /// VB: Public Shared Function GetByte(ByVal value() As Boolean, Optional ByVal offset As Integer = 0) As Byte
        public static byte GetByte(bool[] value, int offset = 0)
        {
            byte b = 0;

            if (value == null) throw new ArgumentNullException();
            if (value.Length < 8) throw new ArgumentException("Boolean array must contain at least 8 elements.");
            if (value.Length - offset < 8) throw new IndexOutOfRangeException("Offset is too high.");

            for (int i = 0; i < 8; i++)
            {
                //if (value[i]) b = (byte)(b + (1 * (10 ^ i)));
                if (value[i])
                {
                    b = (byte)(b + (byte)(Math.Pow(2, i)));
                }
            }
            return b;
        }

        /// <summary>
        /// Gets a little endian boolean array representing the given little endian byte array.
        /// </summary>
        /// <param name="value">Little endian byte array to convert.</param>
        /// <returns>Returns a boolean array in little endian.</returns>
        /// <remarks>This function assumes that the byte array is little-endian.</remarks>
        /// VB: Public Shared Function GetBits(ByVal value() As Byte) As Boolean()
        public static bool[] GetBits(byte[] value)
        {
            List<bool> bits = new List<bool>();
            foreach (byte v in value)
            {
                bits.AddRange(GetBits(v));
            }
            return bits.ToArray();
        }

        /// <summary>
        /// Creates a Bitfield representing the given SByte.
        /// </summary>
        /// <param name="value">Value to use.</param>
        /// <returns>Returns a Bitfield representing the given SByte.</returns>
        public static Bitfield FromSByte(sbyte value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            return new Bitfield(GetBits(bytes));
        }

        /// <summary>
        /// Creates a Bitfield representing the given Int16.
        /// </summary>
        /// <param name="value">Value to use.</param>
        /// <returns>Returns a Bitfield representing the given Int16.</returns>
        /// VB: Public Shared Function FromInt16(ByVal value As Short) As Bitfield
        public static Bitfield FromInt16(short value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            return new Bitfield(GetBits(bytes));
        }

        /// <summary>
        /// Creates a Bitfield representing the given UInt16.
        /// </summary>
        /// <param name="value">Value to use.</param>
        /// <returns>Returns a Bitfield representing the given UInt16.</returns>
        /// VB: Public Shared Function FromUInt16(ByVal value As UShort) As Bitfield
        public static Bitfield FromUInt16(ushort value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            return new Bitfield(GetBits(bytes));
        }

        /// <summary>
        /// Creates a Bitfield representing the given Int32.
        /// </summary>
        /// <param name="value">Value to use.</param>
        /// <returns>Returns a Bitfield representing the given Int32.</returns>
        /// VB: Public Shared Function FromInt32(ByVal value As Integer) As Bitfield
        public static Bitfield FromInt32(int value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            return new Bitfield(GetBits(bytes));
        }

        /// <summary>
        /// Creates a Bitfield representing the given UInt32.
        /// </summary>
        /// <param name="value">Value to use.</param>
        /// <returns>Returns a Bitfield representing the given UInt32.</returns>
        /// VB: Public Shared Function FromUInt32(ByVal value As UInteger) As Bitfield
        public static Bitfield FromUInt32(uint value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            return new Bitfield(GetBits(bytes));
        }

        /// <summary>
        /// Creates a Bitfield representing the given Int64.
        /// </summary>
        /// <param name="value">Value to use.</param>
        /// <returns>Returns a Bitfield representing the given Int64.</returns>
        /// VB: Public Shared Function FromInt64(ByVal value As Long) As Bitfield
        public static Bitfield FromInt64(long value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            return new Bitfield(GetBits(bytes));
        }

        /// <summary>
        /// Creates a Bitfield representing the given UInt64.
        /// </summary>
        /// <param name="value">Value to use.</param>
        /// <returns>Returns a Bitfield representing the given UInt64.</returns>
        /// VB: Public Shared Function FromUInt64(ByVal value As ULong) As Bitfield
        public static Bitfield FromUInt64(ulong value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            return new Bitfield(GetBits(bytes));
        }

        /// <summary>
        /// Creates a Bitfield representing the given character.
        /// </summary>
        /// <param name="c">Character to use.</param>
        /// <param name="encoding">Encoding to use.</param>
        /// <returns>Returns a Bitfield representing the given character.</returns>
        /// VB: Public Shared Function FromChar(ByVal c As Char, ByVal encoding As Encoding) As Bitfield
        public static Bitfield FromChar(char c, Encoding encoding)
        {
            byte[] bytes = encoding.GetBytes(new char[] { c });
            return new Bitfield(GetBits(bytes));
        }

        /// <summary>
        /// Creates a Bitfield representing the given string.
        /// </summary>
        /// <param name="s">String to use.</param>
        /// <param name="encoding">Encoding to use.</param>
        /// <returns>Returns a Bitfield representing the given string.</returns>
        /// VB: Public Shared Function FromString(ByVal s As String, ByVal encoding As Encoding) As Bitfield.
        public static Bitfield FromString(string s, Encoding encoding)
        {
            byte[] bytes = encoding.GetBytes(s);
            return new Bitfield(GetBits(bytes));
        }

        #endregion

        #region Private Delegates
        /***********************************************************************
         * Private Delegates
         **********************************************************************/

        private delegate bool BinaryBitwiseOperator(bool a, bool b);
        private delegate bool UnaryBitwiseOperator(bool value);

        #endregion

        #region Private Static Methods
        /***********************************************************************
         * Private Static Methods
         **********************************************************************/

        /// <summary>
        /// Performs a binary bitwise operation on a pair of Bitfields, taking endianness into account.
        /// </summary>
        /// <param name="bitfield1">First Bitfield.</param>
        /// <param name="bitfield2">Second Bitfield.</param>
        /// <param name="op">Operation to use.</param>
        /// <returns>Returns the result of the operation.</returns>
        /// <remarks>The returned Bitfield has the same endianness as that of the larger of the two Bitfields. If the two Bitfields have 
        /// the same length, the returned Bitfield has the same endianness as the first Bitfield.</remarks>
        private static Bitfield _BinaryBitwiseOperator(Bitfield bitfield1, Bitfield bitfield2, BinaryBitwiseOperator op)
        {
            Bitfield smaller, larger;

            // determine which bitfield is smaller
            if (bitfield1.Count <= bitfield2.Count)
            {
                smaller = bitfield1;
                larger = bitfield2;
            }
            else
            {
                smaller = bitfield2;
                larger = bitfield1;
            }

            // if the endiannesses aren't the same, reverse the smaller bitfield to match the larger
            if (bitfield1.endianness != bitfield2.endianness)
            {
                smaller.Reverse();
            }

            // perform the operation
            for (int i = 0; i < smaller.Count; i++)
            {
                larger[i] = op.Invoke(smaller[i], larger[i]);
            }

            return larger; // return the larger bitfield
        }

        private static Bitfield _UnaryBitwiseOperator(Bitfield bitfield, UnaryBitwiseOperator op)
        {
            Bitfield bits = bitfield;

            for (int i = 0; i < bits.Count; i++)
            {
                bits[i] = op.Invoke(bits[i]);
            }
            return bits;
        }
        #endregion

        #region Operators
        /***********************************************************************
         * Operators
         **********************************************************************/

        /// <summary>
        /// Bitwise AND
        /// </summary>
        /// <param name="bitfield1"></param>
        /// <param name="bitfield2"></param>
        /// <returns></returns>
        public static Bitfield operator &(Bitfield bitfield1, Bitfield bitfield2)
        {
            return _BinaryBitwiseOperator(bitfield1, bitfield2, delegate(bool a, bool b) { return a & b; });
        }

        /// <summary>
        /// Bitwise OR
        /// </summary>
        /// <param name="bitfield1"></param>
        /// <param name="bitfield2"></param>
        /// <returns></returns>
        public static Bitfield operator |(Bitfield bitfield1, Bitfield bitfield2)
        {
            return _BinaryBitwiseOperator(bitfield1, bitfield2, delegate(bool a, bool b) { return a | b; });
        }

        /// <summary>
        /// Bitwise XOR
        /// </summary>
        /// <param name="bitfield1"></param>
        /// <param name="bitfield2"></param>
        /// <returns></returns>
        public static Bitfield operator ^(Bitfield bitfield1, Bitfield bitfield2)
        {
            return _BinaryBitwiseOperator(bitfield1, bitfield2, delegate(bool a, bool b) { return a ^ b; });
        }

        /// <summary>
        /// Bitwise NOT
        /// </summary>
        /// <param name="bitfield"></param>
        /// <returns></returns>
        public static Bitfield operator !(Bitfield bitfield)
        {
            return _UnaryBitwiseOperator(bitfield, delegate(bool value) { return !value; });
        }

        /// <summary>
        /// Rotational left-shift
        /// </summary>
        /// <param name="bitfield"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static Bitfield operator <<(Bitfield bitfield, int count)
        {
            Bitfield bits = bitfield;

            bool temp;
            for (int c = 0; c < count; c++)
            {
                temp = bits[0];
                for (int i = 0; i < bitfield.Count; i++)
                {
                    bits[i] = bits[i + 1];
                }
                bits[bits.Count - 1] = temp;
            }

            return bits;
        }

        /// <summary>
        /// Rotational right-shift
        /// </summary>
        /// <param name="bitfield"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static Bitfield operator >>(Bitfield bitfield, int count)
        {
            Bitfield b = bitfield;
            b.Reverse();
            b = b << count;
            b.Reverse();
            return b;
        }

        /// <summary>
        /// Increments the value by 1.
        /// </summary>
        /// <param name="bitfield"></param>
        /// <returns></returns>
        public static Bitfield operator ++(Bitfield bitfield)
        {
            Bitfield b = bitfield;
            bool reversed = false;

            if (b.Endian != Endianness.LittleEndian)
            {
                b.Reverse();
                reversed = true;
            }

            for (int i = 0; i < b.Count; i++)
            {
                if (!b[i])
                {
                    b[i] = true;
                    break;
                }
                else
                {
                    b[i] = false;
                }
            }

            if (reversed) b.Reverse();
            return b;
        }

        /// <summary>
        /// Decrements the value by 1.
        /// </summary>
        /// <param name="bitfield"></param>
        /// <returns></returns>
        public static Bitfield operator --(Bitfield bitfield)
        {
            Bitfield b = bitfield;
            bool reversed = false;

            if (b.Endian != Endianness.LittleEndian)
            {
                b.Reverse();
                reversed = true;
            }

            for (int i = 0; i < b.Count; i++)
            {
                if (b[i])
                {
                    b[i] = false;
                    break;
                }
                else
                {
                    b[i] = true;
                }
            }

            if (reversed) b.Reverse();
            return b;
        }
        #endregion
    }
}
