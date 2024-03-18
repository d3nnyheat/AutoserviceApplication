using System;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace AutoserviceApplication;

public partial class LoginWindow : Window
{
    public LoginWindow()
    {
        InitializeComponent();
    }

    private void LoginAttempt(object? sender, RoutedEventArgs e)
    {
        if (LoginTextBox.Text == "admin" && PasswordTextBox.Text == "admin")
        {
            MainMenu menu = new AutoserviceApplication.MainMenu(); 
            this.Hide(); 
            menu.Show(); 
        }
        else if (LoginTextBox.Text == "client" && PasswordTextBox.Text == "client")
        {
            ClientServicesForm csf = new ClientServicesForm();
            this.Hide(); 
            csf.Show(); 
        }
        else
        {
            LoginError.IsVisible = true;
        }
    }

    private void AppExit(object? sender, RoutedEventArgs e)
    {
        Environment.Exit(0);
    }
}