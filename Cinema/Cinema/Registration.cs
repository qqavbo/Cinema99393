using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace Cinema
{
    public partial class Registration : Form
    {
        public Registration()
        {
            InitializeComponent();
        }
        public static string name;
        public static string patronymic;

        private void register_Click(object sender, EventArgs e)
        {
            var surName = TB_surname.Text;
            name = TB_name.Text;
            patronymic = TB_patronymic.Text;
            var email = TB_mail.Text;
            var password = TB_password.Text;
            var confirmPassword = TB_proof.Text;
            
            try
            {

                string connectionString = "Data Source=LAPTOP-JSVR0I7M; Initial Catalog=Cinema; Integrated Security = True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    if (surName == "" || name == "" || email == "" || password == "" || patronymic == "" || confirmPassword == "")
                    {
                        MessageBox.Show("Не все поля заполнены", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        
                    }

                    else if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                    {
                        MessageBox.Show("Email введен некорректно", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    else if (password != confirmPassword)
                    {
                        MessageBox.Show("Пароли не совпадают", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    { 
                    string checkUserQuery = "SELECT COUNT(*) FROM Users WHERE UserMail = @UserMail";
                    using (SqlCommand checkUserCommand = new SqlCommand(checkUserQuery, connection))
                    {
                        checkUserCommand.Parameters.AddWithValue("@UserMail", email);
                        int userCount = (int)checkUserCommand.ExecuteScalar();
                        if (userCount > 0)
                        {
                            MessageBox.Show("Пользователь с таким email уже зарегистрирован", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    string insertUserQuery = "INSERT INTO Users (UserSurname, UserName, UserPatronymic, UserMail, UserPassword, roles) VALUES (@UserSurname, @UserName, @UserPatronymic, @UserMail, @UserPassword, @roles)";
                        using (SqlCommand userCommand = new SqlCommand(insertUserQuery, connection))
                        {
                            userCommand.Parameters.AddWithValue("@UserSurname", surName);
                            userCommand.Parameters.AddWithValue("@UserName", name);
                            userCommand.Parameters.AddWithValue("@UserPatronymic", patronymic);
                            userCommand.Parameters.AddWithValue("@UserMail", email);


                            using (SHA256 sha256 = SHA256.Create())
                            {
                                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(confirmPassword));//пароль хэшируется
                                StringBuilder builder = new StringBuilder();// это надо для создания строки из байтового массива(?)
                                for (int i = 0; i < bytes.Length; i++)//перебираем массив
                                {
                                    builder.Append(bytes[i].ToString("x2"));//да
                                }
                                userCommand.Parameters.AddWithValue("@UserPassword", builder.ToString());
                            }
                            userCommand.Parameters.AddWithValue("@roles", 2);
                            userCommand.ExecuteNonQuery();

                            MessageBox.Show("Вы успешно авторизованы!", "Успешно!", MessageBoxButtons.OK);

                            Input input = new Input();
                            input.Show();
                            Hide();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message, "Ошибка!", MessageBoxButtons.OK);
            }
        }

        private void Registration_Load(object sender, EventArgs e)
        {

        }

        private void Registration_FormClosed(object sender, FormClosedEventArgs e)
        {
            Input input = new Input();
            input.Show();
            Hide();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                TB_password.UseSystemPasswordChar = false;
            }
            else
            {
                TB_password.UseSystemPasswordChar = true;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                TB_proof.UseSystemPasswordChar = false;
            }
            else
            {
                TB_proof.UseSystemPasswordChar = true;
            }
        }
    }
}
