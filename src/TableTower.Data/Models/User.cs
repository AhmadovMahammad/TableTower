namespace TableTower.Data.Models;

public class User
{
    public int ID { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Occupation { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Address HomeAddress { get; set; } = new Address();
    public List<Product> PurchasedProducts { get; set; } = new List<Product>();
    public Company Employer { get; set; } = new Company();
}
