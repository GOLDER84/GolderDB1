using UniDB.Domain;
using UniDB.Storage;

namespace UniDB.Engine.Executive_Commands;

public class ImportCommand : IExecutableCommand
{
    private readonly StorageManagement _storageManagement = StorageManagement.GetInstance();
    private string BasePath = @"E:\University\Term3\Data Structure\AllProject\UniDB\project01-unidb-GOLDER84\UniDB\UniDB\"; // Update this path

    public void Execute(Command command)
    {
        string fileName = command.Parameters[0];
        string filePath = Path.Combine(BasePath, fileName);
        Console.WriteLine(filePath);

        if (!File.Exists(filePath))
        {
            Console.WriteLine($"File not found: {filePath}");
            return;
        }

        int successCount = 0;
        int failCount = 0;

        foreach (string line in File.ReadLines(filePath))
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            try
            {
                string[] parts = line.Split(" - ");
                
                int id = int.Parse(parts[0].Trim());
                string name = parts[1].Trim().Trim('"');
                double gpa = double.Parse(parts[2].Trim());

                var student = new Student(id, name, gpa);
                if (_storageManagement.InsertOne(student))
                {
                    successCount++;
                }
                else
                {
                    failCount++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing line: {line} - {ex.Message}");
                failCount++;
            }
        }
        Console.WriteLine($"Import completed: {successCount} inserted, {failCount} failed.");
    }
}