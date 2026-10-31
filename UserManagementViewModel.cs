using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HelpDeskWpf.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace HelpDeskWpf
{
    public partial class UserManagementViewModel : ObservableObject
    {
        //коллекция для таблицы
        public ObservableCollection<User> Users { get; } = new();
        //свойства для привязки полей ввода
        [ObservableProperty]
        private string? newLogin;
        [ObservableProperty]
        private string? newFullName;
        public UserManagementViewModel()
        {
            LoadUsers();
        }
        [RelayCommand]
        public void LoadUsers()
        {
            using var db = new AppDbContext();
            var list = db.Users.Include(u => u.Role).ToList();

            Users.Clear();
            foreach (var user in list) Users.Add(user);
        }
        // Обрати внимание: метод принимает пароль, так как PasswordBox нельзя привязать
        public void AddUser(string password)
        {
            if (string.IsNullOrWhiteSpace(NewLogin) ||
                string.IsNullOrWhiteSpace(password)) return;
            
            using var db = new AppDbContext();
            
            if(db.Users.Any(u=> u.Login == NewLogin.Trim()))
            {
                MessageBox.Show("Такой логин уже существует!");
                return;
            }

            var role = db.Roles.FirstOrDefault(r => r.Name == "Сотрудник");
            if (role == null) return;

            var newUser = new User
            {
                Login = NewLogin.Trim(),
                Password=password.Trim(),
                Fullname=NewFullName?.Trim() ?? "",
                RoleId=role.Id
            };

            db.Users.Add(newUser);
            db.SaveChanges();

            NewLogin = "";
            NewFullName = "";
            LoadUsers();
        }
        [RelayCommand]
        private void DeleteUser(User user)
        {
            if (user == null) return;
            if (user.Login == "admin")
            {
                MessageBox.Show("Нельзя удалять главного администратора!");
                return;
            }
            using var db = new AppDbContext();
            var dbUser = db.Users.Find(user.Id);
            if(dbUser != null)
            {
                try
                {
                    db.Users.Remove(dbUser);
                    db.SaveChanges();
                    LoadUsers();
                }
                catch (Exception)
                {
                    MessageBox.Show("Нельзя удалять пользователя, у него есть заявки!");
                }
            }
        }

    }
}
