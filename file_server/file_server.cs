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

		string fileName{ get; set; }

		///<summary>
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
			serverSocket.Start (); //Starts serversocket

			long size;
			NetworkStream networkStream;

			while (true) {

				clientSocket = serverSocket.AcceptTcpClient (); //Accept connection with client
				Console.WriteLine (" >> Accept connection from NEW client"); 

				networkStream = clientSocket.GetStream (); //Network stream for sending and recieving data

				size = 0;

				do {
					fileName = LIB.readTextTCP (networkStream); //reads filename from client
					size = LIB.check_File_Exists (fileName); //checks if file exist
					LIB.writeTextTCP (networkStream, size.ToString ()); //Send size to client

				} while(size == 0); //Check if file exist 

				sendFile (fileName, size, networkStream);
				clientSocket.Close (); //Disconnet tcp connection
				Console.WriteLine (" >> Connection closed with THIS client");
			}

		}

		private void sendFile (String fileName, long fileSize, NetworkStream io)
		{
			Console.WriteLine ("Sending file...");
			byte[] data = File.ReadAllBytes (fileName); //Saves file content on data 

			byte[] dataLength = BitConverter.GetBytes (data.Length); //
			byte[] package = new byte[4 + data.Length]; //prepares package for send
			dataLength.CopyTo (package, 0); //copies datalength to package
			data.CopyTo (package,4); //copies data to package

			int bytesSent = 0;
			int bytesLeft = package.Length;

			while (bytesLeft > 0) { //keeps going until all bytes are send

				int nextPacketSize = (bytesLeft > BUFSIZE) ? BUFSIZE : bytesLeft;

				io.Write (package, bytesSent, nextPacketSize); //write part of package with size nextpacketSize to client.
				bytesSent += nextPacketSize;
				bytesLeft -= nextPacketSize;

			}
			Console.WriteLine ("File sended");

		}

		public static void Main (string[] args)
		{
			Console.WriteLine ("Server starts...");
			new file_server ();
		}
	}
}
