using System;
using System.Net.Sockets;
using System.Net;
using System.Data.SqlClient;
using System.Text;

namespace ReceptorMensage
{
    class MainClass
    {

        static void Main()
        {


            MainClass main = new MainClass();
            TcpListener server = null;
            TcpClient client = null;
            NetworkStream stream = null;

            Byte[] bytes = new Byte[701264];
            String data = null;
            try
            {
                Int32 port = 13000;
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");
                server = new TcpListener(localAddr, port);
                server.Start();


                while (true)
                {
                    Console.Write("Waiting for a connection... ");
                    client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");
                    data = null;
                    stream = client.GetStream();
                    int i;

                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        // Translate data bytes to a ASCII string.
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        Console.WriteLine("Received: {0}", data);

                        // Process the data sent by the client.
                        data = data.ToUpper();

                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                        // Send back a response.
                        stream.Write(msg, 0, msg.Length);
                        Console.WriteLine("Sent: {0}", data);
                    }

                    main.Inserir(data);
                    client.Close();


                }
            }
            catch (WebException e)
            {
                Console.WriteLine("Can't connect with the server, try again", e);
            }
            finally
            {
                server.Stop();
            }


        }


        public void Inserir(string ceps)
        {
           
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            builder.DataSource = "sendregex.database.windows.net,1433";
            builder.UserID = "AdminRegex";
            builder.Password = "Eliemerson@123";
            builder.InitialCatalog = "RegezDataBase";

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                Console.WriteLine("\nQuery data example:");
                Console.WriteLine("=========================================\n");

               
             string sql = "INSERT INTO TABELA (id, ceps) VALUES (@id, @ceps)";
                

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                   var id = Guid.NewGuid();
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@ceps", ceps);

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();

                }

            }

        }
    }
}


