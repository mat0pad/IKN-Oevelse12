using System;
using System.IO;
using System.Net;
using System.Text;
using System.Net.Sockets;

namespace tcp
{
	class file_server
	{
		/// <summary>
		/// The PORT
		/// </summary>
		const int PORT = 9000;
		/// <summary>
		/// The BUFSIZE
		/// </summary>
		const int BUFSIZE = 1000;
		string fileName{get;set;}

		/// <summary>
		/// Initializes a new instance of the <see cref="file_server"/> class.
		/// Opretter en socket.
		/// Venter på en connect fra en klient.
		/// Modtager filnavn
		/// Finder filstørrelsen
		/// Kalder metoden sendFile
		/// Lukker socketen og programmet
		/// </summary>
		private file_server ()
		{
			TcpListener serverSocket = new TcpListener (PORT);

			TcpClient clientSocket = default(TcpClient);

			Console.WriteLine (" >> Server Started");
			serverSocket.Start ();



			//int requestCount = 0;

			while (true) {
				clientSocket = serverSocket.AcceptTcpClient ();
				Console.WriteLine (" >> Accept connection from client");
				NetworkStream networkStream = clientSocket.GetStream ();
				long size = 0;
				do
				{


					fileName = LIB.readTextTCP (networkStream);
					size = LIB.check_File_Exists (fileName); //checks if file exist
					LIB.writeTextTCP (networkStream,size.ToString());
				}while(size == 0);
				sendFile (fileName, size, networkStream);

			}
		}
		private void sendFile (String fileName, long fileSize, NetworkStream io)
		{
			byte[] data = File.ReadAllBytes (fileName); //reading data



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
			Console.WriteLine ("Server starts...");
			new file_server();
		}
	}
}
