using System;
using System.Data.SqlClient;
using System.Configuration;

namespace MusicDirectoryApp.Data
{
    public static class AuthService
    {
        private static readonly string ConnectionString =
            ConfigurationManager.ConnectionStrings["MusicDirectoryDB"].ConnectionString;

        // Текущий пользователь
        public static User CurrentUser { get; private set; }

        // Класс пользователя
        public class User
        {
            public int UserId { get; set; }
            public string Username { get; set; }
            public string Role { get; set; } // "admin" или "guest"
            public bool IsAdmin => Role == "admin";
        }

        // Метод входа
        public static bool Login(string username, string password)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string query = "SELECT user_id, username, user_role FROM Users WHERE username = @username AND password = @password";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    CurrentUser = new User
                    {
                        UserId = (int)reader["user_id"],
                        Username = reader["username"].ToString(),
                        Role = reader["user_role"].ToString()
                    };
                    return true;
                }
            }
            return false;
        }

        // Метод входа как гостя
        public static void LoginAsGuest()
        {
            CurrentUser = new User
            {
                UserId = 0,
                Username = "Гость",
                Role = "guest"
            };
        }

        // Выход
        public static void Logout()
        {
            CurrentUser = null;
        }

        // Проверка, авторизован ли пользователь
        public static bool IsAuthenticated => CurrentUser != null;
    }
}