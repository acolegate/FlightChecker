using System;
using System.Configuration;
using System.Diagnostics;
using System.Windows.Forms;

namespace FlightChecker
{
    public partial class DatesHaveChangedForm : Form
    {
        /// <summary>Initializes a new instance of the <see cref="DatesHaveChangedForm"/> class.</summary>
        /// <param name="updatedDate">The updated date.</param>
        public DatesHaveChangedForm(DateTime updatedDate)
        {
            InitializeComponent();

            label2.Text = updatedDate.ToString("dddd d MMMM yyyy");
        }

        /// <summary>Handles the Click event of the CloseButton control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void CloseButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>Handles the Click event of the BrowseWebsiteButton control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void BrowseWebsiteButton_Click(object sender, EventArgs e)
        {
            Process.Start(ConfigurationManager.AppSettings["urltobrowseonchange"]);
            Close();
        }
    }
}
