using HelpDeskWpf.Models;
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
    /// Interaction logic for TicketEditView.xaml
    /// </summary>
    public partial class TicketEditView : UserControl
    {
        private User _currentUser;
        private Ticket? _editingTicket;
        // Событие для возврата назад
        public event Action? GoBackRequested;
        public event Action? CreateTicketClicked;

        public TicketEditView(User user,Ticket? ticket=null)
        {
            InitializeComponent();
            _currentUser = user;
            _editingTicket = ticket;

            if(_editingTicket  != null)
            {
                //Title = "Редактирование заявки";
                txtTitle.Text = _editingTicket.Title;
                txtDescription.Text = _editingTicket.Description;
                cmbPriority.SelectedIndex = _editingTicket.Priority;
            }
            else
            {
                //Title = "Новая заявка";
                cmbPriority.SelectedIndex = 0;
            }
        }
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text)) return;

            using var db = new AppDbContext();
            if (_editingTicket != null)
            {
                var dbTicket = db.Tickets.Find(_editingTicket.Id);
                if (dbTicket != null)
                {
                    dbTicket.Title = txtTitle.Text.Trim();
                    dbTicket.Description = txtDescription.Text.Trim();
                    dbTicket.Priority = cmbPriority.SelectedIndex;
                    db.SaveChanges();
                }
            }
            else
            {
                var newStatus = db.TicketStatuses.FirstOrDefault(s => s.Name == "Новая");

                var ticket = new Ticket
                {
                    Title = txtTitle.Text.Trim(),
                    Description = txtDescription.Text.Trim(),
                    Priority = cmbPriority.SelectedIndex,
                    AuthorId = _currentUser.Id,
                    CreateDate = DateTime.Now,
                    StatusId = newStatus.Id
                };

                db.Tickets.Add(ticket);
                db.SaveChanges();
            }

            //this.DialogResult = true;
            //this.Close();
            GoBackRequested?.Invoke(); // Сообщаем: "Готово, верни нас назад!"

        }
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            GoBackRequested?.Invoke(); // Просто возвращаемся
        }
        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            CreateTicketClicked?.Invoke();//вместо открытого окна
        }

    }
}
