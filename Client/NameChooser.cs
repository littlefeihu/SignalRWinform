using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class NameChooser : Form
    {
        public string ChoosenName { get; set; }
        public NameChooser(ref string name)
        {
            this.ChoosenName = name;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Name could not be null!","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                textBox1.Focus();
            }
            else
            {
                this.ChoosenName = textBox1.Text;
                this.Close();
            }
        }
    }
}
