using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Hoot
{
    public partial class Popup : Form
    {
        public Popup()
        {
            InitializeComponent();
        }

        // Define constant values for window styles
        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_TOPMOST = 0x00000008;
        private const int WS_EX_NOACTIVATE = 0x08000000;

        // Import the necessary functions from the user32.dll library
        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        // This allows the form to be created with topmost set to true but also not to take the focus
        // so it doesn't interrupt what you were doing when the alert popped up
        protected override CreateParams CreateParams
        {
            get
            {
                // Add the WS_EX_TOPMOST and WS_EX_NOACTIVATE styles to the window
                CreateParams createParams = base.CreateParams;
                createParams.ExStyle |= (WS_EX_TOPMOST | WS_EX_NOACTIVATE);
                return createParams;
            }
        }

        private void Popup_Load(object sender, EventArgs e)
        {
            labelTime.Text = DateTime.Now.ToString("h:mm tt");

            // position form bottom right
            Rectangle workingArea = Screen.GetWorkingArea(this);
            this.Location = new Point(workingArea.Right - Size.Width,
                                      workingArea.Bottom - Size.Height);
        }

        private void timerClose_Tick(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
