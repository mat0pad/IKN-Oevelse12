using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace tcp
{
	class file_client
	{
		TcpClient clientSocket = new TcpClient();

		NetworkStream serverStream;

		const int PORT = 9000;

		const int BUFSIZE = 1000;

		private file_client (string[] args)
		{
			
			clientSocket.Connect("10.0.0.2", PORT);
			Console.WriteLine ("Client Connected to server");
			serverStream = clientSocket.GetStream ();

			string Request = SendRequest ();
			receiveFile (Request, serverStream);

		}
			
		private string SendRequest()
		{
			Console.WriteLine ("Write the name of file:");
			string Request = Console.ReadLine ();
			LIB.writeTextTCP (serverStream, Request);
			return Request;
		}


		private long ReadSize()
		{
			long size = long.Parse (LIB.readTextTCP (serverStream));
			Console.WriteLine ("Size is: {0}", size);
			return size;
		}

		private void receiveFile (String fileName, NetworkStream io)
		{
			
			long file_size = ReadSize();
			int allBytesRead = 0;
			Console.WriteLine ("Receiving file..");


			byte[] length = new byte[file_size];
			int bytesRead = serverStream.Read(length, 0, 4);
			int dataLength = BitConverter.ToInt32(length,0);

			// Read the data
			int bytesLeft = dataLength;
			byte[] data = new byte[dataLength];


			while (bytesLeft > 0)
			{

				int nextPacketSize = (bytesLeft > BUFSIZE) ? BUFSIZE : bytesLeft;

				bytesRead = serverStream.Read(data, allBytesRead, nextPacketSize);
				allBytesRead += bytesRead;
				bytesLeft -= bytesRead;

			}

			Console.WriteLine ("File recieved");
			File.WriteAllBytes("/root/Desktop/"+fileName, data);
			Console.WriteLine (fileName + " Saved on Desktop");
		}
			
		public static void Main (string[] args)
		{
			Console.WriteLine ("Client starts...");
			new file_client(args);
		}
	}
}
