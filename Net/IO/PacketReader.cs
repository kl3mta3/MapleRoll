using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MapleRoll.Net.IO
{
    public class PacketReader : BinaryReader
    {
        private NetworkStream _stream;
        public PacketReader(NetworkStream stream) : base(stream)
        {

            _stream = stream;

        }

        public string ReadMessage()
        {
            Byte[] buffer;
            var length = ReadInt32();
            buffer = new Byte[length];
            _stream.Read(buffer, 0, length);

            var message = Encoding.ASCII.GetString(buffer);

            return message;
        }

    }
}
