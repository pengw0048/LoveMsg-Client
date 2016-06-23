using System;
using System.Windows.Forms;

namespace LoveMsg
{
    public partial class Form3 : Form
    {
        private Form1 instance;
        public Form3(Form1 instance)
        {
            InitializeComponent();
            this.instance = instance;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var group = textBox1.Text.Trim();
            var member = textBox2.Text.Trim();
            if (group == "" || member == "")
            {
                MessageBox.Show(instance,"不能为空");
                return;
            }
            string ret = "";
            try
            {
                ret = Http.HttpGet(Http.server + "action=login&group=" + group + "&member=" + member);
            }catch(Exception ex) { MessageBox.Show(ex.ToString()); return; }
            if (ret.Length <= 2)
            {
                MessageBox.Show(instance,"未知错误");
                return;
            }
            if (ret.Substring(0, 2) != "1:")
            {
                MessageBox.Show(ret);
                return;
            }
            MessageBox.Show(instance,ret.Substring(2));
            instance.settings.Set("group", group, false);
            instance.settings.Set("member", member, false);
            instance.settings.Save();
            this.Dispose();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            textBox1.Text = instance.settings.Get("group","");
            textBox2.Text = instance.settings.Get("member","");
        }
    }
}
