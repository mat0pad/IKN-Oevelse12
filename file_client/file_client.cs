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


			byte[] barr = new byte[1000];

			barr = System.Text.Encoding.UTF8.GetBytes("CLIENT");

			Console.WriteLine ("Sending data:\n" + Link.BytesToString(barr));

			Transport trans = new Transport (1000, "FILE_CLIENT");

			trans.send (barr, barr.Length);

			/*Console.WriteLine ("Receiving data app:");
			trans.receive (ref barr);

			Console.WriteLine ("Response:\n" + Link.BytesToString(barr));*/

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
