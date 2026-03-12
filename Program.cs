using System;
using System.Windows.Forms;
using MusicDirectoryApp.Data;
using MusicDirectoryApp.Forms;

namespace MusicDirectoryApp
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Основной цикл приложения
            while (true)
            {
                // Показываем форму входа
                FormLogin loginForm = new FormLogin();
                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    // Если вход успешен, показываем главную форму
                    Application.Run(new FormMain());

                    // После закрытия главной формы проверяем, вышел ли пользователь
                    // Если AuthService.CurrentUser == null, значит пользователь вышел
                    if (AuthService.CurrentUser == null)
                    {
                        // Продолжаем цикл - показываем форму входа снова
                        continue;
                    }
                    else
                    {
                        // Пользователь закрыл приложение - выходим
                        break;
                    }
                }
                else
                {
                    // Пользователь отменил вход - завершаем приложение
                    break;
                }
            }
        }
    }
}