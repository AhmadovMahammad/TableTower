namespace TableTower.Data;
public static class InMemoryDatabase
{
    public static List<User> Users { get; } =
    [
         new() { ID = 1, Name = "Mahammad Ahmadov", Occupation = "Software Developer", Country = "Azerbaijan", Description = "Builds clean backend systems." },
         new() { ID = 2, Name = "Lale Hasanli", Occupation = "UX Designer", Country = "Azerbaijan", Description = "Designs intuitive interfaces." },
         new() { ID = 3, Name = "Rashad Mammadov", Occupation = "Product Manager", Country = "Azerbaijan", Description = "Leads user-focused products." },
         new() { ID = 4, Name = "Aygun Aliyeva", Occupation = "Data Scientist", Country = "Azerbaijan", Description = "Finds insights in data." },
         new() { ID = 5, Name = "Togrul Huseynov", Occupation = "DevOps Engineer", Country = "Azerbaijan", Description = "Builds automated pipelines." },
         new() { ID = 6, Name = "Narmin Karimova", Occupation = "AI Researcher", Country = "Azerbaijan", Description = "Improves deep learning models." },
         new() { ID = 7, Name = "Kamran Safarov", Occupation = "Security Analyst", Country = "Azerbaijan", Description = "Secures systems and data." },
         new() { ID = 8, Name = "Sevinc Ismayilova", Occupation = "Frontend Engineer", Country = "Azerbaijan", Description = "Builds responsive UIs." },
         new() { ID = 9, Name = "Sahnise Shirinli", Occupation = "Sales Specialist", Country = "Azerbaijan", Description = "Manages ticket sales." },
         new() { ID = 10, Name = "Ilkin Murselov", Occupation = "QA Engineer", Country = "Azerbaijan", Description = "Ensures software quality." },
    ];

    public static List<Product> Products { get; } =
    [
        new() { ID = 1, Name = "Keyboard", Price = 120.50m, Category = "Electronics", Description = "Mechanical keyboard with RGB." },
        new() { ID = 2, Name = "Laptop Stand", Price = 45.00m, Category = "Accessories", Description = "Ergonomic laptop stand." },
        new() { ID = 3, Name = "Monitor", Price = 230.00m, Category = "Electronics", Description = "27-inch 4K monitor." }
    ];
}