
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;

namespace Cinema
{
    public partial class Buy : Form
    {
        public Buy()
        {
            InitializeComponent();
        }
        private int row;
        private int seat;
        private double price;
        private int _selectedRow;
        private int _selectedSeat;

        private int hallID;
        private int rowNumber;
        private int seatNumber;
        private int scheduleID;
        private int placeID;

        public static int index;
        public Buy(int selectedRow, int selectedSeat, double seatPrice)
        {
            InitializeComponent();

            row = selectedRow;
            seat = selectedSeat;
            price = seatPrice;
        }

        List<(int, int)> selectedSeats;

        public Buy(List<(int, int)> selectedSeats)
        {
            InitializeComponent();

            this.selectedSeats = selectedSeats;

            label1.Text = string.Join("\n", selectedSeats.Select(seat =>
            {
                rowNumber = seat.Item1; 
                seatNumber = seat.Item2; 
                return $"{rowNumber} ряд, {seatNumber} место"; 
            }));
        } 
        
        public static List<(int, string, TimeSpan, string, string, string, TimeSpan, DateTime, string, int)> info_film = new List<(int, string, TimeSpan, string, string, string, TimeSpan, DateTime, string, int)>();
        private void Buy_Load(object sender, EventArgs e)
        {
            info_film = Day_Seances.getInfoOneFilm(Panell.film_title, Panell.film_starttime, Panell.date_film, Panell.film_hall);
            var (filmID, title, duration, genre, ageRestriction, description, startTime, date, hall, price) = info_film[0];
            label4.Text = $"билет на фильм {title} на {date.ToString("dd.MM")} число";
            label5.Text = $"начало в {startTime}";
            label6.Text = $"к оплате {price * selectedSeats.Count} рублей";

        }
        private void button1_Click(object sender, EventArgs e)
        {
            var (filmID, title, duration, genre, ageRestriction, description, startTime, date, hall, price) = info_film[0];
            string filmTitle = title;
            DateTime dateSeance = date;
            TimeSpan timeSeance = startTime;
            string hallName = hall;
            string paymentType = comboBox1.Text;

            comboBox1.SelectedIndex = 0;

            index = 0;
            for (int i = 0; i < selectedSeats.Count; i++)
            {
                Email.SendToEmail(selectedSeats, Input.emaiAddress);
                //MessageBox.Show("Сообщение отправлено успешно!");
                index++;
            }
            
            string connectionString = "Data Source=LAPTOP-JSVR0I7M; Initial Catalog=Cinema; Integrated Security=True";
            foreach ((int rowNumber, int seatNumber) in selectedSeats)
            {

                int hallID;
                string hallTitle = hall;
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var command = new SqlCommand("SELECT Hall_ID FROM Hall WHERE Title = @hallTitle", connection);
                    command.Parameters.AddWithValue("@hallTitle", hallTitle);
                    hallID = (int)command.ExecuteScalar();
                }

                int seanceID;
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var command = new SqlCommand("SELECT Seance_ID FROM Seance WHERE[start time] = @startTime", connection);
                    command.Parameters.AddWithValue("@startTime", timeSeance);
                    seanceID = (int)command.ExecuteScalar();
                }

                int scheduleID;
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var command = new SqlCommand("SELECT Schedule_ID FROM Schedule WHERE [Date] = @date AND Film = @filmID AND Seance = @seanceID AND Hall = @hallID", connection);
                    command.Parameters.AddWithValue("@date", dateSeance);
                    command.Parameters.AddWithValue("@filmID", filmID);
                    command.Parameters.AddWithValue("@seanceID", seanceID);
                    command.Parameters.AddWithValue("@hallID", hallID);
                    scheduleID = (int)command.ExecuteScalar();
                }

                int placeID;
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var command = new SqlCommand("SELECT Place_ID FROM Places WHERE Hall = @hallID AND [Row number] = @rowNumber AND [Number of seats] = @seatNumber", connection);
                    command.Parameters.AddWithValue("@hallID", hallID);
                    command.Parameters.AddWithValue("@rowNumber", rowNumber);
                    command.Parameters.AddWithValue("@seatNumber", seatNumber);
                    placeID = (int)command.ExecuteScalar();
                }

                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var command = new SqlCommand("INSERT INTO Buy (Schedule, Place, [Type of payment]) VALUES (@scheduleID, @placeID, @paymentType)", connection);
                    command.Parameters.AddWithValue("@scheduleID", scheduleID);
                    command.Parameters.AddWithValue("@placeID", placeID);
                    command.Parameters.AddWithValue("@paymentType", paymentType);
                    command.ExecuteNonQuery();
                }
                Seat.OccupiedPlaces(filmTitle, dateSeance, timeSeance, hallName, Hall1.cinemaHall1);
                Button button = (Button)sender;
            }
            MessageBox.Show("Сообщение отправлено успешно!");
            Close();

        }
    }
}
