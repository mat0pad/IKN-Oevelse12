using System;
using System.IO;
using System.Net;
using System.Text;
using System.Net.Sockets;
using Linklaget;

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
			
			var size  = 0;
			Console.WriteLine (" >> Server Started");
			Link link = new Link (1000,"FILE_SERVER");

			while (true) {

				Console.WriteLine (" >> Accept connection from NEW client"); 

				size = 0;

				do {

					//reads filename from client
					//size = LIB.check_File_Exists (fileName); //checks if file exist
					//Send size to client
					String test = "0101AFBGA"; // ABCFBDGBCA
					byte[] toSend = Encoding.ASCII.GetBytes(test);
					link.send(toSend, toSend.Length);
					size = 1;

				} while(size == 0); //Check if file exist 


				Console.WriteLine (" >> Connection closed with THIS client");
				break;
			}

		}

		private void sendFile (String fileName, long fileSize)
		{
			Console.WriteLine ("Sending file...");
			byte[] data = File.ReadAllBytes (fileName); //Saves file content on data 


			byte[] package = new byte[fileSize]; //prepares package for send

			data.CopyTo (package,0); //copies data to package

			int bytesSent = 0;
			int bytesLeft = (int)fileSize;

			while (bytesLeft > 0) { //keeps going until all bytes are send

				int nextPacketSize = (bytesLeft > BUFSIZE) ? BUFSIZE : bytesLeft;

				//io.Write (package, bytesSent, nextPacketSize); //write part of package with size nextpacketSize to client.
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
