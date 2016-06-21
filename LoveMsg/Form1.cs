using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace LoveMsg
{
    public partial class Form1 : Form
    {
        private Settings settings;
        public Form1()
        {
            InitializeComponent();
            label1.Parent = pictureBox1;
            label1.BackColor = Color.Transparent;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            settings = new Settings("settings.ini");
            HandleFontChange();
            HandleResize();
            this.Left = settings.GetInt("X", (Screen.PrimaryScreen.Bounds.Width - 150));
            this.Top = settings.GetInt("Y", 10);
            this.Form1_MouseLeave(null, null);
        }
        private void HandleResize()
        {
            pictureBox1.Height = label1.Height + 2 * label1.Top;
            pictureBox1.Width = label1.Width + 2 * label1.Left;
            this.Size = pictureBox1.Size;
        }
        private void HandleFontChange()
        {
            var fontFamily = new FontFamily(settings.Get("fontFamily", "Arial"));
            var fontSize = settings.GetFloat("fontSize", 12);
            var font = new Font(fontFamily, fontSize);
            label1.Font = font;
        }
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
                settings.Set("X", this.Left.ToString(), false);
                settings.Set("Y", this.Top.ToString(), false);
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            settings.Save();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_MouseEnter(object sender, EventArgs e)
        {
            this.Opacity = settings.GetDouble("OpacityEnter", 0.8);
        }

        private void Form1_MouseLeave(object sender, EventArgs e)
        {
            this.Opacity = settings.GetDouble("OpacityLeave", 0.3);
        }
    }
}
