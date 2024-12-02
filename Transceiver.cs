using System.Collections.Generic;

namespace FrequencyViewerPlugin
{
    public class TransceiverData
    {
        public int id { get; set; }
        public double frequency { get; set; }
        public double latDeg { get; set; }
        public double lonDeg { get; set; }
        public double heightMslM { get; set; }
        public double heightAglM { get; set; }
        public string FreqMhz()
        {
            return $"{frequency / 1_000_000:0.000}";
        }
    }

    public class Transceiver
    {
        public string callsign { get; set; }
        public List<TransceiverData> transceivers { get; set; }
    }
}
