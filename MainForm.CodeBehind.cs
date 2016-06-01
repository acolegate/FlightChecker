using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace FlightChecker
{
    public partial class MainForm
    {
        private const int MillisecondsPerMinute = 60000;

        private static readonly string TempFileName = ApplicationPath + @"\LastMessage.txt";

        /// <summary>
        /// Gets the application path.
        /// </summary>
        private static string ApplicationPath
        {
            get { return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); }
        }

        /// <summary>
        /// Gets the get previous value.
        /// </summary>
        private static string GetPreviousValue
        {
            get
            {
                string returnValue = string.Empty;

                if (File.Exists(TempFileName))
                {
                    using (StreamReader streamReader = File.OpenText(TempFileName))
                    {
                        returnValue = streamReader.ReadLine();
                    }
                }

                return returnValue;
            }
        }

        /// <summary>
        /// Writes the message to temp file.
        /// </summary>
        /// <param name="messageFromWebsite">
        /// The message from website.
        /// </param>
        private static void WriteMessageToTempFile(string messageFromWebsite)
        {
            using (TextWriter textWriter = new StreamWriter(TempFileName, false))
            {
                textWriter.WriteLine(messageFromWebsite);
                textWriter.Flush();
                textWriter.Close();
            }
        }

        /// <summary>
        /// Closes the app.
        /// </summary>
        private void CloseApp()
        {
            timer.Stop();

            if (MessageBox.Show(this, "Are you sure you want to quit?", Program.ApplicationFullName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                Close();
            }
            else
            {
                timer.Start();
            }
        }

        /// <summary>
        /// Hides the window.
        /// </summary>
        private void HideWindow()
        {
            WindowState = FormWindowState.Minimized;
        }

        /// <summary>
        /// Initialises the timer.
        /// </summary>
        private void InitialiseTimer()
        {
            timer.Interval = int.Parse(ConfigurationManager.AppSettings["pollintervalminutes"]) * MillisecondsPerMinute;
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        //Deactivate event handler for your form.
        private void MainForm_Deactivate(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Hide();
            }
        }

        /// <summary>
        /// Refreshes the message.
        /// </summary>
        /// <param name="alwaysShowBaloonTip">
        /// if set to <c>true</c> [always show baloon tip].
        /// </param>
        private void RefreshMessage(bool alwaysShowBaloonTip = true)
        {
            textBox.Text = "Checking...";
            Application.DoEvents();

            string previousValue = GetPreviousValue;

            ResponseFromWebsite response = WebHelper.MessageFromWebsite;

            WriteMessageToTempFile(response.Message);

            textBox.Text = response.Message;

            if (WindowState == FormWindowState.Minimized)
            {
                if (alwaysShowBaloonTip)
                {
                    notifyIcon.ShowBalloonTip(10, Program.ApplicationFullName, response.Message, ToolTipIcon.Info);
                }

                if (response.Success && response.FurthestDate != null)
                {
                    if (string.IsNullOrEmpty(previousValue) == false && previousValue != response.Message)
                    {
                        DatesHaveChangedForm datesHaveChangedForm = new DatesHaveChangedForm((DateTime)response.FurthestDate);
                        datesHaveChangedForm.ShowDialog(this);
                    }
                }
            }
        }

        /// <summary>
        /// Restores the window.
        /// </summary>
        private void RestoreWindow()
        {
            WindowState = FormWindowState.Normal;
            Show();
            BringToFront();
            Focus();
            RefreshMessage(false);
        }

        /// <summary>
        /// Handles the Tick event of the timer control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        private void Timer_Tick(object sender, EventArgs e)
        {
            RefreshMessage(false);
        }
    }
}