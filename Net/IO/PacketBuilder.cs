using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapleRoll.Net.IO
{
    public class PacketBuilder
    {
        MemoryStream _ms;

        public PacketBuilder()
        {
            _ms = new MemoryStream();
        }

        public void WriteOpCode(byte opcode)
        {

            _ms.WriteByte(opcode);

        }

        public void WriteMessage(string message) 
        { 
            var messageLength= message.Length;

            //string newMessage = $"{messageLength.ToString()}:{message}";
            //var newMessageLegth= newMessage.Length;

           // _ms.Write(BitConverter.GetBytes(newMessageLegth), 0, 4);
            //_ms.Write(Encoding.ASCII.GetBytes(newMessage), 0, newMessageLegth);

            _ms.Write(BitConverter.GetBytes(messageLength), 0, 4);
            _ms.Write(Encoding.ASCII.GetBytes(message),0, messageLength);
        
        }

        public Byte[] GetPacketBytes()
        {

            return _ms.ToArray();
        }
    }
}
