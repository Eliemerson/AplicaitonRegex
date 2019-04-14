using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace SendFile
{
    class Program
    {
        static void Main(string[] args)
        {
            NetworkStream stream = null;
            TcpListener listener = null;
            BinaryWriter writer = null;
            BinaryReader reader = null;

          
            try
            {
                Int32 port = 13000;
                TcpClient client = new TcpClient("127.0.0.1", port);

                Console.WriteLine("Cliente ------> Trying to connect, wait...");
                Console.WriteLine("Reading the doc, wait...");

                string[] text = System.IO.File.ReadAllLines(@"C:\dev\git\HomeWorkJilian\mockData.txt");
                string formatPattern = @"[0-9]{5}-[\d]{3}";
                StringBuilder builder = new StringBuilder();

                for (int i = 0; i < text.Length; i++)
                {
                    var filter = Regex.Match(text[i], formatPattern);
                    builder.Append(filter).Append("");                    
                }

                
                System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\dev\git\HomeWorkJilian\WriteData.txt");
                file.WriteLine(builder.ToString());
                file.Close();
                string RegexText = System.IO.File.ReadAllText(@"C:\dev\git\HomeWorkJilian\WriteData.txt");


                Byte[] data = System.Text.Encoding.ASCII.GetBytes(RegexText);
                stream = client.GetStream();
                stream.Write(data, 0, data.Length);
                Console.WriteLine("Sent: {0}", data);

                var sizebyte = data.Length;

                Console.WriteLine("Size bye {0} in data send", sizebyte);

                data = new Byte[sizebyte];
                String responseData = String.Empty;
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine("Received: {0}", responseData);

                stream.Close();
                client.Close();
                Console.ReadKey();

            }
            catch (WebException e) { }
        }
    }

}
