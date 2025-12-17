using UniDB.Storage;

namespace UniDB.Engine.Executive_Commands;

public class FilterCommand : IExecutableCommand
{
    StorageManagement _storageManagement = StorageManagement.GetInstance();
    
    public void Execute(Command command)
    {
        var results = _storageManagement.Filter(command.Parameters[0], command.Parameters[1]);
        if (results.Count == 0)
        {
            Console.WriteLine("No matching students found.");
            return;
        }
        Console.WriteLine("Filtered Student: ");
        foreach (var student in results)
        {
            Console.WriteLine($"ID={student.Id}, Name={student.Name}, GPA={student.Gpa}");
        }
    }
}