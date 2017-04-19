using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace tcp
{
	class file_client
	{
<<<<<<< HEAD

		const int PORT = 9000;

=======
>>>>>>> bb0c81219631f7699600d5158b8289866c0ce645
		const int BUFSIZE = 1000;

		private file_client (string[] args)
		{
<<<<<<< HEAD
			string File_Name;

=======
			byte[] barr = new byte[1000];

			Linklaget.Link link = new Linklaget.Link (1000, "FILE_CLIENT");
			link.receive (barr);
>>>>>>> bb0c81219631f7699600d5158b8289866c0ce645

			Console.WriteLine (barr.ToString);
		}

		private void establishConnection()
		{
<<<<<<< HEAD

=======
			// Establish connection to server
>>>>>>> bb0c81219631f7699600d5158b8289866c0ce645
		}

		private string SendFileRequest (string req)
		{
<<<<<<< HEAD
			
=======
			// Send filename to server
>>>>>>> bb0c81219631f7699600d5158b8289866c0ce645
		}


		private int ReadSize ()
		{
<<<<<<< HEAD
=======
			// Read filesize send from server
>>>>>>> bb0c81219631f7699600d5158b8289866c0ce645
		}

		private void receiveFile (String fileName, NetworkStream io)
		{
			// Receive file
		}

		public static void Main (string[] args)
		{
			Console.WriteLine ("Client starts...");
			new file_client (args);
		}
	}
}
