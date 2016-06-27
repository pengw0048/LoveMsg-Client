using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace LoveMsg
{
    public partial class Form1 : Form
    {
        public Settings settings;
        private bool showHeart = false;
        private bool showForm = false;
        private bool showMsg = false;
        private bool doAnime = true;
        private bool stayTop = true;
        private DateTime startDate;
        private Animation animation;
        private ToolTip toolTip2 = null;
        private Messages msg = null;
        public Form1()
        {
            InitializeComponent();
            pictureBox1.BackColor = Color.Transparent;
            label1.Parent = pictureBox1;
            label1.BackColor = Color.Transparent;
            pictureBox2.Parent = pictureBox1;
            pictureBox2.BackColor = Color.Transparent;
            pictureBox2.BringToFront();
            pictureBox3.Parent = pictureBox1;
            pictureBox3.BackColor = Color.Transparent;
            pictureBox3.BringToFront();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            settings = new Settings("settings.ini");
            if (settings.GetBool("autopop", true)) 自动弹出ToolStripMenuItem.Checked = true;
            else 显示图标ToolStripMenuItem.Checked = true;
            if (settings.Get("group", "") == "" || settings.Get("member", "") == "")
                toolStripMenuItem4_Click(null, null);
            animation = new Animation();
            timer3.Interval = settings.GetInt("animeSpeed", 200);
            timer3.Enabled = true;
            label1.Text = DaysBetween().ToString();
            HandleFontChange();
            HandleResize();
            this.Left = settings.GetInt("X", (Screen.PrimaryScreen.Bounds.Width - 150));
            this.Top = settings.GetInt("Y", 10);
            timer2_Tick(null, null);
            msg = new Messages(this);
            new Thread(new ThreadStart(GetStartDate)).Start();
            new Thread(new ThreadStart(CleanThread)).Start();
            SetDateToolTip();
            timer5.Enabled = true;
        }
        private void HandleResize()
        {
            int left= label1.Width + label1.Left;
            pictureBox1.Height = label1.Height + 2 * label1.Top;
            if (showMsg)
            {
                label2.Top=pictureBox3.Height = pictureBox3.Width= pictureBox1.Height - 10;
                label2.Left=pictureBox3.Left = left;
                left += pictureBox3.Width + 3;
                pictureBox3.Visible = true;
            }else
            {
                pictureBox3.Visible = false;
            }
            if (showHeart)
            {
                pictureBox2.Height = pictureBox1.Height - 6;
                pictureBox2.Width = pictureBox2.Height;
                pictureBox2.Left = left;
                pictureBox2.Visible = true;
                pictureBox1.Width = left + pictureBox2.Width + 4;
            }
            else
            {
                pictureBox1.Width = left + 4;
                pictureBox2.Visible = false;
            }
            this.Size = pictureBox1.Size;
        }
        private void HandleFontChange()
        {
            var fontstr = settings.Get("font", "");
            Font font = null;
            try
            {
                font = (Font)new FontConverter().ConvertFromInvariantString(fontstr);
            }
            catch (Exception) { }
            if (font == null)
            {
                var fontFamily = FontFamily.GenericMonospace;
                var fontSize = 12;
                font = new Font(fontFamily, fontSize, FontStyle.Bold);
            }
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
            stayTop = false;
            new Form2(this).ShowDialog();
            stayTop = true;
            timer3.Interval = settings.GetInt("animeSpeed", 200);
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

        private void timer3_Tick(object sender, EventArgs e)
        {
            if (!doAnime) return;
            label1.ForeColor = animation.Next();
        }

        private void toolStripMenuItem3_CheckStateChanged(object sender, EventArgs e)
        {
            doAnime = toolStripMenuItem3.Checked;
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            stayTop = false;
            new Form3(this).ShowDialog();
            stayTop = true;
            new Thread(new ThreadStart(GetStartDate)).Start();
            SetDateToolTip();
        }

        void GetStartDate()
        {
            try
            {
                var ret = Http.HttpGet(Http.server + "action=getdate&group=" + settings.Get("group", ""));
                if (ret.StartsWith("1:"))
                {
                    settings.Set("startDate", ret.Substring(2));
                    label1.Text = DaysBetween().ToString();
                    HandleFontChange();
                    HandleResize();
                }
            }
            catch (Exception) { }
        }

        void SetDateToolTip()
        {
            toolTip1.SetToolTip(label1, settings.Get("member", "") + "@" + settings.Get("group", ""));
        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            if(stayTop)
                this.TopMost = true;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (!msg.HasMore()) return;
            var newmsg = msg.Get();
            if (toolTip2 != null) toolTip2.Dispose();
            toolTip2 = new ToolTip();
            toolTip2.IsBalloon = true;
            toolTip2.Show(newmsg.name+":\r\n"+newmsg.content, label1, timer5.Interval);
        }

        private void timer5_Tick(object sender, EventArgs e)
        {
            if(msg.HasMore() && settings.GetBool("autopop", true))
            {
                var newmsg = msg.Get();
                if (toolTip2 != null) toolTip2.Dispose();
                toolTip2 = new ToolTip();
                toolTip2.IsBalloon = true;
                toolTip2.Show(newmsg.name + ":\r\n" + newmsg.content, label1, timer5.Interval);
            }
            if(showMsg && !msg.HasMore())
            {
                showMsg = false;
                HandleResize();
            }else if(!showMsg && msg.HasMore())
            {
                showMsg = true;
                HandleResize();
            }
        }

        private void 自动弹出ToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            显示图标ToolStripMenuItem.Checked = !自动弹出ToolStripMenuItem.Checked;
            settings.Set("autopop", 自动弹出ToolStripMenuItem.Checked);
        }

        private void 显示图标ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            自动弹出ToolStripMenuItem.Checked = !显示图标ToolStripMenuItem.Checked;
            settings.Set("autopop", 自动弹出ToolStripMenuItem.Checked);
        }

        void CleanThread()
        {
            while (true)
            {
                try
                {
                    Http.HttpGet(Http.server + "action=cleanup&group=" + settings.Get("group", "") + "&member=" + settings.Get("member", ""));
                }
                catch (Exception) { }
                try
                {
                    Thread.Sleep(60000);
                }
                catch (Exception) { }
            }
        }
    }
}
