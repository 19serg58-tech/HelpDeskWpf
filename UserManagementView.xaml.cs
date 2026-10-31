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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HelpDeskWpf
{
    /// <summary>
    /// Interaction logic for UserManagementView.xaml
    /// </summary>
    public partial class UserManagementView : UserControl
    {
        private UserManagementViewModel _viewModel;
        public UserManagementView()
        {
            InitializeComponent();
            _viewModel=new UserManagementViewModel();
            DataContext = _viewModel;
        }

        // Этот код остается БЕЗ ИЗМЕНЕНИЙ, он просто берет пароль и чистит поле
        private void BtnAddUser_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.AddUser(txtNewPassword.Password);
            txtNewPassword.Clear();
        }
    }
}
