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
        public Form2(Form1 instance)
        {
            this.instance = instance;
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Value = instance.settings.GetDate("startDate", DateTime.Today);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            instance.settings.Set("startDate", dateTimePicker1.Value);
            instance.settings.Save();
            this.Dispose();
        }
    }
}
