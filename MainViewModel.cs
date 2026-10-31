using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HelpDeskWpf.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace HelpDeskWpf
{
   public partial class MainViewModel : ObservableObject
    {
        private User _currentUser;
        public User CurrentUser => _currentUser;
        public string[] StatusList { get; } = { "Все", "Новая", "В работе", "Решена" };
        //заявки из таблицы
        public ObservableCollection<Ticket> Tickets { get; } = new();

        [ObservableProperty]
        private string? searchText;
        [ObservableProperty]
        private string? selectedStatus;

        public MainViewModel(User user)
        {
            _currentUser= user;
            SelectedStatus = "Все";
            LoadTickets(); //загрузка заявок при старте
        }
        // 2. МАГИЯ MVVM: Когда пользователь вводит текст или меняет статус, 
        // эти методы сработают АВТОМАТИЧЕСКИ (благодаря генератору кода)
        partial void OnSearchTextChanged(string? value) => LoadTickets();
        partial void OnSelectedStatusChanged(string? value) => LoadTickets();

        [RelayCommand]
        private void LoadTickets()
        {
            using var db = new AppDbContext();
            var query = db.Tickets
                .Include(t => t.Status)
                .Include(t => t.Author)
                .Include(t => t.Assignee)
                .AsQueryable();

            // 1. Фильтр по роли
            if (_currentUser.Role.Name == "Сотрудник")
            {
                query = query.Where(t => t.AuthorId == _currentUser.Id);
            }

            // 2. Фильтрация по статусу (ВОТ ЗДЕСЬ ИСПРАВЛЕНИЕ)
            if (!string.IsNullOrEmpty(SelectedStatus) && SelectedStatus != "Все")
            {
                // Ищем по имени статуса, а не по ID автора!
                query = query.Where(t => t.Status.Name == SelectedStatus);
            }

            // 3. Поиск по теме
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                query = query.Where(t => t.Title.Contains(SearchText));
            }

            var list = query.OrderByDescending(t => t.CreateDate).ToList();

            Tickets.Clear();
            foreach (var ticket in list)
            {
                Tickets.Add(ticket);
            }
        }
        [RelayCommand]
        private void TakeToWork(Ticket ticket)
        {
            if (ticket == null || _currentUser.Role.Name != "Специалист") return;

            using var db = new AppDbContext();
            var dbTicket = db.Tickets.Find(ticket.Id);
            var status = db.TicketStatuses.FirstOrDefault(s => s.Name == "В работе");

            if(dbTicket != null && status != null)
            {
                dbTicket.StatusId = status.Id;
                dbTicket.AssigneeId = _currentUser.Id;
                db.SaveChanges();

                LoadTickets(); //обновляем таблицу
            }
        }
        [RelayCommand]
        private void DeleteTicket(Ticket ticket)
        {
            if (ticket == null || _currentUser.Role.Name != "Специалист") return;
            using var db = new AppDbContext();
            var dbTicket = db.Tickets.Find(ticket.Id);
            if(dbTicket != null)
            {
                db.Tickets.Remove(dbTicket);
                db.SaveChanges();
                LoadTickets(); //Обновляем таблицу
            }
        }
        [RelayCommand]
        private void ResolveTicket(Ticket ticket)
        {
            if (ticket == null || _currentUser.Role.Name != "Специалист") return;
            using var db = new AppDbContext();
            var dbTicket = db.Tickets.Find(ticket.Id);

            var status = db.TicketStatuses.FirstOrDefault(s => s.Name == "Решена");
            if(dbTicket !=null && status != null)
            {
                dbTicket.StatusId = status.Id;
                db.SaveChanges();
                
                LoadTickets();    
            }

        }
    }
}
