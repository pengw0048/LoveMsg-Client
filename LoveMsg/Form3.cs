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
            this.Dispose();
        }
    }
}
