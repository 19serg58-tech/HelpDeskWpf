using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HelpDeskWpf.Models;

namespace HelpDeskWpf
{
    public partial class TicketsView : UserControl
    {
        // Событие для открытия создания
        public event Action? CreateTicketClicked;

        // НОВОЕ Событие для открытия редактирования (передает саму заявку)
        public event Action<Ticket>? EditTicketClicked;

        public TicketsView()
        {
            InitializeComponent();
        }

        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            CreateTicketClicked?.Invoke();
        }

        // Двойной клик просто вызывает событие и передает туда выбранную заявку!
        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var grid = sender as DataGrid;
            if (grid?.SelectedItem is Ticket selectedTicket)
            {
                EditTicketClicked?.Invoke(selectedTicket);
            }
        }
    }
}