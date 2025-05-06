using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cinema
{
    public partial class Hall1 : Form
    {

        public static List<(int, string, TimeSpan, string, string, string, TimeSpan, DateTime, string, int)> info_film = new List<(int, string, TimeSpan, string, string, string, TimeSpan, DateTime, string, int)>();
        
        public static Button[][] cinemaHall1;
        
        private int _selectedRow;
        private int _selectedSeat;
        List<(int, int)> selectedSeats = new List<(int, int)>();

        public Hall1()
        {
            InitializeComponent();
            this.BackColor = Color.FromArgb(20, 4, 30);
            
            Button[][] cinemaHall1Buttons =
            {
                new[] { b11, b12, b13, b14, b15, b16, b17, b18, b19, b110 },
                new[] { b21, b22, b23, b24, b25, b26, b27, b28, b29, b210 },
                new[] { b31, b32, b33, b34, b35, b36, b37, b38 },
                new[] { b41, b42, b43, b44, b45, b46, b47, b48 },
                new[] { b51, b52, b53, b54, b55, b56, b57, b58 },
                new[] { b61, b62, b63, b64, b65, b66, b67, b68 },
                new[] { b71, b72, b73, b74, b75, b76, b77, b78 },
                new[] { b81, b82, b83, b84, b85, b86, b87, b88, b89, b810, b811, b812 }
            };

            for (int i = 0; i < cinemaHall1Buttons.Length; i++)
            {
                for (int j = 0; j < cinemaHall1Buttons[i].Length; j++)
                {
                    cinemaHall1Buttons[i][j].Tag = new Seat(i + 1, j + 1);
                    cinemaHall1Buttons[i][j].Click += new EventHandler(SeatButton_Click);
                }
            }
            cinemaHall1 = cinemaHall1Buttons;
        }

        private void Hall1_Load(object sender, EventArgs e)
        {
            info_film = Day_Seances.getInfoOneFilm(Panell.film_title, Panell.film_starttime, Panell.date_film, Panell.film_hall);
            var (filmID, title, duration, genre, ageRestriction, description, startTime, date, hall, price) = info_film[0];
            label15.Text = title;
            label16.Text = description;
            label10.Text = ageRestriction;
            label11.Text = genre;
            label12.Text = duration.ToString();
            label13.Text = price.ToString() + " рублей";

            label15.ForeColor = Color.White;
            label16.ForeColor = Color.White;
            label10.ForeColor = Color.White;
            label11.ForeColor = Color.White;
            label12.ForeColor = Color.White;
            label13.ForeColor = Color.White;

            string filmTitle = title;
            DateTime dateSeance = date;
            TimeSpan timeSeance = startTime;
            string hallName = hall;
            Seat.OccupiedPlaces(filmTitle, dateSeance, timeSeance, hallName, cinemaHall1);
        }

        int count = 0;
        private void SeatButton_Click(object sender, EventArgs e)
        {
            info_film = Day_Seances.getInfoOneFilm(Panell.film_title, Panell.film_starttime, Panell.date_film, Panell.film_hall);
            var (filmID, title, duration, genre, ageRestriction, description, startTime, date, hall, price) = info_film[0];
            Button button = (Button)sender;
            Seat seat = (Seat)button.Tag;
            _selectedRow = seat.RowNumber;
            _selectedSeat = seat.SeatNumber;
            
            button.BackColor = Color.Yellow;

            DialogResult result = MessageBox.Show($"Вы выбрали {_selectedRow} ряд {_selectedSeat} место. Стоимость билета составит: {label13.Text}.", "Выбранное место", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                count++;
                
                button.BackColor = Color.Yellow;
                button.Enabled = false;
                selectedSeats.Add((seat.RowNumber, seat.SeatNumber));
                
                if (count == 1)
                {
                    label17.Text = $"1 билет за {count * price} рублей";
                }
                else
                {
                    label17.Text = $"{count} билетa за {count * price} рублей";
                }
            }
            else
            {
                button.BackColor = Color.FromArgb(192, 192, 255); ;
            }
        }

        Day_Seances day_Seances = new Day_Seances();  
    
        private void buy_Click(object sender, EventArgs e)
        {
            if (Input.flagAnonim == true)
            {
                MessageBox.Show("чтобы продолжить, необходимо зарегистрироваться!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                button1.Visible = true;
                buy.Visible = false;
                label17.Visible = false;
            }
            else
            {
                if (selectedSeats.Count == 0)
                {
                    MessageBox.Show("необходимо выбрать место!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    var (filmID, title, duration, genre, ageRestriction, description, startTime, date, hall, price) = info_film[0];
                    string filmTitle = title;
                    DateTime dateSeance = date;
                    TimeSpan timeSeance = startTime;
                    string hallName = hall;
                    Seat.OccupiedPlaces(filmTitle, dateSeance, timeSeance, hallName, cinemaHall1);

                    Buy buy = new Buy(selectedSeats);

                    buy.Show();
                }
            }
        }
       
        private void Hall1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Hide();

            Schedule schedule = new Schedule();
            schedule.Show();

            day_Seances.getNewSeances(Panell.date_film);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Input input = new Input();
            input.Show();
            Hide();
        }


        ToolTip toolTip1 = new ToolTip();
    }
}
