namespace UniDB.Benchmark;

public class DataGenerator
{
    private static readonly string[] FirstNames = 
    {
        "Ali", "Vali", "Mohammad", "Ahmad", "Fatima", "Aye", "Zeynab", 
        "Mustafa", "Hasan", "Hosein", "Elaheh", "Maryam", "Omar", "Cadu",
        "Deniz", "Ece", "Berk", "Selin", "Kaan", "Yusuf"
    };

    public static void GenerateCSV(string filePath, int count)
    {
        var random = new Random();
        using var writer = new StreamWriter(filePath);
        
        for (int i = 1; i <= count; i++)
        {
            string name = FirstNames[random.Next(FirstNames.Length)];
            double gpa = Math.Round(random.NextDouble() * 20, 2);
            writer.WriteLine($"{i} - \"{name}\" - {gpa}");
        }
        
        Console.WriteLine($"Generated {count} records to {filePath}");
    }
}