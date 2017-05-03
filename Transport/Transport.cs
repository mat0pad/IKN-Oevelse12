using System;
using Linklaget;

/// <summary>
/// Transport.
/// </summary>
namespace Transportlaget
{
	/// <summary>
	/// Transport.
	/// </summary>
	public class Transport
	{
		/// <summary>
		/// The link.
		/// </summary>
		private Link link;
		/// <summary>
		/// The 1' complements checksum.
		/// </summary>
		private Checksum checksum;
		/// <summary>
		/// The buffer.
		/// </summary>
		private byte[] buffer;
		/// <summary>
		/// The seq no.
		/// </summary>
		private byte seqNo;
		/// <summary>
		/// The old_seq no.
		/// </summary>
		private byte old_seqNo;
		/// <summary>
		/// The error count.
		/// </summary>
		private int errorCount;
		/// <summary>
		/// The DEFAULT_SEQNO.
		/// </summary>
		private const int DEFAULT_SEQNO = 2;

		private int BUFFER_SIZE = 0;
		/// <summary>
		/// The data received. True = received data in receiveAck, False = not received data in receiveAck
		/// </summary>
		private bool dataReceived;
		/// <summary>
		/// The number of data the recveived.
		/// </summary>
		private int recvSize = 0;

		/// <summary>
		/// Initializes a new instance of the <see cref="Transport"/> class.
		/// </summary>
		public Transport (int BUFSIZE, string APP)
		{
			link = new Link(BUFSIZE+(int)TransSize.ACKSIZE, APP);
			checksum = new Checksum();
			buffer = new byte[BUFSIZE+(int)TransSize.ACKSIZE];
			seqNo = 0;
			old_seqNo = DEFAULT_SEQNO;
			errorCount = 0;
			dataReceived = false;
			BUFFER_SIZE = BUFSIZE;
		}

		/// <summary>
		/// Receives the ack.
		/// </summary>
		/// <returns>
		/// The ack.
		/// </returns>
		private bool receiveAck()
		{
			recvSize = link.receive(ref buffer);
			dataReceived = true;

			if (recvSize == (int)TransSize.ACKSIZE) {
				dataReceived = false;
				if (!checksum.checkChecksum (buffer, (int)TransSize.ACKSIZE) ||
				  buffer [(int)TransCHKSUM.SEQNO] != seqNo ||
				  buffer [(int)TransCHKSUM.TYPE] != (int)TransType.ACK)
				{
					return false;
				}
				seqNo = (byte)((buffer[(int)TransCHKSUM.SEQNO] + 1) % 2);
			}
 
			return true;
		}

		/// <summary>
		/// Sends the ack.
		/// </summary>
		/// <param name='ackType'>
		/// Ack type.
		/// </param>
		private void sendAck (bool ackType)
		{
			byte[] ackBuf = new byte[(int)TransSize.ACKSIZE];
			ackBuf [(int)TransCHKSUM.SEQNO] = (byte)
				(ackType ? (byte)buffer [(int)TransCHKSUM.SEQNO] : (byte)(buffer [(int)TransCHKSUM.SEQNO] + 1) % 2);
			ackBuf [(int)TransCHKSUM.TYPE] = (byte)(int)TransType.ACK;
			checksum.calcChecksum (ref ackBuf, (int)TransSize.ACKSIZE);
			link.send(ackBuf, (int)TransSize.ACKSIZE);
		}

		/// <summary>
		/// Send the specified buffer and size.
		/// </summary>
		/// <param name='buffer'>
		/// Buffer.
		/// </param>
		/// <param name='size'>
		/// Size.
		/// </param>
		public void send(byte[] buf, int size)
		{
			if(size <= BUFFER_SIZE){

				int newSize = size + 4;

				// set seq to not old seq
				seqNo = (byte)((old_seqNo + 1) % 2);

				// Copy to buffer
				Array.Copy (buf, 0, buffer, 4, size);

				// Set type
				buffer[(int) TransCHKSUM.TYPE] = (byte)TransType.DATA;

				// Set seqno
				buffer[(int) TransCHKSUM.SEQNO] = (byte)seqNo;

				// Calculate sum and low & high to index 0,1 ...
				checksum.calcChecksum (ref buffer, newSize);

				// TODO: Remove
				byte[] test = new byte[4];
				test [0] = buffer [0];
				test [1] = buffer [1];
				test [2] = buffer [2];
				test [3] = buffer [3];

				Console.WriteLine("Transport sending header:\n" + Link.BytesToString (test));

				// Send it through link layer
				link.send (buffer, newSize);

				// Receive ack or resend
				while (!receiveAck ()) {
					// Send it through link layer
					link.send (buffer, newSize);
				}
			}
			else
				throw new ArgumentOutOfRangeException("Size is bigger than " + BUFFER_SIZE);
		}

		/// <summary>
		/// Receive the specified buffer.
		/// </summary>
		/// <param name='buffer'>
		/// Buffer.
		/// </param>
		public int receive (ref byte[] buf)
		{
			while (true) {

				// Receive size
				recvSize = link.receive (ref buffer);

				seqNo = buffer[(int)TransCHKSUM.SEQNO];


				// Send Ack
				if (checksum.checkChecksum (buffer, recvSize) && seqNo != old_seqNo) {

					// Set seq
					old_seqNo = seqNo;

					// Send ack
					sendAck (true);

					break;
				} else {
					
					// Ack for resend
					sendAck(true);
				}

				// TODO: Remove

				byte[] test = new byte[4];
				test [0] = buffer [0];
				test [1] = buffer [1];
				test [2] = buffer [2];
				test [3] = buffer [3];


				Console.WriteLine("Transport receiving header:\n" + Link.BytesToString (test));


				break;
			}


			Array.Copy (buffer, 4, buf, 0, buffer.Length-4);
			//buf = buffer.

			return recvSize-4;
		}
	}
}