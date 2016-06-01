using System;
using System.Windows.Forms;

namespace FlightChecker
{
	internal static class Program
	{
		/// <summary>
		/// Gets the full name of the application.
		/// </summary>
		/// <value>
		/// The full name of the application.
		/// </value>
		public const string ApplicationFullName = "Flight Checker";

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		private static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}
	}
}
