using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TelemetryRadio.Protocols.TCP
{
    public enum TCPState
    {
        /// <summary>
        /// (Server) Represents waiting for a connection request from any remote TCP and port.
        /// </summary>
        Listen,
        /// <summary>
        /// (Client) Represents waiting for a matching connection request after having sent a connection request.
        /// </summary>
        SynSent,
        /// <summary>
        /// (Server) Represents waiting for a confirming connection request acknowledgement after having both received and sent a connection request.
        /// </summary>
        SynReceived,
        /// <summary>
        /// Represents an open connection. Data can be sent and receieved.
        /// </summary>
        Established,
        /// <summary>
        /// Represents waiting for a connection termination request from the local TCP.
        /// </summary>
        FinWait1,
        /// <summary>
        /// Represents waiting for a connection termination request from the local user.
        /// </summary>
        FinWait2,
        /// <summary>
        /// Represents waiting for an acknowledgement of the connection termination request previously sent to the remote TCP.
        /// </summary>
        LastAck,
        /// <summary>
        /// Represents waiting for enough time to pass to be sure the remote TCP received and acknowledged its connection termination request.
        /// </summary>
        TimeWait,
        /// <summary>
        /// Represents no connection state
        /// </summary>
        Closed
    }
}
