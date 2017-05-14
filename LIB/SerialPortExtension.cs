using System;
using System.Diagnostics;
using System.IO.Ports;
using System.Threading;


namespace test
{
	public static class SerialPortExtension {


		public static void WaitForBytes(this SerialPort port, int count, int millisecondsTimeout, Action timeoutFunc) {

			if (port == null) throw new ArgumentNullException("port");
			if (port.BytesToRead >= count) return;

			using (var watcher = new SerialPortWatcher(port)) {
				watcher.WaitForBytes(count, millisecondsTimeout, timeoutFunc);
			}

		}
			
		public static void WaitForBytes(this SerialPort port, int count, Action timeoutFunc) {
			if (port == null) throw new ArgumentNullException("port");
			WaitForBytes(port, count, port.ReadTimeout, timeoutFunc);
		}

	}

	/// <summary>
	/// Watches for incoming bytes on a serial port and provides a reliable method to wait for a given
	/// number of bytes in a synchronous communications algorithm.
	/// </summary>
	class SerialPortWatcher : IDisposable {

		// This class works primarilly by watching for the SerialPort.DataReceived event.  However, since
		// that event is not guaranteed to fire, it is neccessary to also periodically poll for new data.

		private const int POLL_MS = 30;

		private AutoResetEvent dataArrived = new AutoResetEvent(false);
		private SerialPort port;

		public SerialPortWatcher(SerialPort port) {
			if (port == null) throw new ArgumentNullException("port");
			this.port = port;
			this.port.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
		}

		public void Dispose() {
			if (port != null) {
				port.DataReceived -= port_DataReceived;
				port = null;
			}
			if (dataArrived != null) {
				dataArrived.Dispose();
				dataArrived = null;
			}
		}

		void port_DataReceived(object sender, SerialDataReceivedEventArgs e) {

			if (dataArrived != null) 
				dataArrived.Set();

		}
			
		public void WaitForBytes(int count, int millisecondsTimeout, Action timeoutFunc) {

			if (count < 0) throw new ArgumentOutOfRangeException("count");
			if (millisecondsTimeout < 0) throw new ArgumentOutOfRangeException("millisecondsTimeout");
			if (port == null) throw new InvalidOperationException("SerialPortWatcher has been disposed.");
			if (!port.IsOpen) throw new InvalidOperationException("Port is closed");

			if (port.BytesToRead >= count) return;

			DateTime expire = DateTime.Now.AddMilliseconds(millisecondsTimeout);

			// Wait for the specified number of bytes to become available.  This is done primarily by
			// waiting for a signal from the thread which handles the DataReceived event.  However, since
			// that event isn't guaranteed to fire, we also poll for new data every POLL_MS milliseconds.
			while (port.BytesToRead < count) {
				if (DateTime.Now >= expire) {
					timeoutFunc ();
					/*throw new TimeoutException(String.Format(
						"Timed out waiting for data from port {0}", port.PortName));*/
				}
				WaitForSignal();
			}

		}

		// Timeout exceptions are expected to be thrown in this block of code, and are perfectly normal.
		// A separate method is used so it can be marked with DebuggerNonUserCode, which will cause the
		// debugger to ignore these exceptions (even if Thrown is checkmarked under Debug | Exceptions).
		[DebuggerNonUserCode]
		private void WaitForSignal() {
			try {
				dataArrived.WaitOne(POLL_MS);
			} catch (TimeoutException) { }
		}

	}
}

