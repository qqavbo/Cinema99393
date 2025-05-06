using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace Cinema
{
    public partial class Schedule : Form
    {
        public Schedule()
        {
            InitializeComponent();
            Program.schedule = this;
            this.BackColor = Color.FromArgb(20, 4, 30);
            panel1.BackColor = Color.FromArgb(96, 64, 130);
            panel2.BackColor = Color.FromArgb(96, 64, 130);
        }
        TableLayoutPanel tableLayoutPanel = new TableLayoutPanel();
        private Day_Seances daySeances = new Day_Seances();
        private void Schedule_FormClosed(object sender, FormClosedEventArgs e)
        {
            Input input = new Input();
            input.Show();
            Hide();
        }

        private void Schedule_Load(object sender, EventArgs e)
        {
            List<DateTime> list = new List<DateTime>();
            for (int i = 0; i < 5; i++)
            {
                list.Add(DateTime.Now.AddDays(i));
            }

            tableLayoutPanel.RowCount = 1;
            tableLayoutPanel.ColumnCount = 5;
            tableLayoutPanel.Dock = DockStyle.Fill;

            DateTime today = DateTime.Now.Date;
            DateTime tomorrow = today.AddDays(1);
            string[] daysOfWeek = new string[] { "ВОСКРЕСЕНЬЕ", "ПОНЕДЕЛЬНИК", "ВТОРНИК", "СРЕДА", "ЧЕТВЕРГ", "ПЯТНИЦА", "СУББОТА" };

            for (int i = 0; i < 5; i++)
            {
                FlowLayoutPanel flPanel = new FlowLayoutPanel();
                flPanel.Size = new System.Drawing.Size(150, 75);

                Panel panel = new Panel();
                panel.BackColor = Color.Transparent;
                panel.Size = new Size(150, 75);
                flPanel.Controls.Add(panel);
                flPanel.BackColor = Color.Transparent;
               
                Button button = new Button();
                button.Name = $"button {i}";
                button.Click += new EventHandler(button_Click); 

                DateTime date = list[i];
                string buttonText = string.Empty;
                if (date.Date == today)
                {
                    buttonText = "СЕГОДНЯ\n " + date.ToString("dd MMMM", CultureInfo.GetCultureInfo("ru-RU"));
                }
                else if (date.Date == tomorrow)
                {
                    buttonText = "ЗАВТРА\n " + date.ToString("dd MMMM", CultureInfo.GetCultureInfo("ru-RU"));
                }
                else
                {
                    buttonText = daysOfWeek[(int)date.DayOfWeek] + "  " + "\n" + date.ToString("dd MMMM", CultureInfo.GetCultureInfo("ru-RU"));
                }

                button.Text = buttonText;
                button.Tag = list[i]; 
                button.Size = new Size(80, 40);
                button.Location = new Point((panel.Width - button.Width) / 2, (panel.Height - button.Height) / 2);
                button.FlatStyle = FlatStyle.Popup;
                button.BackColor = Color.White;
                panel.Controls.Add(button);

                tableLayoutPanel.BackColor = System.Drawing.Color.Transparent;
                
                tableLayoutPanel.Controls.Add(flPanel, i, 0);
                tableLayoutPanel.Location = new Point(150, 80);
            }
            this.Controls.Add(tableLayoutPanel);

            Day_Seances day_Seances = new Day_Seances();
            day_Seances.getNewSeances(list[0]);

        }
        private void button_Click(object sender, EventArgs e)
        {
            Program.schedule.flowLayoutPanel1.Controls.Clear();
            Button button = sender as Button;
            DateTime date = (DateTime)button.Tag;
            daySeances.getNewSeances(date);

        }
    }
}
