using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TelemetryRadio.Protocols
{
    /// <summary>
    /// Represents a packet of data using a specific protocol.
    /// </summary>
    public interface IPacket
    {
        /// <summary>
        /// Gets the protocol header
        /// </summary>
        IHeader Header { get; }

        /// <summary>
        /// Gets the transmission data
        /// </summary>
        byte[] Body { get; }

        /// <summary>
        /// Gets the maximum number of bytes the packet can take up.
        /// </summary>
        int MaxLength { get; }

        /// <summary>
        /// Converts the header and data into a byte array.
        /// </summary>
        /// <returns></returns>
        byte[] ToByteArray();
    }
}
