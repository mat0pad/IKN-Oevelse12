using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace tcp
{
	class file_client
	{
		const int BUFSIZE = 1000;

		private file_client (string[] args)
		{
			byte[] barr = new byte[1000];

			Linklaget.Link link = new Linklaget.Link (1000, "FILE_CLIENT");
			link.receive (barr);

			Console.WriteLine (barr.ToString);
		}

		private void establishConnection()
		{
			// Establish connection to server
		}

		private string SendFileRequest (string req)
		{
			// Send filename to server
		}


		private int ReadSize ()
		{
			// Read filesize send from server
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
