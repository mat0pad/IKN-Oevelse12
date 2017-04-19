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
				if(APP.Equals("FILE_SERVER"))
				{
					serialPort = new SerialPort("/dev/ttyS1",115200,Parity.None,8,StopBits.One);
				}
				else
				{
					serialPort = new SerialPort("/dev/ttyS1",115200,Parity.None,8,StopBits.One);
				}
			#else
				serialPort = new SerialPort("/dev/ttyS1",115200,Parity.None,8,StopBits.One);
			#endif
			if(!serialPort.IsOpen)
				serialPort.Open();

			buffer = new byte[(BUFSIZE*2)];
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
<<<<<<< HEAD
			Array.Copy(buf,0,buffer,0,size);

			var counter = 5;
			buffer [4] = DELIMITERA;
=======
			if (size > BUFFER_SIZE)
				throw new System.ArgumentException(@"Parameter cannot be larger than set buffersize of {BUFFER_SIZE}, was {size}", "size");

			var counter = 0;
>>>>>>> bb0c81219631f7699600d5158b8289866c0ce645

			for (int i = 4 ; i < size; i++) {
			
				if (buf [i].Equals (DELIMITERA)) {

					buffer[counter] = DELIMITERB;
					buffer[counter+1] = DELIMITERC;

					counter += 2;
				} 
				else if (buf [i].Equals (DELIMITERB)) {

					buffer[counter] = DELIMITERB;
					buffer[counter+1] = DELIMITERD;
					counter += 2;
				} 
				else {
					buffer[counter] = buf [i];
					counter++;
				}
			}

<<<<<<< HEAD
			buffer [counter] = DELIMITERA;

			Console.WriteLine (System.Text.Encoding.UTF8.GetString(buffer));
			//serialPort.Write('A' + tempBuf.ToString() + 'A');
=======
			Console.WriteLine ('A' + System.Text.Encoding.UTF8.GetString(buffer) + 'A');
			serialPort.Write('A' + buffer.ToString() + 'A');
>>>>>>> bb0c81219631f7699600d5158b8289866c0ce645
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
			var numOfBytes = serialPort.BytesToRead;

			var tempBuf = new byte[BUFFER_SIZE];
			var returnBuf = new byte[BUFFER_SIZE];

			if (numOfBytes > BUFFER_SIZE) {
				serialPort.Read (tempBuf, 0, BUFFER_SIZE);
				numOfBytes = BUFFER_SIZE;
			}
			else {
				serialPort.Read (tempBuf, 0, numOfBytes);
			}

			var counter = 0; 

			// i = 1 to remove A start
			for (int i = 1; i < numOfBytes; i++) {

				if (tempBuf[i].Equals(DELIMITERB) && tempBuf[i+1].Equals(DELIMITERC)) {

					returnBuf [counter] = DELIMITERA;
					++counter;
				} 
				else if (tempBuf[i].Equals(DELIMITERB) && tempBuf[i+1].Equals(DELIMITERD)) {

					returnBuf[counter] = DELIMITERB;
					++counter;
				} 
				else {
					
					returnBuf[counter] = buf [i];
					++counter;
				}

				if (i == numOfBytes - 1)
					break;
			}

	    	
			buf = returnBuf;

			return counter+1;
		}
	}
}
