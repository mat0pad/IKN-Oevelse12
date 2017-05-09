using System;
using System.IO;
using System.Net;
using System.Text;
using System.Net.Sockets;
using Linklaget;
using Transportlaget;

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
			
			long size  = 0;
			Console.WriteLine (" >> Server Started");
			//Link link = new Link (1000,"FILE_SERVER");

			Transport trans = new Transport(1000,"FILE_SERVER");

			while (true) {

				Console.WriteLine (" >> Accept connection from NEW client"); 

				size = 0;

				do {

					// Reads filename from client
					fileName = receiveFilename(trans);

					// Send file size to client
					size = sendFileSize(trans, fileName);

				} while(size == 0); //Check if file exist 
					

				// Send the actual file to client
				sendFile (trans, fileName, size);

				Console.WriteLine (" >> Connection closed with THIS client");
				//break;
			}
		}

		private string receiveFilename(Transport trans)
		{
			byte[] receiver = new byte[BUFSIZE];

			//reads filename from client
			trans.receive(ref receiver);

			string name = System.Text.Encoding.UTF8.GetString (receiver);

			Console.WriteLine ("Filname received:\n" + name);

			return name;
		}


		private long sendFileSize(Transport trans,string fileName)
		{
			//checks if file exist
			long size = LIB.check_File_Exists (fileName); 

			//Send size to client
			Console.WriteLine ("\nSending size:");

			var sizeInBytes = System.Text.Encoding.UTF8.GetBytes(size.ToString());

			trans.send (sizeInBytes, sizeInBytes.Length);

			return size;
		}

		private void sendFile(Transport trans,string fileName, long size)
		{
			Console.WriteLine ("Sending file...");

			// Saves file content on data 
			byte[] data = File.ReadAllBytes (fileName); 

			// Prepares package for send
			byte[] package = new byte[size]; 

			// Copies data to package
			data.CopyTo (package,0); 

			int bytesSent = 0;
			int bytesLeft = (int)size;

			while (bytesLeft > 0) { //keeps going until all bytes are send

				int nextPacketSize = (bytesLeft > BUFSIZE) ? BUFSIZE : bytesLeft;

				// Send part of package with size nextpacketSize to client.
				trans.send (package, nextPacketSize); 

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
