﻿using PacketLibrary.Logging;
using System;
using System.Net.Sockets;

namespace PacketLibrary.Network
{
    public class DefaultConnection : IConnection
    {
        public Logger logger = Logger.LOGGER;

        private Protocol CurrentProtocol;
        private TcpClient Client;

        public DefaultConnection(Protocol protocol, TcpClient client)
        {
            CurrentProtocol = protocol;
            Client = client;
        }

        public void Read()
        {
            NetworkStream stream = Client.GetStream();

            try
            {
                while (stream.DataAvailable)
                {
                    Packet packet = CurrentProtocol.ProtocolRegistry.ReadPacket(stream);
                }
            }
            catch (ArgumentOutOfRangeException exception)
            {
                logger.Error("Something went wrong when reading data from stream of client.", exception);
            }
        }

        public TcpClient GetClient()
        {
            return Client;
        }

        public Protocol GetCurrentProtocol()
        {
            return CurrentProtocol;
        }

        public void SendPacket(Packet packet) => GetCurrentProtocol().ProtocolRegistry.WritePacket(packet, Client.GetStream());

        public void SetProtocol(Protocol protocol)
        {
            CurrentProtocol = protocol;
        }
    }
}
