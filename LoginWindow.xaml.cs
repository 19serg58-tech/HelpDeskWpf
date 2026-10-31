using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using HelpDeskWpf.Models;

namespace HelpDeskWpf
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private LoginViewModel _viewModel;
        public LoginWindow()
        {
            InitializeComponent();
            _viewModel = new LoginViewModel();
            DataContext = _viewModel;
            // Подписываемся на событие успешного входа из ViewModel
            _viewModel.LoginSuccessful += OnLoginSuccessful;

        }
        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            // Передаем пароль из PasswordBox в ViewModel
            _viewModel.TryLogin(txtPassword.Password);
        }

        private void OnLoginSuccessful(User user)
        {
            // Создаем главное окно (мы его сделаем на следующем шаге)
            // и передаем туда вошедшего пользователя
            var mainWindow =new MainWindow(user);
            //открываем главное окно и закрываем это
            mainWindow.Show();
            this.Close();

        }
    }
}
