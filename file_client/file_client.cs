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
		/// <summary>
		/// The PORT.
		/// </summary>
		const int PORT = 9000;
		/// <summary>
		/// The BUFSIZE.
		/// </summary>
		const int BUFSIZE = 1000;

		/// <summary>
		/// Initializes a new instance of the <see cref="file_client"/> class.
		/// </summary>
		/// <param name='args'>
		/// The command-line arguments. First ip-adress of the server. Second the filename
		/// </param>
		private file_client (string[] args)
		{
			
			clientSocket.Connect("10.0.0.2", PORT);
			Console.WriteLine ("Client Connected to server");
			serverStream = clientSocket.GetStream ();

			SendRequest ();
			long file_size = ReadSize ();

		}



		private void SendRequest()
		{
			Console.WriteLine ("Write the name of file:");
			string Request = Console.ReadLine ();
			LIB.writeTextTCP (serverStream, Request);
		}


		private long ReadSize()
		{
			long size = long.Parse (LIB.readTextTCP (serverStream));
			Console.WriteLine ("Size is: {0}", size);
			return size;
		}
		/// <summary>
		/// Receives the file.
		/// </summary>
		/// <param name='fileName'>
		/// File name.
		/// </param>
		/// <param name='io'>
		/// Network stream for reading from the server
		/// </param>
		private void receiveFile (String fileName, NetworkStream io)
		{
			// TO DO Your own code
		}

		/// <summary>
		/// The entry point of the program, where the program control starts and ends.
		/// </summary>
		/// <param name='args'>
		/// The command-line arguments.
		/// </param>
		public static void Main (string[] args)
		{
			Console.WriteLine ("Client starts...");
			new file_client(args);
		}
	}
}
