using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HelpDeskWpf.Models;
using Microsoft.EntityFrameworkCore;

namespace HelpDeskWpf;

public partial class LoginViewModel : ObservableObject
{
    [ObservableProperty]
    private string? login;

    [ObservableProperty]
    private string? errorMessage;
    //событие, на которое подпишется окно, чтобы закрываться и открываться MainWindow
    public event Action<User>? LoginSuccessful;
    
   // [RelayCommand]
    public void TryLogin(string password)
    {
        ErrorMessage = "";
        if(string.IsNullOrWhiteSpace(Login) || string.IsNullOrWhiteSpace(password))
        {
            ErrorMessage = "Введите логин и пароль";
            return; 
        }
        
        using var db = new AppDbContext();
        var user = db.Users
            .Include(x => x.Role)
            .FirstOrDefault(u => u.Login == Login && u.Password == password);

        if (user != null)
        {
            LoginSuccessful?.Invoke(user);
        }
        else
        {
            ErrorMessage = "Неверный логин или пароль";
        }
    }
}
