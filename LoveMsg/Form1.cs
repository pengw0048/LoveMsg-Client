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
        public Settings settings;
        private bool showHeart = false;
        private bool showForm = false;
        private DateTime startDate;
        public Form1()
        {
            InitializeComponent();
            label1.Parent = pictureBox1;
            label1.BackColor = Color.Transparent;
            pictureBox2.Parent = pictureBox1;
            pictureBox2.BackColor = Color.Transparent;
            pictureBox2.BringToFront();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            settings = new Settings("settings.ini");
            label1.Text = DaysBetween().ToString();
            HandleFontChange();
            HandleResize();
            this.Left = settings.GetInt("X", (Screen.PrimaryScreen.Bounds.Width - 150));
            this.Top = settings.GetInt("Y", 10);
            timer2_Tick(null, null);
        }
        private void HandleResize()
        {
            pictureBox1.Height = label1.Height + 2 * label1.Top;
            if (showHeart)
            {
                pictureBox2.Height = pictureBox1.Height-6;
                pictureBox2.Width = pictureBox2.Height;
                pictureBox2.Left = label1.Width + label1.Left;
                pictureBox2.Visible = true;
                pictureBox1.Width = pictureBox2.Left + pictureBox2.Width+4;
            }
            else
            {
                pictureBox1.Width = label1.Width + 2 * label1.Left;
                pictureBox2.Visible = false;
            }
            this.Size = pictureBox1.Size;
        }
        private void HandleFontChange()
        {
            var fontFamilyName = settings.Get("fontFamily", "");
            FontFamily fontFamily=FontFamily.GenericMonospace;
            try { fontFamily = new FontFamily(fontFamilyName); } catch (Exception) { }
            var fontSize = settings.GetFloat("fontSize", 12);
            var fontBold = settings.Get("fontBold", "true") != "false";
            var font = new Font(fontFamily, fontSize, fontBold ? FontStyle.Bold : FontStyle.Regular);
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
            showForm = true;
            showHeart = true;
            HandleResize();
            this.Opacity = settings.GetDouble("OpacityEnter", 0.8);
        }

        private void Form1_MouseLeave(object sender, EventArgs e)
        {
            showForm = false;
            timer2.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = DaysBetween().ToString();
            HandleResize();
        }
        private int DaysBetween()
        {
            startDate = settings.GetDate("startDate", DateTime.Today);
            return (DateTime.Now - startDate).Days + 1;
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            new Form2(this).ShowDialog();
            label1.Text = DaysBetween().ToString();
            HandleFontChange();
            HandleResize();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            timer2.Enabled = false;
            if (showForm) return;
            this.Opacity = settings.GetDouble("OpacityLeave", 0.3);
            showHeart = false;
            HandleResize();
        }
    }
    public class GlobalMouseHandler : IMessageFilter
    {
        private Form1 instance;
        private const int WM_LBUTTONDOWN = 0x201;

        public GlobalMouseHandler(Form1 instance)
        {
            this.instance = instance;
        }

        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == WM_LBUTTONDOWN)
            {
                
            }
            return false;
        }
    }
}
