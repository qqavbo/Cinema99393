using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cinema
{
    internal class Day_Seances
    {
        public List<Tuple<int, string, TimeSpan, int, string, DateTime, string>> films = new List<Tuple<int, string, TimeSpan, int, string, DateTime, string>>();
        public static List<(int, string, TimeSpan, string, string, string, TimeSpan, DateTime, string, int)> film_info = new List<(int, string, TimeSpan, string, string, string, TimeSpan, DateTime, string, int)>();

        public void getNewSeances(DateTime day)
        {
            films.Clear();

            Program.schedule.flowLayoutPanel1.Controls.Clear();

            string connectionString = "Data Source=LAPTOP-JSVR0I7M; Initial Catalog=Cinema; Integrated Security=True";

            string query = "SELECT DISTINCT Film.Film_ID, Film.Title, Film.Duration, Genre.Genre_ID, Film.[Age limit], Schedule.Date, Film.Description FROM Schedule " +
                           "JOIN Film ON Schedule.Film = Film.Film_ID " +
                           "JOIN [Genres and films] ON Film.Film_ID = [Genres and films].Film " +
                           "JOIN Genre ON [Genres and films].Genre = Genre.Genre_ID " +
                           "WHERE Schedule.Date = @day ";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                command.Parameters.AddWithValue("@day", day.ToString("yyyy-MM-dd"));

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int filmID = reader.GetInt32(0);
                    string title = reader.GetString(1);
                    var duration = reader.GetTimeSpan(2);
                    int genre = reader.GetInt32(3);
                    string ageRestriction = reader.GetString(4);
                    DateTime Date = reader.GetDateTime(5);
                    string description = reader.GetString(6);

                    films.Add(new Tuple<int, string, TimeSpan, int, string, DateTime, string>(filmID, title, duration, genre, ageRestriction, Date, description));
                }
                reader.Close();

                for (int i = 0; i < films.Count; i++)
                {
                    int filmID = films[i].Item1;
                    string title = films[i].Item2;
                    TimeSpan duration = films[i].Item3;
                    int genre = films[i].Item4;
                    string ageRestriction = films[i].Item5;
                    DateTime Date = films[i].Item6;

                    var filmPanel = new Panell(Program.schedule.flowLayoutPanel1, filmID, title, duration, genre, ageRestriction, Date);
                    Program.schedule.flowLayoutPanel1.Controls.Add(filmPanel.GetPanel());

                }
            }
        }

        public static List<(int, string, TimeSpan, string, string, string, TimeSpan, DateTime, string, int)> getInfoOneFilm(string Film, TimeSpan time, DateTime date, string hall)
        {
            film_info.Clear();

            string connectionString = "Data Source=LAPTOP-JSVR0I7M; Initial Catalog=Cinema; Integrated Security=True";
            //  Название, продолжительность, жанр, ограничение возрастное
            string query = "SELECT DISTINCT Film.Film_ID, Film.Title, Film.Duration, Genre.Title, Film.[Age limit], Film.Description, Seance.[start time], Schedule.Date, Hall.Title, Film.Price* Seance.Сoefficient AS Price " +
                           "FROM Schedule " +
                           "join Hall on Schedule.Hall = Hall.Hall_ID " +
                           "JOIN Film ON Schedule.Film = Film.Film_ID " +
                           "JOIN[Genres and films] ON Film.Film_ID = [Genres and films].Film " +
                           "JOIN Genre ON[Genres and films].Genre = Genre.Genre_ID " +
                           "JOIN Seance ON Schedule.Seance = Seance.Seance_ID " +
                           "WHERE Film.Title = @film and Seance.[start time] = @time and Schedule.Date = @date and Hall.Title = @hall";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                command.Parameters.AddWithValue("@Film", Film);
                command.Parameters.AddWithValue("@time", time);
                command.Parameters.AddWithValue("@date", date.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@hall", hall);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int filmID = reader.GetInt32(0);
                    string title = reader.GetString(1);
                    var duration = reader.GetTimeSpan(2);
                    string genre = reader.GetString(3);
                    string ageRestriction = reader.GetString(4);
                    string description = reader.GetString(5);
                    TimeSpan startTime = reader.GetTimeSpan(6);
                    DateTime data = reader.GetDateTime(7);
                    string _hall = reader.GetString(8);
                    int price = reader.GetInt32(9);

                    film_info.Add((filmID, title, duration, genre, ageRestriction, description, startTime, data, _hall, price));
                }
                reader.Close();

                for (int i = 0; i < film_info.Count; i++)
                {
                    int filmID = film_info[i].Item1;
                    string title = film_info[i].Item2;
                    var duration = film_info[i].Item3;
                    string genre = film_info[i].Item4;
                    string ageRestriction = film_info[i].Item5;
                    string description = film_info[i].Item6;
                    TimeSpan startTime = film_info[i].Item7;
                    DateTime data = film_info[i].Item8;
                    string _hall = film_info[i].Item9;
                    int price = film_info[i].Item10;
                }
            }
            return film_info;
        }

    }
}