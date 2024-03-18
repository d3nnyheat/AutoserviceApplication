using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using MySql.Data.MySqlClient;

namespace AutoserviceApplication;

public partial class ServicesForm : Window
{
    public ServicesForm()
    {
        InitializeComponent();
        string fullTableShow = "SELECT * FROM services;";
        ShowTable(fullTableShow);
        FillCmb();
    }
    private List<Services> serviceslist;
    private string connString = "server=localhost;database=autoservice;User Id=root;password=landoNorris4";
    private MySqlConnection conn;
    string fullTable = "SELECT * FROM services";

    private void ShowTable(string sql)
    {
        serviceslist = new List<Services>();
        conn = new MySqlConnection(connString);
        conn.Open();
        MySqlCommand command = new MySqlCommand(sql, conn);
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read() && reader.HasRows)
        {
            var newService = new Services()
            {
                ID = reader.GetInt32("ID"),
                Name = reader.GetString("Name"),
                Duration = reader.GetInt32("Duration"),
                Price = reader.GetInt32("Price"),
                Discount = reader.GetInt32("Discount"),
                Image = LoadImage("avares://AutoserviceApplication/Images/" + reader.GetString("Image"))
            };
            serviceslist.Add(newService);
        }
        conn.Close();
        ServicesGrid.ItemsSource = serviceslist;
    }
    public void FillCmb()
    {
        serviceslist = new List<Services>();
        conn = new MySqlConnection(connString);
        conn.Open();
        MySqlCommand command = new MySqlCommand(fullTable, conn);
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read() && reader.HasRows)
        {
            var Serv = new Services()
            {
                ID = reader.GetInt32("ID"),
                Name = reader.GetString("Name"),
                Duration = reader.GetInt32("Duration"),
                Price = reader.GetInt32("Price"),
                Discount = reader.GetInt32("Discount"),
                Image = LoadImage("avares://AutoserviceApplication/Images/" + reader.GetString("Image"))
            };
            serviceslist.Add(Serv);
        }
        conn.Close();
        var discontComboBox = this.Find<ComboBox>("DiscontComboBox");
        discontComboBox.ItemsSource = new List<string>
        {
            "Все скидки",
            "От 0% до 5%",
            "От 5% до 15%",
            "От 15% до 30%",
            "От 30% до 70%"
        };
    }
    public Bitmap LoadImage(string Uri)
    {
        return new Bitmap(AssetLoader.Open(new Uri(Uri)));
    }
    private void ResetTable_OnClick(object? sender, RoutedEventArgs e)
    {
        SearchTextBox.Text = string.Empty;
        string fullTableShow = "SELECT * FROM services;";
        ShowTable(fullTableShow);
    }
    private void SortAscending(object? sender, RoutedEventArgs e)
    {
        var sortedItems = ServicesGrid.ItemsSource.Cast<Services>().OrderBy(s => s.Price).ToList();
        ServicesGrid.ItemsSource = sortedItems;
    }
    private void DiscountFilter(object? sender, SelectionChangedEventArgs e)
    {
        var discontComboBox = (ComboBox)sender;
        var selectedDiscount = discontComboBox.SelectedItem as string;
            
        int startDiscount = 0;
        int endDiscount = 0;
        if (selectedDiscount == "Все услуги")
        {
            ServicesGrid.ItemsSource = serviceslist;
        }
        else if (selectedDiscount == "От 0% до 5%")
        {
            startDiscount = 0;
            endDiscount = 5;
        }
        else if (selectedDiscount == "От 5% до 15%")
        {
            startDiscount = 5;
            endDiscount = 15;
        }
        else if (selectedDiscount == "От 15% до 30%")
        {
            startDiscount = 15;
            endDiscount = 30;
        }
        else if (selectedDiscount == "От 30% до 70%")
        {
            startDiscount = 30;
            endDiscount = 71;
        }
        if (startDiscount != 0 && endDiscount != 0)
        {
            var filteredUsers = serviceslist
                .Where(x => x.Discount >= startDiscount && x.Discount < endDiscount)
                .ToList();

            ServicesGrid.ItemsSource = filteredUsers;
        }
    }
    private void SortDescending(object? sender, RoutedEventArgs e)
    {
        var sortedItems = ServicesGrid.ItemsSource.Cast<Services>().OrderByDescending(s => s.Price).ToList();
        ServicesGrid.ItemsSource = sortedItems;
    }
    private void SearchTextBox_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        var srv = serviceslist;
        srv = srv.Where(x => x.Name.Contains(SearchTextBox.Text)).ToList();
        ServicesGrid.ItemsSource = srv;
    }
    private void ServicesGrid_OnLoadingRow(object? sender, DataGridRowEventArgs e)
    {
        Services serviceslist = e.Row.DataContext as Services; 
        if (serviceslist != null) 
        {
            if (5 <= serviceslist.Discount && serviceslist.Discount < 15) 
            {
                e.Row.Background = Brushes.Green;
            }
            else if (15 <= serviceslist.Discount && serviceslist.Discount < 30) 
            {
                e.Row.Background = Brushes.LimeGreen;
            }
            else if (30 <= serviceslist.Discount && serviceslist.Discount <= 70) 
            {
                e.Row.Background = Brushes.PaleGreen;
            }
            else
            {
                e.Row.Background = Brushes.Transparent; 
            }
        }
    }

    private void AddData(object? sender, RoutedEventArgs e)
    {
        Services newService = new Services();
        AddEdit add = new AddEdit(newService, serviceslist);
        add.Title = "Добавление данных";
        add.Header.Text = "Добавление данных";
        add.Show();
        this.Close();
    }

    private void EditData(object? sender, RoutedEventArgs e)
    {
        Services currentService = ServicesGrid.SelectedItem as Services;
        if (currentService == null)
            return;
        AddEdit edit = new AddEdit(currentService, serviceslist);
        edit.Title = "Редактирование данных";
        edit.Header.Text = "Редактирование данных";
        edit.Show();
        this.Close();
    }

    private void DeleteData(object? sender, RoutedEventArgs e)
    {
        try
        {
            Services srv = ServicesGrid.SelectedItem as Services;
            if (srv == null)
            {
                return;
            }
            conn = new MySqlConnection(connString);
            conn.Open();
            string sql = "DELETE FROM services WHERE ID = " + srv.ID;
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
            serviceslist.Remove(srv);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private void BackToMenu(object? sender, RoutedEventArgs e)
    {
        MainMenu menu = new MainMenu();
        Close();
        menu.Show();
    }
}
