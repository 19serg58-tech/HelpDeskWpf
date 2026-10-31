using System.Configuration;
using System.Data;
using System.Windows;
using HelpDeskWpf;
using HelpDeskWpf.Models;

namespace HelpDeskWpf
{
    
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            //создаем контексе базы данных
            using (var db = new AppDbContext())
            {
                // 2.Применяем миграции и создаем начальных пользователей(admin, user)            }
                DbInitializer.Initialize(db);
            }
            //запускаем логин вручную
            var loginWindow= new LoginWindow();
            loginWindow.Show();
        }
    }

}
