using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace AutoserviceApplication;

public partial class MainMenu : Window
{
    public MainMenu()
    {
        InitializeComponent();
    }

    private void ServicesButton_OnClick(object? sender, RoutedEventArgs e)
    {
        ServicesForm servicesForm = new ServicesForm();
        this.Hide();
        servicesForm.Show();
    }

    private void Logon_OnClick(object? sender, RoutedEventArgs e)
    {
        LoginWindow lw = new LoginWindow();
        this.Hide();
        lw.Show();
    }
}