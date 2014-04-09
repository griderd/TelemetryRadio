using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GSLib.Extensions
{
    public static class ByteArray
    {
        /// <summary>
        /// Gets all of the bytes in the array using the given specification.
        /// </summary>
        /// <param name="bytes">Array to print.</param>
        /// <param name="prefix">Byte prefix, if any ("0x").</param>
        /// <param name="format">Format for the string.</param>
        /// <param name="seperateBytes">If true, seperates the bytes with a space.</param>
        /// <returns>Returns a formatted string containing all of the bytes in the array.</returns>
        public static string ArrayToString(this Byte[] bytes, string prefix, string format, bool seperateBytes = false)
        {
            StringBuilder o = new StringBuilder();

            for (int i = 0; i < bytes.Length; i++)
            {
                byte b = bytes[i];
                o.Append(prefix);
                o.Append(b.ToString(format));
                if ((seperateBytes) && (i < bytes.Length - 1)) o.Append(" ");
            }

            return o.ToString();
        }
    }
}
