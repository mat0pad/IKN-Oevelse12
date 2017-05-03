using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Transportlaget;
using Linklaget;

namespace client
{
	class file_client
	{
		const int BUFSIZE = 1000;

		private file_client (string[] args)
		{
			string File_Name;

			var barr = System.Text.Encoding.UTF8.GetBytes("/Desktop/image.jpg");

			Console.WriteLine ("Sending data:\n" + Link.BytesToString(barr));

			Transport trans = new Transport (1000, "FILE_CLIENT");

			trans.send (barr, barr.Length);

			Console.WriteLine ("\nReceiving data app:");

			var files2Receive = new byte[1000];

			trans.receive (ref files2Receive);

			Console.WriteLine ("Response:\n" + Link.BytesToString(files2Receive));
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
