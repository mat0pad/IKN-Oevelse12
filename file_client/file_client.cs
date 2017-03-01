using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace tcp
{
	class file_client
	{
		TcpClient clientSocket = new TcpClient ();

		NetworkStream serverStream;

		const int PORT = 9000;

		const int BUFSIZE = 1000;
		//max of size send each iteration

		long size = 0;

		private file_client (string[] args)
		{
			string File_Name;

			try {

				clientSocket.Connect ("10.0.0.2", PORT);
				Console.WriteLine (" >> Client Connected to server");
				serverStream = clientSocket.GetStream ();

				while (true) {

					File_Name = "";

					while (size == 0) { //iteration until file 

						File_Name = SendRequest ();
						ReadSize ();

						if (size == 0)
							Console.WriteLine ("File dosn't exist, try again");
					}
					receiveFile (LIB.extractFileName(File_Name), serverStream);

					size = 0;
					serverStream.Flush();
					// Connect again
					clientSocket.Connect ("10.0.0.2", PORT);
					serverStream = clientSocket.GetStream ();
				}
			}
			catch(SocketException ){

				Console.WriteLine (" >> Connection closed");
				Console.WriteLine (" >> Host not found..");
			}
			catch(UnauthorizedAccessException ){

				Console.WriteLine (" >> Connection closed");
				Console.WriteLine (" >> Access to specified file was denined..");
			}
			catch (Exception e) {

				Console.WriteLine ("Connection closed..");
				Console.WriteLine (e.GetType());
				Console.WriteLine (e.StackTrace.ToString ());

			}

		}

		private string SendRequest ()
		{
			Console.WriteLine ("Write the name of the file:");
			string Request = Console.ReadLine ();
			LIB.writeTextTCP (serverStream, Request);
			return Request;
		}


		private void ReadSize ()
		{
			size = long.Parse (LIB.readTextTCP (serverStream));
			Console.WriteLine ("Size is: {0}", size);
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

				bytesRead = serverStream.Read (data, allBytesRead, nextPacketSize);
				allBytesRead += bytesRead;
				bytesLeft -= bytesRead;

			}

			Console.WriteLine ("File recieved");
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
