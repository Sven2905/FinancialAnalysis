using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using WebApiWrapper;

namespace FinancialAnalysis.Logic.MessageSystem
{
    public class MessageSystemClient
    {
        TcpClient client;
        IPEndPoint serverEndPoint;
        public MessageSystemClient()
        {
            client = new TcpClient();
            WebApiConfiguration webApiConfigurationFile = BinarySerialization.ReadFromBinaryFile<WebApiConfiguration>(@".\WebApiConfig.cfg");
            serverEndPoint = new IPEndPoint(IPAddress.Parse(webApiConfigurationFile.Server), 3000);
        }

        public void SendMessage(string message)
        {
            client.Connect(serverEndPoint);

            NetworkStream clientStream = client.GetStream();

            ASCIIEncoding encoder = new ASCIIEncoding();
            byte[] buffer = encoder.GetBytes("message");

            clientStream.Write(buffer, 0, buffer.Length);
            clientStream.Flush();

            //message has successfully been received
            byte[] receivedData = new byte[4096];
            int bytesRead;

            bytesRead = 0;

            try
            {
                //blocks until a client sends a message
                bytesRead = clientStream.Read(receivedData, 0, 4096);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
