using UniDB.Storage;

namespace UniDB.Engine.Executive_Commands;

public class AverageCommand : IExecutableCommand
{
    StorageManagement _storageManagement = StorageManagement.GetInstance();
    
    public void Execute(Command command)
    {
        double average = _storageManagement.Average(command.Parameters[0]);
        Console.WriteLine($"Average of {command.Parameters[0]}s: {average}");
    }
}