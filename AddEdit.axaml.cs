using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MySql.Data.MySqlClient;

namespace AutoserviceApplication;

public partial class AddEdit : Window
{
    private List<Services> servicesList;
    private Services CurrentService;

    public AddEdit(Services currentService, List<Services> _services)
    {
        InitializeComponent();
        CurrentService = currentService;
        this.DataContext = currentService;
        servicesList = _services;
    }
    private string connString = "server=localhost;database=autoservice;User Id=root;password=landoNorris4";
    private MySqlConnection conn;

    private void Save_OnClick(object? sender, RoutedEventArgs e)
    {
        var usr = servicesList.FirstOrDefault(x => x.ID == CurrentService.ID);
        if (usr == null)
        {
            try
            {
                conn = new MySqlConnection(connString);
                conn.Open();
                string add =
                    "INSERT INTO services (Name, Duration, Price, Discount, Image) VALUES ('" + Name.Text + "', '" + Duration.Text + "', '" + Convert.ToDecimal(Price.Text) + "', '" + Convert.ToInt32(Discount.Text) + "', '" + Img.Text + "');";
                MySqlCommand cmd = new MySqlCommand(add, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error" + exception);
            }
        }
        else
        {
            try
            {
                conn = new MySqlConnection(connString);
                conn.Open();
                string upd = "UPDATE services SET Name = '" + Name.Text + "', Price = '" + Convert.ToDecimal(Price.Text) + "', Duration = '" + Duration.Text + "', Image = '" + Img.Text + "', Discount = '" + Convert.ToInt32(Discount.Text) + "'  WHERE ID = " + Convert.ToInt32(ID.Text) + ";";
                MySqlCommand cmd = new MySqlCommand(upd, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception exception)
            {
                Console.Write("Error" + exception);
            }
        }
    }

    private async void File_Select(object? sender, RoutedEventArgs e)
    {
        try
        {
            OpenFileDialog fileDialog = new OpenFileDialog(); //создание диалогового окна выбора файла
            fileDialog.Filters.Add(new FileDialogFilter() { Name = "Image Files", Extensions = { "jpg", "jpeg", "png", "gif" } }); //ограничение на выбор только изображений
            string[]? fileNames = await fileDialog.ShowAsync(this); //отображение диалогового окна и получение выбранных файлов
            if (fileNames != null && fileNames.Length > 0) //если файл выбран
            {
                string imagePath = System.IO.Path.GetFileName(fileNames[0]); //получение пути к выбранному файлу
                Img.Text = imagePath;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private void GoBack(object? sender, RoutedEventArgs e)
    {
        ServicesForm back = new ServicesForm();
        this.Close();
        back.Show();
    }
}