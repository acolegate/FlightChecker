using System;
using System.Windows.Forms;

namespace FlightChecker
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            InitialiseTimer();
        }

        private void MainForm_Activated(object sender, EventArgs e)
        {
            if (WindowState != FormWindowState.Minimized)
            {
                Show();
            }
        }

        /// <summary>
        /// Handles the Load event of the MainForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            RefreshMessage();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                ShowInTaskbar = false;
                notifyIcon.Visible = true;
            }
        }

        /// <summary>
        /// Handles the Click event of the minimiseButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void MinimiseButton_Click(object sender, EventArgs e)
        {
            HideWindow();
        }

        /// <summary>
        /// Handles the Click event of the quitButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void QuitButton_Click(object sender, EventArgs e)
        {
            CloseApp();
        }

        /// <summary>
        /// Handles the Click event of the quitStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void QuitStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseApp();
        }

        /// <summary>
        /// Handles the Click event of the refreshButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void RefreshButton_Click(object sender, EventArgs e)
        {
            RefreshMessage();
        }

        /// <summary>
        /// Handles the Click event of the RefreshToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void RefreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshMessage();
        }

        /// <summary>
        /// Handles the Click event of the restoreToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void RestoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RestoreWindow();
        }

        private void notifyIcon_Click(object sender, EventArgs e)
        {
            RefreshMessage();
        }

        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            RestoreWindow();
        }
    }
}
