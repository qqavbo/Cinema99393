using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Net.Mail;

namespace Cinema
{
    public partial class Input : Form
    {
        private SqlConnection connection;
        public static bool flagAnonim;
        public static string emaiAddress;
        public static string name;
        public static string patronymic;
        
        public Input()
        {
            InitializeComponent();
            
            var connectionString = "Data Source=LAPTOP-JSVR0I7M; Initial Catalog=Cinema; Integrated Security = True";
            connection = new SqlConnection(connectionString);
        }

        private void entry_Click(object sender, EventArgs e)
        {
            flagAnonim = false;
            var email = tb_mail.Text;
            emaiAddress = tb_mail.Text;
            var password = tb_password.Text;
           
        var hashedPassword = HashPassword(password); 

            if (CheckUser(email, hashedPassword))
            {
                var roleId = GetRoleId(email);
                if (roleId == 1)// 1 это админ
                {
                    Admin admin = new Admin(); 
                    admin.Show();
                    Hide();
                }
                else if (roleId == 2) // 2 это обычный чел 
                {
                    Schedule schedule = new Schedule();
                    schedule.Show();
                    Hide();
                }
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль");
                tb_mail.Text = "";
                tb_password.Text = "";
            }
        }
        private bool CheckUser(string email, string hashedPassword)
        {
            try
            {
                connection.Open(); 

                var query = "SELECT COUNT(*) FROM Users WHERE UserMail = @UserMail AND UserPassword = @UserPassword";
                
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserMail", email);
                command.Parameters.AddWithValue("@UserPassword", hashedPassword);

                var result = (int)command.ExecuteScalar();

                return result == 1;
            }
            catch (Exception)
            {
                return false; 
                
            }
            finally
            {
                connection.Close();
            }
        }
        private void registration_Click(object sender, EventArgs e)
        {
            Registration registration = new Registration();
            registration.Show();
            Hide();
        }
        private void anonim_Click(object sender, EventArgs e)
        {
            flagAnonim = true;
            Schedule schedule = new Schedule();
            schedule.Show();
            Hide();
        }

        private int GetRoleId(string email)
        {
            try
            {
                connection.Open(); 

                var query = $"SELECT roles FROM Users WHERE UserMail='{email}'"; 
                var command = new SqlCommand(query, connection);
                var result = (int)command.ExecuteScalar();

                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при получении роли пользователя: {ex.Message}");
                return -1;
            }
            finally
            {
                connection.Close();
            }
        }
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create()) //создаем объект SHA256
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));//хэшируем переданный пароль
                var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();//тут конвертируется хэшированный пароль в чо-то
                return hash;
            }
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                tb_password.UseSystemPasswordChar = false;
            }
            else
            {
                tb_password.UseSystemPasswordChar = true;
            }
        }
    }
}
