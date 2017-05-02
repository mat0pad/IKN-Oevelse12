using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Transportlaget;


namespace client
{
	class file_client
	{
		const int BUFSIZE = 1000;

		private file_client (string[] args)
		{

			string File_Name;


			byte[] barr = new byte[100];

			barr = System.Text.Encoding.UTF8.GetBytes("BAGFJSABC");

			Transport trans = new Transport (1000, "FILE_CLIENT");
			trans.send (barr, barr.Length);

			Console.WriteLine (barr.ToString());
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
