using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cinema
{
    public partial class Admin : Form
    {
        public Admin()
        {
            InitializeComponent();
        }

        string connectionString = "Data Source=LAPTOP-JSVR0I7M; Initial Catalog=Cinema; Integrated Security=True"; 

        private int GetInsertedFilmID(SqlConnection conn)
        {
            SqlCommand cmd = new SqlCommand("SELECT MAX(Film_ID) FROM Film", conn);
            int filmID = Convert.ToInt32(cmd.ExecuteScalar());
            
            return filmID;
        }

        private void AddFilmGenre(SqlConnection conn, int filmID)
        {
            int genreID = GetSelectedGenreID(); 
            
            SqlCommand cmd = new SqlCommand("INSERT INTO [Genres and films] (Film, [Genre]) VALUES (@FilmID, @GenreID)", conn);
            
            cmd.Parameters.AddWithValue("@FilmID", filmID);
            cmd.Parameters.AddWithValue("@GenreID", genreID);
            cmd.ExecuteNonQuery(); 
        }

        private int GetSelectedGenreID()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                
                SqlCommand cmd = new SqlCommand("SELECT Genre_ID FROM Genre WHERE Title = @Title", conn);
                cmd.Parameters.AddWithValue("@Title", comboBox1.SelectedItem.ToString());
                int genreID = Convert.ToInt32(cmd.ExecuteScalar());
                
                return genreID;
            }
        }

        private void LoadGenres()
        {
            comboBox1.Items.Clear();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                
                SqlCommand cmd = new SqlCommand("SELECT Title FROM Genre ORDER BY Title ASC", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                
                while (reader.Read())
                {
                    comboBox1.Items.Add(reader[0].ToString());
                }
                
                reader.Close();
            }
        }

        private void LoadFilms()
        {
            comboBox2.Items.Clear();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT Title FROM Film ORDER BY Title ASC", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                
                while (reader.Read())
                {
                    comboBox2.Items.Add(reader[0].ToString());
                    comboBox5.Items.Add(reader[0].ToString());
                }
                
                reader.Close();
            }
        }

        private void Admin_Load(object sender, EventArgs e)
        {
            LoadFilms(); 
            LoadGenres();
            LoadSeances();
            LoadHalls();
        }

        private void Admin_FormClosed(object sender, FormClosedEventArgs e)
        {
            Input input = new Input();
            input.Show();
            Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            comboBox6.SelectedIndex = 0;
            int hours = int.Parse(comboBox7.Text);
            int minutes = int.Parse(comboBox8.Text);

            TimeSpan duration = new TimeSpan(hours, minutes, 0);

            string formattedDuration = duration.ToString(@"hh\:mm\:ss");

            if (textBox1.Text == "" || comboBox7.Text == "" || comboBox8.Text == "" || textBox3.Text == "" || textBox4.Text == "" || textBox5.Text == "" || comboBox6.Text == "" || textBox7.Text == "")
            {
                MessageBox.Show("Не все поля заполнены", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand("INSERT INTO Film (Title, Duration, Description, Producer, Price, [Age limit], [Rental period], [Start date]) " +
                            "VALUES (@Title, @Duration, @Description, @Producer, @Price, @AgeLimit, @RentalPeriod, @StartDate)", conn);

                        cmd.Parameters.AddWithValue("@Title", textBox1.Text);
                        cmd.Parameters.AddWithValue("@Duration", formattedDuration);
                        cmd.Parameters.AddWithValue("@Description", textBox3.Text);
                        cmd.Parameters.AddWithValue("@Producer", textBox4.Text);
                        cmd.Parameters.AddWithValue("@Price", textBox5.Text);
                        cmd.Parameters.AddWithValue("@AgeLimit", comboBox6.Text);
                        cmd.Parameters.AddWithValue("@RentalPeriod", textBox7.Text);
                        cmd.Parameters.AddWithValue("@StartDate", dateTimePicker1.Value);
                        cmd.ExecuteNonQuery();

                        int filmID = GetInsertedFilmID(conn);
                        AddFilmGenre(conn, filmID);
                        MessageBox.Show("Фильм успешно добавлен!", "Успешно!", MessageBoxButtons.OK);

                        LoadGenres();
                        LoadFilms();
                    }
                }
                catch (Exception ex)
                { MessageBox.Show("Ошибка: " + ex.Message, "Ошибка!", MessageBoxButtons.OK); }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string selectedFilm = comboBox2.SelectedItem.ToString();
            comboBox5.Items.Clear();
            DialogResult result = MessageBox.Show("Вы уверены, что хотите удалить " + selectedFilm + "?", "Подтвердите удаление", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string sql = "DELETE FROM [Genres and films] WHERE Film = (SELECT Film_ID FROM Film WHERE Title = @Title);" +
                                 "DELETE FROM Schedule WHERE Film = (SELECT Film_ID FROM Film WHERE Title = @Title);" +
                                 "DELETE FROM Film WHERE Title = @Title;";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Title", selectedFilm);
                        MessageBox.Show("Фильм успешно удален!", "Успешно!", MessageBoxButtons.OK);
                        connection.Open();
                        command.ExecuteNonQuery();
                    }

                    comboBox5.Items.Clear();
                    using (SqlCommand command = new SqlCommand("SELECT Title FROM Film", connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                comboBox5.Items.Add(reader["Title"].ToString());
                            }
                        }
                    }
                }
            }
        }
        private void LoadHalls()
        {
            comboBox3.Items.Clear();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT Title FROM Hall ORDER BY Title ASC", conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    comboBox3.Items.Add(reader[0].ToString());
                }

                reader.Close();
            }
        }
        private void LoadSeances()
        {
            comboBox4.Items.Clear();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT [start time] FROM Seance ORDER BY [start time] ASC", conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    comboBox4.Items.Add(reader[0].ToString());
                }

                reader.Close();
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox8.Text == "" || textBox9.Text == "")
            {
                MessageBox.Show("Не все поля заполнены", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            DateTime timeValue;

            if (!DateTime.TryParseExact(textBox8.Text, "HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out timeValue))
            {
                MessageBox.Show("Неверный формат времени", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO Seance ([start time], Сoefficient) " +
                        "VALUES (@Time, @Сoefficient)", conn);

                    cmd.Parameters.AddWithValue("@Time", textBox8.Text);
                    cmd.Parameters.AddWithValue("@Сoefficient", textBox9.Text);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Сеанс успешно добавлен!", "Успешно!", MessageBoxButtons.OK);

                    LoadSeances();
                }
            }
        }
        private void LoadSchedules()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT Schedule.ID, Schedule.Date, Hall.Title AS Hall, Film.Title AS Film, Seance.[start time] AS Time " +
                                                 "FROM Schedule " +
                                                 "INNER JOIN Hall ON Schedule.Hall = Hall.ID " +
                                                 "INNER JOIN Film ON Schedule.Film = Film.ID " +
                                                 "INNER JOIN Seance ON Schedule.Seance = Seance.ID", conn);
                
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            if (comboBox3.SelectedItem == null || comboBox4.SelectedItem == null || comboBox5.SelectedItem == null)
            {
                MessageBox.Show("Не все поля заполнены", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    DateTime @date = Convert.ToDateTime(dateTimePicker2.Text); 
                    SqlCommand cmd = new SqlCommand("INSERT INTO Schedule (Date, Film, Seance, Hall) " +
                        "VALUES (@Date, @Film, @Seance, @Hall)", conn);

                    SqlCommand hallCmd = new SqlCommand("SELECT Hall_ID FROM Hall WHERE Hall.Title = @TitleHoll", conn);
                    hallCmd.Parameters.AddWithValue("@TitleHoll", comboBox3.SelectedItem.ToString());
                    int hallID = (int)hallCmd.ExecuteScalar();

                    SqlCommand seanceCmd = new SqlCommand("SELECT Seance_ID FROM Seance WHERE [start time] = @StartTime", conn);
                    seanceCmd.Parameters.AddWithValue("@StartTime", comboBox4.SelectedItem.ToString());
                    int seanceID = (int)seanceCmd.ExecuteScalar();

                    SqlCommand filmCmd = new SqlCommand("SELECT DISTINCT Film_ID FROM Film WHERE Film.Title = @Title", conn);
                    filmCmd.Parameters.AddWithValue("@Title", comboBox5.SelectedItem.ToString());
                    int filmID = (int)filmCmd.ExecuteScalar();

                    cmd.Parameters.AddWithValue("@Date", @date.ToString("dd-MM-yyyy"));
                    cmd.Parameters.AddWithValue("@Hall", hallID);
                    cmd.Parameters.AddWithValue("@Seance", seanceID);
                    cmd.Parameters.AddWithValue("@Film", filmID);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Расписание успешно добавлено!", "Успешно!", MessageBoxButtons.OK);

                    LoadSchedules();
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DateTime date= DateTime.Now;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "Select Film.Title, Schedule.Date, Seance.[start time], Hall.Title " +
                               "From Schedule " +
                               "Inner Join Film on Film.Film_ID = Schedule.Film " +
                               "Inner join Seance on Seance.Seance_ID = Schedule.Seance " +
                               "Inner join Hall on Hall.Hall_ID = Schedule.Hall " +
                               "Where Schedule.Date >= @date ";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@date", date.ToString("yyyy-MM-dd"));
                connection.Open();

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                System.Data.DataTable dataTable = new System.Data.DataTable();

                adapter.Fill(dataTable);

                dataGridView1.DataSource = dataTable;

                dataTable.Columns[0].ColumnName = "фильм";
                dataTable.Columns[1].ColumnName = "дата";
                dataTable.Columns[2].ColumnName = "начало";
                dataTable.Columns[3].ColumnName = "зал";

                dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.PowderBlue;
                dataGridView1.EnableHeadersVisualStyles = false;
            }
        }
    }
}
