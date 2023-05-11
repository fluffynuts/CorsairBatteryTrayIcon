using System;
using System.Windows.Forms;

namespace CorsairBatteryTrayIcon
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
        }

        private void About_Load(
            object sender,
            EventArgs e
        )
        {
            var version = typeof(Program)
                .Assembly.GetName().Version;
            VersionLabel.Text = $"Version: {version.Major}.{version.Minor}.{version.Revision}";
        }

        private void DismissButton_Click(
            object sender,
            EventArgs e
        )
        {
            Hide();
        }
    }
}