using System.Runtime.Serialization;
using System.IO.Ports;

namespace CookieEdit2
{
    [DataContract]
    public class SerialIoSettings
    {
        [DataMember]
        public string name;
        [DataMember]
        public string port;
        [DataMember]
        public int baud;
        [DataMember]
        public Parity parity;
        [DataMember]
        public int dataBits;
        [DataMember]
        public StopBits stopBits;
        [DataMember]
        public Handshake handshake;

        public override string ToString()
        {
            return name;
        }

        public SerialIoSettings()
        {
            this.name = "New Config";
            this.port = "COM1";
            this.baud = 9600;
            this.parity = Parity.Even;
            this.dataBits = 7;
            this.stopBits = StopBits.One;
            this.handshake = Handshake.XOnXOff;
        }

        public SerialIoSettings(string name, string port, int baud, Parity parity, int dataBits, StopBits stopBits, Handshake handshake)
        {
            this.name = name;
            this.port = port;
            this.baud = baud;
            this.parity = parity;
            this.dataBits = dataBits;
            this.stopBits = stopBits;
            this.handshake = handshake;
        }

        public SerialPort GetPort()
        {
            var newport = new SerialPort(name, baud, parity, dataBits, stopBits)
            {
                Handshake = handshake
            };
            return newport;
        }
    }


}
