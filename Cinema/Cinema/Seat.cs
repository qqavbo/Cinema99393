using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cinema
{
    internal class Seat
    {
        public int RowNumber { get; set; }
        public int SeatNumber { get; set; }

        public Seat(int rowNumber, int seatNumber)
        {
            RowNumber = rowNumber;
            SeatNumber = seatNumber;
        }

        public static void OccupiedPlaces(string filmName, DateTime dateTime, TimeSpan timeSpan, string hallName, Button[][] hallBuyPlaces)
        {
            string connectionString = "Data Source=LAPTOP-JSVR0I7M; Initial Catalog=Cinema; Integrated Security=True";
            int RowNumber = 0, NumberSeats = 0;
            string query = "SELECT Places.[Row number], Places.[Number of seats] " +
                           "FROM Film " +
                           "JOIN Schedule ON Schedule.Film = Film.Film_ID " +
                           "JOIN Hall ON Hall.Hall_ID = Schedule.Hall " +
                           "JOIN Places ON Places.Hall = Hall.Hall_ID " +
                           "Inner join Seance on Seance.Seance_ID = Schedule.Seance " +
                           "RIGHT JOIN Buy ON Buy.Place = Places.Place_ID AND Buy.Schedule = Schedule.Schedule_ID " +
                           "WHERE Film.Title = @filmName " +
                           "AND Schedule.[Date] = @dateTime " +
                           "AND Seance.[start time] = @timeSpan " +
                           "AND Hall.Title = @hallName ";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                command.Parameters.AddWithValue("@filmName", filmName.ToString());
                command.Parameters.AddWithValue("@dateTime", dateTime.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@timeSpan", timeSpan.ToString());
                command.Parameters.AddWithValue("@hallName", hallName.ToString());

                SqlDataReader reader = command.ExecuteReader();

                List<Seat> occupiedSeats = new List<Seat>();
                while (reader.Read())
                {
                    int rowNumber = reader.GetInt32(0) - 1;
                    int seatNumber = reader.GetInt32(1) - 1;
                    Seat occupiedSeat = new Seat(rowNumber, seatNumber);
                    occupiedSeats.Add(occupiedSeat);
                }
                reader.Close();

                if (hallBuyPlaces != null)
                {
                    for (int i = 0; i < hallBuyPlaces.Length; i++)
                    {
                        for (int j = 0; j < hallBuyPlaces[i].Length; j++)
                        {
                            Seat maybeOccupiedseat = new Seat(i, j);
                            if (occupiedSeats.Any(s => s.RowNumber == maybeOccupiedseat.RowNumber && s.SeatNumber == maybeOccupiedseat.SeatNumber))
                            {
                                hallBuyPlaces[i][j].BackColor = Color.Red;
                                hallBuyPlaces[i][j].Enabled = false;
                            }
                        }
                    }
                }
            }
        }
    }
}
