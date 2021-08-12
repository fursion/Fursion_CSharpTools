using System;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Text;

namespace Fursion_CSharpTools.Net.UDP
{
    public class UDPComponentPool:Singleton<UDPComponentPool>
    {
        private const int defSize = 20;
        private List<UDP_Data_Receive_Component> def_UDP_Component;
        public UDPComponentPool()
        {
            def_UDP_Component = new List<UDP_Data_Receive_Component>();
            for(int i = 0; i < defSize; i++)
            {
                def_UDP_Component.Add(new UDP_Data_Receive_Component());
            }
        }
        public static UDP_Data_Receive_Component UDPDataComponent(UdpClient udpClient)
        {
            return new UDP_Data_Receive_Component(udpClient);
        }
        private static void Expansion()
        {

        }
    }
}
