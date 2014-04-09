using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TelemetryRadio
{
    public interface IRadioReciever
    {
        /// <summary>
        /// The channel the receiver is tuned to.
        /// </summary>
        int ReceiverChannel { get; set; }

        /// <summary>
        /// Tunes the IRadioReceiver to the given channel.
        /// </summary>
        /// <param name="channel">Channel to tune to.</param>
        void TuneReceiver(int channel);

        /// <summary>
        /// Gets and processes the current packet on the current channel.
        /// </summary>
        void GetPacket();
    }
}
