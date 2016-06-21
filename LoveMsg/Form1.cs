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

        }
    }
}
