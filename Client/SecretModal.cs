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
    public partial class SecretModal : Form
    {
       public  static string Key { get; set; }
        public SecretModal(ref string key)
        {
            Key = key;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Key = textBox1.Text;
            this.Close();
        }
    }
}
