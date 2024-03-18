using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MySql.Data.MySqlClient;

namespace AutoserviceApplication;

public partial class RequestForm : Window
{
    private MySqlConnection conn;
    private string connString = "server=localhost;database=autoservice;User Id=root;password=landoNorris4";
    private List<Requests> requestsList;
    private Requests CurrentRequests;

    public RequestForm()
    {
        InitializeComponent();
    }
    private void Save_OnClick(object? sender, RoutedEventArgs routedEventArgs)
    {
        conn = new MySqlConnection(connString);
        conn.Open();
        string request = "INSERT INTO requests (ContactInfo, Description) VALUES ( '" + ContactInfo.Text + "', '" + Description.Text + "');";
        MySqlCommand cmd = new MySqlCommand(request, conn);
        cmd.ExecuteNonQuery();
        conn.Close();
    }

    private void GoBack(object? sender, RoutedEventArgs e)
    {
        AutoserviceApplication.LoginWindow lw = new AutoserviceApplication.LoginWindow();
        this.Close();
        lw.Show();
    }
}
