using System;
using System.IO.Ports;

/// <summary>
/// Link.
/// </summary>
namespace Linklaget
{
	/// <summary>
	/// Link.
	/// </summary>
	public class Link
	{
		/// <summary>
		/// The DELIMITE for slip protocol.
		/// </summary>
		const byte DELIMITERA = (byte)'A';

		const byte DELIMITERB = (byte)'B';

		const byte DELIMITERC = (byte)'C';

		const byte DELIMITERD = (byte)'D';
		/// <summary>
		/// The buffer for link.
		/// </summary>
		private byte[] buffer;

		private int BUFFER_SIZE;

		/// <summary>
		/// The serial port.
		/// </summary>
		SerialPort serialPort;

		/// <summary>
		/// Initializes a new instance of the <see cref="link"/> class.
		/// </summary>
		public Link (int BUFSIZE, string APP)
		{
			// Create a new SerialPort object with default settings.
			#if DEBUG
			if (APP.Equals ("FILE_SERVER")) {
				serialPort = new SerialPort ("/dev/ttyS1", 115200, Parity.None, 8, StopBits.One);
			} else {
				serialPort = new SerialPort ("/dev/ttyS1", 115200, Parity.None, 8, StopBits.One);
			}
			#else
				serialPort = new SerialPort("/dev/ttyS1",115200,Parity.None,8,StopBits.One);
			#endif
			if (!serialPort.IsOpen)
				serialPort.Open ();

			buffer = new byte[(BUFSIZE * 2)];
			BUFFER_SIZE = BUFSIZE;

			// Uncomment the next line to use timeout
			//serialPort.ReadTimeout = 500;

			serialPort.DiscardInBuffer ();
			serialPort.DiscardOutBuffer ();
		}

		/// <summary>
		/// Send the specified buf and size.
		/// </summary>
		/// <param name='buf'>
		/// Buffer.
		/// </param>
		/// <param name='size'>
		/// Size.
		/// </param>
		public void send (byte[] buf, int size)
		{
			if (size > BUFFER_SIZE)

				throw new System.ArgumentException(@"Parameter cannot be larger than set buffersize of {BUFFER_SIZE}, was {size}", "size");

			byte[] sendBuf = new byte[size*2];

			Array.Copy(buf,0,sendBuf,0,size);

			var counter = 5;

			sendBuf [4] = DELIMITERA;

			for (int i = 4 ; i < size; i++) {
			
				if (buf [i].Equals (DELIMITERA)) {

					sendBuf[counter] = DELIMITERB;
					sendBuf[counter+1] = DELIMITERC;

					counter += 2;
				} else if (buf [i].Equals (DELIMITERB)) {
					
					sendBuf[counter] = DELIMITERB;
					sendBuf[counter+1] = DELIMITERD;
					counter += 2;
				} 
				else {
					sendBuf[counter] = buf [i];
					counter++;
				}
			}
				
			sendBuf [counter] = DELIMITERA;

			byte[] buf2Send = new byte[counter+1];

			Array.Copy (sendBuf,0, buf2Send, 0, counter+1);

		//	Console.WriteLine ("Link send data:\n" + System.Text.Encoding.UTF8.GetString(buf2Send).Substring(4));

			byte[] test = new byte[4];
			test [0] = buf2Send [0];
			test [1] = buf2Send [1];
			test [2] = buf2Send [2];
			test [3] = buf2Send [3];


		//	Console.WriteLine ("Link send:\n" + BytesToString(buf2Send));

			serialPort.Write (buf2Send,0,buf2Send.Length);
		}

		public static string BytesToString (byte[] byteArray)
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder ("{ ");
			for (var i = 0; i < byteArray.Length; i++) {
				var b = byteArray [i];
				sb.Append (b);
				if (i < byteArray.Length - 1) {
					sb.Append (", ");
				}
			}
			sb.Append (" }");
			return sb.ToString ();
		}

		/// <summary>
		/// Receive the specified buf and size.
		/// </summary>
		/// <param name='buf'>
		/// Buffer.
		/// </param>
		/// <param name='size'>
		/// Size.
		/// </param>
		public int receive (ref byte[] buf)
		{
			int numOfBytes;

			do {
				numOfBytes = serialPort.BytesToRead;
			} while (numOfBytes == 0);


			var tempBuf = new byte[BUFFER_SIZE * 2];
			var returnBuf = new byte[BUFFER_SIZE];

			if (numOfBytes > BUFFER_SIZE * 2) {
				serialPort.Read (tempBuf, 0, BUFFER_SIZE);
				numOfBytes = BUFFER_SIZE * 2;
			} else {
				serialPort.Read (tempBuf, 0, numOfBytes);
			}

			returnBuf [0] = tempBuf [0];
			returnBuf [1] = tempBuf [1];
			returnBuf [2] = tempBuf [2];
			returnBuf [3] = tempBuf [3];

			var counter = 4; 

			// i = 5 to remove A start
			for (int i = 5; i < numOfBytes; i++) {

				if (tempBuf [i].Equals (DELIMITERA)) {

					if (i == numOfBytes - 1)
						break;
				} 
				else {
					if (tempBuf [i].Equals (DELIMITERB) && tempBuf [i + 1].Equals (DELIMITERC)) {

						returnBuf [counter] = DELIMITERA;
						++counter;
					} else if (tempBuf [i].Equals (DELIMITERB) && tempBuf [i + 1].Equals (DELIMITERD)) {

						returnBuf [counter] = DELIMITERB;
						++counter;
					} else {
					
						returnBuf [counter] = tempBuf [i];
						++counter;
					}
				}
			}

			byte[] buf2Receive = new byte[counter];

			Array.Copy (returnBuf,0, buf2Receive, 0, counter); 

			//Console.WriteLine ("Link receive:\n" + BytesToString (buf2Receive));
	    	
			buf = buf2Receive;

			return counter;
		}
	}
}
