using TableTower.Data.Models;

namespace TableTower.Data;
public static class InMemoryDatabase
{
    // Primitive Types
    public static List<int> IntegerData { get; } = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];

    public static List<string> StringData { get; } =
    [
        "Mahammad Ahmadov",
        "Lale Hasanli",
        "Rashad Mammadov",
        "Aygun Aliyeva",
        "Togrul Huseynov",
        "Narmin Karimova",
        "Kamran Safarov",
        "Sevinc Ismayilova",
        "Sahnise Shirinli",
        "Ilkin Murselov"
    ];

    public static List<double> PriceData { get; } = [19.99, 29.99, 39.99, 49.99, 59.99];

    public static List<bool> BooleanData { get; } = [true, false, true, true, false];

    public static List<DateTime> DateData { get; } =
    [
        new DateTime(2023, 1, 15),
        new DateTime(2023, 2, 20),
        new DateTime(2023, 3, 25),
        new DateTime(2023, 4, 10),
        new DateTime(2023, 5, 5)
    ];

    // Simple Classes
    public static List<Product> Products { get; } =
    [
        new() { ID = 1, Name = "Keyboard", Price = 120.50m, Category = "Electronics", Description = "Mechanical keyboard with RGB." },
        new() { ID = 2, Name = "Laptop Stand", Price = 45.00m, Category = "Accessories", Description = "Ergonomic laptop stand." },
        new() { ID = 3, Name = "Monitor", Price = 230.00m, Category = "Electronics", Description = "27-inch 4K monitor." },
        new() { ID = 4, Name = "Wireless Mouse", Price = 65.75m, Category = "Electronics", Description = "Ergonomic wireless mouse." },
        new() { ID = 5, Name = "USB-C Hub", Price = 89.99m, Category = "Accessories", Description = "Multi-port USB-C hub." }
    ];

    public static List<Address> Addresses { get; } =
    [
        new() { Street = "28 May Street", City = "Baku", Country = "Azerbaijan", PostalCode = "AZ1000" },
        new() { Street = "Nizami Street", City = "Baku", Country = "Azerbaijan", PostalCode = "AZ1010" },
        new() { Street = "Khagani Street", City = "Baku", Country = "Azerbaijan", PostalCode = "AZ1005" },
        new() { Street = "Fountain Square", City = "Baku", Country = "Azerbaijan", PostalCode = "AZ1001" },
        new() { Street = "Istiglaliyyat Street", City = "Baku", Country = "Azerbaijan", PostalCode = "AZ1001" }
    ];

    public static List<Department> Departments { get; } =
    [
        new() { ID = 1, Name = "Engineering", EmployeeCount = 45 },
        new() { ID = 2, Name = "Marketing", EmployeeCount = 23 },
        new() { ID = 3, Name = "Sales", EmployeeCount = 30 },
        new() { ID = 4, Name = "Human Resources", EmployeeCount = 10 },
        new() { ID = 5, Name = "Research & Development", EmployeeCount = 18 }
    ];

    public static List<Company> Companies { get; } =
    [
        new() {
            ID = 1,
            Name = "TechSolutions Azerbaijan",
            Headquarters = Addresses[0],
            EmployeeCount = 120,
            AnnualRevenue = 5000000.00m,
            Departments = [Departments[0], Departments[1], Departments[2]]
        },
        new() {
            ID = 2,
            Name = "DataCore Systems",
            Headquarters = Addresses[1],
            EmployeeCount = 85,
            AnnualRevenue = 3200000.00m,
            Departments = [Departments[0], Departments[3]]
        },
        new() {
            ID = 3,
            Name = "InnoTech Azerbaijan",
            Headquarters = Addresses[2],
            EmployeeCount = 150,
            AnnualRevenue = 7800000.00m,
            Departments = [Departments[0], Departments[4], Departments[1]]
        }
    ];

    public static List<User> Users { get; } =
    [
         new() {
             ID = 1,
             Name = "Mahammad Ahmadov",
             Occupation = "Software Developer",
             Country = "Azerbaijan",
             Description = "Builds clean backend systems.",
             HomeAddress = Addresses[0],
             PurchasedProducts = [Products[0], Products[2]],
             Employer = Companies[0]
         },
         new() {
             ID = 2,
             Name = "Lale Hasanli",
             Occupation = "UX Designer",
             Country = "Azerbaijan",
             Description = "Designs intuitive interfaces.",
             HomeAddress = Addresses[1],
             PurchasedProducts = [Products[1]],
             Employer = Companies[0]
         },
         new() {
             ID = 3,
             Name = "Rashad Mammadov",
             Occupation = "Product Manager",
             Country = "Azerbaijan",
             Description = "Leads user-focused products.",
             HomeAddress = Addresses[2],
             PurchasedProducts = [Products[0], Products[1], Products[4]],
             Employer = Companies[1]
         },
         new() {
             ID = 4,
             Name = "Aygun Aliyeva",
             Occupation = "Data Scientist",
             Country = "Azerbaijan",
             Description = "Finds insights in data.",
             HomeAddress = Addresses[3],
             PurchasedProducts = [Products[2], Products[3]],
             Employer = Companies[1]
         },
         new() {
             ID = 5,
             Name = "Togrul Huseynov",
             Occupation = "DevOps Engineer",
             Country = "Azerbaijan",
             Description = "Builds automated pipelines.",
             HomeAddress = Addresses[4],
             PurchasedProducts = [Products[0]],
             Employer = Companies[2]
         },
         new() {
             ID = 6,
             Name = "Narmin Karimova",
             Occupation = "AI Researcher",
             Country = "Azerbaijan",
             Description = "Improves deep learning models.",
             HomeAddress = Addresses[0],
             PurchasedProducts = [Products[2], Products[4]],
             Employer = Companies[2]
         },
         new() {
             ID = 7,
             Name = "Kamran Safarov",
             Occupation = "Security Analyst",
             Country = "Azerbaijan",
             Description = "Secures systems and data.",
             HomeAddress = Addresses[1],
             PurchasedProducts = [Products[3]],
             Employer = Companies[0]
         },
         new() {
             ID = 8,
             Name = "Sevinc Ismayilova",
             Occupation = "Frontend Engineer",
             Country = "Azerbaijan",
             Description = "Builds responsive UIs.",
             HomeAddress = Addresses[2],
             PurchasedProducts = [Products[0], Products[1]],
             Employer = Companies[1]
         },
         new() {
             ID = 9,
             Name = "Sahnise Shirinli",
             Occupation = "Sales Specialist",
             Country = "Azerbaijan",
             Description = "Manages ticket sales.",
             HomeAddress = Addresses[3],
             PurchasedProducts = [Products[4]],
             Employer = Companies[2]
         },
         new() {
             ID = 10,
             Name = "Ilkin Murselov",
             Occupation = "QA Engineer",
             Country = "Azerbaijan",
             Description = "Ensures software quality.",
             HomeAddress = Addresses[4],
             PurchasedProducts = [Products[1], Products[3]],
             Employer = Companies[0]
         },
    ];

    // Complex data - currently not supported
    public static Dictionary<string, object> ComplexData { get; } = new Dictionary<string, object>
    {
        ["Statistics"] = new
        {
            TotalUsers = 10,
            TotalProducts = 5,
            AveragePrice = Products.Average(p => p.Price),
            MostCommonCity = Addresses.GroupBy(a => a.City).OrderByDescending(g => g.Count()).First().Key
        },
        ["TopProducts"] = Products.OrderByDescending(p => p.Price).Take(3).ToList(),
        ["UsersByCompany"] = Companies.ToDictionary(
            c => c.Name,
            c => Users.Where(u => u.Employer.ID == c.ID).ToList()
        )
    };

    // Tuple data
    public static List<(string Name, int Age, string Position)> PersonnelRecords { get; } =
    [
        ("Mahammad Ahmadov", 28, "Senior Developer"),
        ("Lale Hasanli", 25, "Lead Designer"),
        ("Rashad Mammadov", 32, "Product Director"),
        ("Aygun Aliyeva", 29, "Data Scientist"),
        ("Togrul Huseynov", 31, "DevOps Lead")
    ];
}