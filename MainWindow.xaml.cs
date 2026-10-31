using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HelpDeskWpf.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Linq;



namespace HelpDeskWpf;


public partial class MainWindow : Window
{

    private MainViewModel _viewModel;
  
   
    public MainWindow(User currentUser)
    {
        InitializeComponent();
        // Передаем пользователя в ViewModel и привязываем его к окну
        _viewModel=new MainViewModel(currentUser);
        DataContext = _viewModel;

        Title = $"Help Desk - {currentUser.Fullname} {currentUser.Role.Name}";
        //если не специалист - скрываем кнопку
        if (currentUser.Role.Name != "Специалист")
        {
            BtnUsersMenu.Visibility = Visibility.Collapsed;
        }
        //загружаем страницу с заявками
        BtnTicketsMenu_Click(null, null);
    }


    //1 открыть заявки
    private void BtnTicketsMenu_Click(object sender, RoutedEventArgs e)
    {
        var ticketsView = new TicketsView();
        ticketsView.DataContext = _viewModel;

        ticketsView.CreateTicketClicked += () => ShowCreateTicketView();

        // Подписываемся на редактирование (передаем заявку)
        ticketsView.EditTicketClicked += (ticket) => ShowCreateTicketView(ticket);

        MainContent.Content = ticketsView;

        HighlightButton(BtnTicketsMenu); // Подсвечиваем кнопку
    }

    // 2. Открыть Пользователей
    private void BtnUsersMenu_Click(object sender, RoutedEventArgs e)
    {
        HighlightButton(BtnUsersMenu);
        // Пока у нас Пользователи сделаны в виде отдельного Окна (Window)
        var usersView = new UserManagementView();
        MainContent.Content = usersView;
        HighlightButton(BtnUsersMenu);

    }
    // 3. Открыть Настройки
    private void BtnSettingsMenu_Click(object sender, RoutedEventArgs e)
    {
        var textBlock = new TextBlock
        {
            Text = "Раздел 'Настройки' в разработке...",
            FontSize = 20,
            Foreground = Brushes.Gray,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        };
        MainContent.Content = textBlock;

        HighlightButton(BtnSettingsMenu);
    }

    // 4. Выход из системы
    private void BtnLogout_Click(object sender, RoutedEventArgs e)
    {
        var loginWindow = new LoginWindow();
        loginWindow.Show();
        this.Close();
    }

    // Вспомогательный метод для подсветки активной кнопки
    private void HighlightButton(Button activeButton)
    {
        // Возвращаем всем кнопкам обычный серый стиль
        BtnTicketsMenu.Style = (Style)FindResource("MenuButton");
        BtnUsersMenu.Style = (Style)FindResource("MenuButton");
        BtnSettingsMenu.Style = (Style)FindResource("MenuButton");

        // А нажатую делаем активной (синей)
        activeButton.Style = (Style)FindResource("ActiveMenuButton");
    }

    
    private void ShowCreateTicketView(Ticket? ticketToEdit = null)
    {
        var editView = new TicketEditView(_viewModel.CurrentUser);
        editView.GoBackRequested += () =>
        {
            _viewModel.LoadTicketsCommand.Execute(null);
            BtnTicketsMenu_Click(null, null);
        };
        MainContent.Content = editView;
    }


}