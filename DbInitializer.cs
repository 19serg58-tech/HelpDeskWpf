using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelpDeskWpf.Models;
using Microsoft.EntityFrameworkCore;

namespace HelpDeskWpf;

public static class DbInitializer
{
    public static void Initialize(AppDbContext db)
    {
        //1 применяем миграции (создаем базу и таблицы, если их нет)
        db.Database.Migrate();
        if (db.Users.Any()) return;
        //2 если роли уже есть - значит база уже заполнена (выходим)
        if (db.Roles.Any()) return;
        //3 Создаем роли и статусы
        var roleEmployee = new Role { Name = "Сотрудник" };
        var roleSpecialist = new Role { Name = "Специалист" };

        var statusNew = new TicketStatus { Name = "Новая" };
        var statusInWork= new TicketStatus { Name = "В работе" };
        var statusResolved= new TicketStatus { Name = "Решена" };

        db.Roles.AddRange(roleEmployee, roleSpecialist);
       db.TicketStatuses.AddRange(statusNew, statusInWork, statusResolved);

        //сохраняем, чтобы у ролей и статусов появились Id
        db.SaveChanges();

        // создаем тестовых пользователей
        var admin = new User
        {
            Login = "admin",
            Password = "123",
            Fullname = "Иванов Иван (Админ)",
            RoleId = roleSpecialist.Id
        };
        var user = new User
        {
            Login = "user",
            Password = "123",
            Fullname = "Петров Пётр",
            RoleId = roleSpecialist.Id
        };
        db.Users.AddRange(admin,user);

        //5 финальное сохранение
        db.SaveChanges();
    }
}
