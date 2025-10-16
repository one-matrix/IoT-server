using Microsoft.EntityFrameworkCore;

public class TestDatabase
{
    public static async Task TestConnection()
    {
        try
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseNpgsql("Server=8.138.190.194;UserId=postgres;Password=Ps@admin1234;Database=iot_agent;");
            
            using (var context = new ApplicationDbContext(optionsBuilder.Options))
            {
                await context.Database.CanConnectAsync();
                Console.WriteLine("Database connection successful!");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Database connection failed: {ex.Message}");
        }
    }
}