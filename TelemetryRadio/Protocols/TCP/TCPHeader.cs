using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GSLib.Collections;

namespace TelemetryRadio.Protocols.TCP
{
    public struct TCPHeader : IHeader
    {
        /// <summary>
        /// Identifies the sending port
        /// </summary>
        ushort sourcePort;
        /// <summary>
        /// Identifies the receiving port
        /// </summary>
        ushort destPort;
        /// <summary>
        /// If SYN flag is set, then this is the initial sequence number.  The sequence number of the actual first data byte and the
        /// acknowledged number in the corresponding ACK are then this sequence plus 1.
        /// If SYN is clear, then this is the accumulated sequence number of the first byte of this segment for the current session.
        /// </summary>
        uint sequenceNumber;
        /// <summary>
        /// If the ACK flag is set then the value of this field is the next sequence number that the receiver is expecting. This 
        /// acknowledges receipt of all prior bytes, if any. The first ACK sent by each end acknowledges the other ends initial
        /// sequence number itself, but no data.
        /// </summary>
        uint acknowledgementNumber;

        #region FLAGS

        /// <summary>
        /// Specifies the size of the TCP header in 32-bit words. The minimum size is 5 words, and the maximum is 15 words, thus 
        /// giving a minimum size of 20 bytes and maximum size of 60 bytes, allowing for up to 40 bytes of options in the header.
        /// This field will be the first 4 bits of the FLAGS field.
        /// </summary>
        byte dataOffset;

        /// <summary>
        /// Reserved for further use and should be set to zero. This field will be 3 bits within the FLAGS field.
        /// </summary>
        byte reserved;

        /// <summary>
        /// ECN-nonce concealment protection.
        /// </summary>
        bool NS;

        /// <summary>
        /// Congestion Window Reducer flag is set by the sending host to indicate that it received the TCP segment with the ECE flag set
        /// and had responded in congestion control mechanism.
        /// </summary>
        bool CWR;

        /// <summary>
        /// ECN-Echo indicates, if the SYN flag is set, that the TCP peer is ECN capable. If the SYN flag is clear, that that packet with
        /// Congestion Experienced flag in IP header set is received during normal transmission.
        /// </summary>
        bool ECE;

        /// <summary>
        /// Indicates that the urgent pointer field is significant.
        /// </summary>
        bool URG;

        /// <summary>
        /// Indicates that the acknowledgement field is significant. All packets after the initial SYN packet sent by the client should have
        /// this flag set.
        /// </summary>
        bool ACK;

        /// <summary>
        /// Synchronize sequence numbers. Only the first packet sent from each end should have this flag set. Some other flags change meaning 
        /// based on this flag, and some are only valid when it is set.
        /// </summary>
        bool SYN;

        /// <summary>
        /// No more data from sender.
        /// </summary>
        bool FIN;

        #endregion

        /// <summary>
        /// The size of the receive window, which specifies the number of bytes that the sender of this segment is currently willing to receive.
        /// </summary>
        ushort windowSize;

        /// <summary>
        /// The 16-bit checksum field is used for error-checking the header and data
        /// </summary>
        short checksum;

        /// <summary>
        /// If the URG flag is set, then this value is an offset from the sequence number indicating the last urgent data byte
        /// </summary>
        ushort urgentPointer;

        /// <summary>
        /// The length of this field is determined by the offset field.
        /// </summary>
        Bitfield options;

        public byte[] ToByteArray()
        {
            throw new NotImplementedException();
        }
    }
}
