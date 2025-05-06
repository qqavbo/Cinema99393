using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cinema
{
    internal class Panell
    {
        private int filmID;
        private string title;
        private TimeSpan duration;
        private int genre;
        private DateTime Date;
        private string ageRestriction;
        private List<Tuple<string, DateTime, TimeSpan, string>>  showings;
        public static DateTime date_film;
        public static string film_title;
        public static TimeSpan film_starttime;
        public static string film_hall;

        private readonly Panel codePanel;
        public Panell(int filmID, string title, TimeSpan duration, int genre, string ageRestriction, DateTime Date)
        {
            this.filmID = filmID;
            this.title = title;
            this.duration = duration;
            this.genre = genre;
            this.ageRestriction = ageRestriction;
            this.Date = Date;
        }

        public Panell(FlowLayoutPanel parent, int filmID, string title, TimeSpan duration, int genre, string ageRestriction, DateTime Date, bool isDisabled = false)
        {

            codePanel = new Panel();
            codePanel.Margin = new Padding(0, 27, 0, 0);
            codePanel.Size = new Size(400, 40);

            parent.Margin = new Padding(0, 20, 0, 10);
            Panel filmPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 85,
                Width = 300,
                
            };

            Label titleLabel = new Label
            {
                Text = title,
                Font = new Font("Microsoft Sans Serif", 14, FontStyle.Underline),
                Dock = DockStyle.Top,
                ForeColor = Color.White,
            };

            Label durationLabel = new Label
            {
                Font = new Font("Microsoft Sans Serif", 10),
                Dock = DockStyle.Top,
                ForeColor = Color.White,
            };

            if (duration.Hours == 1)
            {
                durationLabel.Text = $"{duration.Hours} час {duration.Minutes} минут";
            }
            else
            {
                durationLabel.Text = $"{duration.Hours} часа {duration.Minutes} минут";
            }

            Label genreLabel = new Label
            {
                Text = GetGenreName(genre),
                Font = new Font("Microsoft Sans Serif", 10),
                Dock = DockStyle.Top,
                ForeColor = Color.White,
            };

            Label ageRestrictionLabel = new Label
            {
                Text = ageRestriction,
                Font = new Font("Microsoft Sans Serif", 10),
                Dock = DockStyle.Top,
                ForeColor = Color.White,
            };

            var showings = GetShowingsForFilmAndDate(filmID, Date);
            for (int i = 0; i < showings.Count; i++)
            {
                var button = new Button
                {
                    Text = showings[i].Item3.ToString(@"hh\:mm"),
                    Dock = DockStyle.Left,
                    Margin = new Padding(15, 0, 15, 0),
                    Width = 50,
                    FlatStyle = FlatStyle.Popup,
                    BackColor = Color.White,
                    Tag = showings[i]
                };

                Tuple<string, DateTime, TimeSpan, string> showing = (Tuple<string, DateTime, TimeSpan, string>)button.Tag;
                TimeSpan startTime = showing.Item3;

                if (startTime < DateTime.Now.TimeOfDay)
                {
                    button.Enabled = false;
                }

                button.Click += new EventHandler(button_Click);
                codePanel.Controls.Add(button);
            }

            filmPanel.Controls.Add(ageRestrictionLabel);
            filmPanel.Controls.Add(genreLabel);
            filmPanel.Controls.Add(durationLabel);
            filmPanel.Controls.Add(titleLabel);
            filmPanel.Controls.Add(codePanel);

            parent.Controls.Add(filmPanel);

        }

        private void button_Click(object sender, EventArgs e)
        {
            var button = (Button)sender; 
            var tuple = (Tuple<string, DateTime, TimeSpan, string>)button.Tag;

            var hallTitle = tuple.Item4;

            film_starttime = tuple.Item3;
            film_title = tuple.Item1;
            film_hall = tuple.Item4;
            Program.schedule.Hide();

            date_film = tuple.Item2;

            if (hallTitle == "Зал 1")
            {
                Hall1 hall1 = new Hall1();
                hall1.Show();
            }
            else if (hallTitle == "Зал 2")
            {
                Hall2 hall2 = new Hall2();
                hall2.Show();
            }
            else if (hallTitle == "Зал 3")
            {
                Hall3 hall3 = new Hall3();
                hall3.Show();
            }
            else if (hallTitle == "Зал 4")
            {
                Hall4 hall4 = new Hall4();
                hall4.Show();
            }
            else
            {
                HallVIP hallVIP = new HallVIP();
                hallVIP.Show();
            }
        }
        /*public byte[] GetFilmImageData(int filmID)
        {
            byte[] imageData = null;
            string connectionString = "Data Source=LAPTOP-JSVR0I7M; Initial Catalog=Cinema; Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT ImageData FROM Film WHERE Film_ID = @filmID";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@filmID", filmID);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        imageData = (byte[])reader["ImageData"];
                    }
                }
            }
            return imageData;
        }*/
        public static List<Tuple<string, DateTime, TimeSpan, string>> GetShowingsForFilmAndDate(int filmID, DateTime date)
        {
            var _showings = new List<Tuple<string, DateTime, TimeSpan, string>>();

            string connectionString = "Data Source=LAPTOP-JSVR0I7M; Initial Catalog=Cinema; Integrated Security=True";
            string query =   "SELECT DISTINCT f.Title, sch.Date, s.[start time], Hall.Title As Time, f.Description " +
                             "FROM Schedule sch " +
                             "JOIN Seance s ON sch.Seance = s.Seance_ID " +
                             "JOIN Film f ON sch.Film = f.Film_ID " +
                             "Inner join Hall on Hall.Hall_ID = sch.Hall " +
                             "WHERE f.Film_ID = @filmID AND sch.[Date] = @date " +
                             "ORDER BY s.[start time] ";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@filmID", filmID);
                command.Parameters.AddWithValue("@date", date);

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var filmTitle = reader.GetString(0);
                    var showDate = reader.GetDateTime(1);
                    var startTime = reader.GetTimeSpan(2);
                    var hallTitle = reader.GetString(3);
                    var duration = TimeSpan.FromMinutes(120);

                    _showings.Add(Tuple.Create(filmTitle, showDate, startTime, hallTitle));
                }
                reader.Close();
            }
            return _showings;
        }
        public Panell()
        {
        }

        public Panel GetPanel()
        {
            return codePanel;
        }

        private static string GetGenreName(int genreID)
        {

            string connectionString = "Data Source=LAPTOP-JSVR0I7M; Initial Catalog=Cinema; Integrated Security = True";
            string query = "SELECT Title FROM Genre WHERE Genre_ID = @genreID";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@genreID", genreID);

                connection.Open();

                string genreName = (string)command.ExecuteScalar();

                return genreName;
            }
        }
    }
}
