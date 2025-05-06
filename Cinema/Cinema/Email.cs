using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace Cinema
{
    internal class Email
    {
        public static List<(int, string, TimeSpan, string, string, string, TimeSpan, DateTime, string, int)> info_film_Word = new List<(int, string, TimeSpan, string, string, string, TimeSpan, DateTime, string, int)>();
        public Email() { }

        public static void SendToEmail(List<(int, int)> _selectedSeats, string emailTo)
        {
            
            info_film_Word = Day_Seances.getInfoOneFilm(Panell.film_title, Panell.film_starttime, Panell.date_film, Panell.film_hall);
            var (filmID, title, duration, genre, ageRestriction, description, startTime, date, hall, price) = info_film_Word[0];

            var firstSeat = _selectedSeats[Buy.index];
            int rowNumber = firstSeat.Item1;
            int seatNumber = firstSeat.Item2;
            Ticket ticket = new Ticket();
            Program.ticket.Show();

            Program.ticket.label1.Text = date.ToString("dd.MM.yyyy");
            Program.ticket.label2.Text = "ряд: " + rowNumber.ToString();
            Program.ticket.label3.Text = "место: " + seatNumber.ToString();
            Program.ticket.label4.Text = "начало: " + startTime.ToString("hh\\:mm");
            Program.ticket.label5.Text = "цена: " + price.ToString() + " рублей";
            Program.ticket.label6.Text = "фильм: " + title.ToString();
            Program.ticket.label10.Text = hall;

            Bitmap Ticket = new Bitmap(Program.ticket.Width, Program.ticket.Height);
            Program.ticket.DrawToBitmap(Ticket, new Rectangle(0, 0, Ticket.Width, Ticket.Height));
            Program.ticket.Close();
            using (var memoryStream = new System.IO.MemoryStream())
            {
                Ticket.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                memoryStream.Seek(0, SeekOrigin.Begin);


                using (var message = new MailMessage())
                {
                    message.From = new MailAddress("f.blue7@mail.ru");
                    message.To.Add(emailTo);
                    message.Subject = "Билет в кино";
                    message.Body = $"Здравствуйте! Вот Ваш электронный билет. \n\nC уважением кинотеатр «КОСМОС»!";

                    var attachment = new Attachment(memoryStream, "Ticket.png", "image/png");
                    message.Attachments.Add(attachment);

                    using (var smtpClient = new SmtpClient("smtp.mail.ru", 587))
                    {
                        smtpClient.EnableSsl = true;
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.Credentials = new NetworkCredential("f.blue7@mail.ru", "XXX8DdYLJ0mS0e6wj5nW");

                        try
                        {
                            smtpClient.Send(message);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Ошибка отправки сообщения электронной почты: " + ex.Message);
                        }
                    }
                }
            }
        }
    }
}
