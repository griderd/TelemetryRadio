using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TelemetryRadio.Protocols
{
    public interface IHeader
    {
        /// <summary>
        /// Converts the protocol header into a byte array.
        /// </summary>
        /// <returns></returns>
        byte[] ToByteArray();
    }
}
