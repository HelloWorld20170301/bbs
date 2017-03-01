using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class formMain : Form
    {
        public formMain()
        {
            InitializeComponent();
        }
        System.Timers.Timer time = new System.Timers.Timer(10);
        private void button1_Click(object sender, EventArgs e)
        {
            //this.Size = new Size(300, 600);
            //Button button = new Button();
            //button.Location = new Point(30, 100);
            //button.Text = "新增的button";
            //button.Click += newButtonClick;
            //this.Controls.Add(button);


        }

        private void formMain_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
        }
    }
}
