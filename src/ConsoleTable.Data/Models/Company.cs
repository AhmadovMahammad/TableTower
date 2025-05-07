namespace ConsoleTable.Data.Models;

public class Company
{
    public int ID { get; set; }
    public string Name { get; set; } = string.Empty;
    public Address Headquarters { get; set; } = new Address();
    public int EmployeeCount { get; set; }
    public decimal AnnualRevenue { get; set; }
    public List<Department> Departments { get; set; } = new List<Department>();
}
