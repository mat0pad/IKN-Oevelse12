using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace tcp
{
	class file_client
	{

		const int PORT = 9000;

		const int BUFSIZE = 1000;
		//max of size send each iteration

		long size = 0;

		private file_client (string[] args)
		{
			string File_Name;


		}

		private string SendRequestFromInput ()
		{

		}

		private string SendRequestFromArg (string req)
		{
			
		}


		private void ReadSize ()
		{
		}

		private void receiveFile (String fileName, NetworkStream io)
		{

			int allBytesRead = 0; //
			Console.WriteLine ("Receiving file..");

			int bytesRead = 0; //serverStream.Read (length, 0, 5);

			// Read the data
			int bytesLeft = (int)size;
			byte[] data = new byte[size]; //var where file data get saved


			while (bytesLeft > 0) { //iterate until all bytes have been send

				int nextPacketSize = (bytesLeft > BUFSIZE) ? BUFSIZE : bytesLeft;

				bytesRead = io.Read (data, allBytesRead, nextPacketSize);
				allBytesRead += bytesRead;
				bytesLeft -= bytesRead;

			}

			Console.WriteLine ("File recieved");
			Console.WriteLine ("Saving file..");
			File.WriteAllBytes ("/root/Desktop/" + fileName, data); //saves file on Desktop
			Console.WriteLine (fileName + " Saved on Desktop");
		}

		public static void Main (string[] args)
		{
			Console.WriteLine ("Client starts...");
			new file_client (args);
		}
	}
}
