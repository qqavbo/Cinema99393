using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cinema
{
    public partial class Ticket : Form
    {
        public Ticket()
        {
            InitializeComponent();
            Program.ticket = this;
        }


        void panel1_Paint(object sender, PaintEventArgs e)
        {
        }
        private void Ticket_Load(object sender, EventArgs e)
        {
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
