using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LoveMsg
{
    public partial class Form2 : Form
    {
        private Form1 instance;
        private Font font;
        public Form2(Form1 instance)
        {
            this.instance = instance;
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Value = instance.settings.GetDate("startDate", DateTime.Today);
            trackBar1.Value = instance.settings.GetInt("animeSpeed", 200) / 20;
            font = instance.label1.Font;
            UpdateFontDisplay();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var fontstr = new FontConverter().ConvertToInvariantString(font);
            instance.settings.Set("startDate", dateTimePicker1.Value,false);
            instance.settings.Set("font", fontstr);
            instance.settings.Set("animeSpeed", trackBar1.Value * 20);
            instance.settings.Save();
            try
            {
                Http.HttpGet(Http.server + "action=setdate&group=" + instance.settings.Get("group", "") + "&value=" + Uri.EscapeDataString(instance.settings.Get("startDate", "")));
            }catch(Exception ex)
            {
                MessageBox.Show(instance,"网络错误：" + ex.Message);
            }
            this.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            fontDialog1.Font = font;
            fontDialog1.ShowDialog();
            font = fontDialog1.Font;
            UpdateFontDisplay();
        }

        void UpdateFontDisplay()
        {
            label2.Text = "字体：" + new FontConverter().ConvertToInvariantString(font).Replace("style=", "");
        }
    }
}
