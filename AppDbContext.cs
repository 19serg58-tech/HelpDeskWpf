using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.IO;
using HelpDeskWpf.Models;


namespace HelpDeskWpf;

public class AppDbContext : DbContext
{
    //*1 Наборы таблиц
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<TicketStatus> TicketStatuses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //string dbPath = Path.Combine(AppContext.BaseDirectory, "helpdesk_v3.db");
        //optionsBuilder.UseSqlite($"Data Source={dbPath}");
        //*********
        string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        string folder = Path.Combine(appDataPath, "HelpDeskWpf");
        //создаем папку, если ее нет
        if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
        string dbPath = Path.Combine(folder, "helpdesk.db");
        optionsBuilder.UseSqlite($"Data Source={dbPath}");
        //**********
    }
    //настройка связей
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //связь 1 Автор заявки
        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.Author) //у заявки есть один автор
            .WithMany(u => u.CreatedTickets) //у автора много заявок
            .HasForeignKey(t => t.AuthorId)  //связь через поле AuthorId
            .OnDelete(DeleteBehavior.Restrict); //Запрещается удалять юзера, если у него есть заявки (защита данных)
        //связь 2 исполнитель заявки
        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.Assignee) //у заявки есть один исполнитель
            .WithMany(u => u.AssignedTicket) //у исполнителя много заявок
            .HasForeignKey(t => t.AssigneeId) //связь через поле AssigneeId
            .OnDelete(DeleteBehavior.Restrict);
        //связь 3 статус заявки
        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.Status) //у заявки один статус
            .WithMany()//у статуса нен списаа заявок (он не нужен)
            .HasForeignKey(t => t.StatusId); //связь через StatusId
    }
}
