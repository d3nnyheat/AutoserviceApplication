using Avalonia.Media.Imaging;

namespace AutoserviceApplication;

public partial class Services
{
    public int ID { get; set; }
    public string Name { get; set; }
    public int Duration { get; set; }
    public int Price { get; set; }
    public int Discount { get; set; }
    public Bitmap? Image { get; set; }
    public string ImagePath { get; set; }
}

public partial class Requests
{
    public int ID { get; set; }
    public string ContactInfo { get; set; }
    public string Description { get; set; }
}