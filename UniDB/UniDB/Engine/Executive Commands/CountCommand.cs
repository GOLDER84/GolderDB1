using UniDB.Storage;

namespace UniDB.Engine.Executive_Commands;

public class CountCommand : IExecutableCommand
{
    StorageManagement _storageManagement = StorageManagement.GetInstance();
    public void Execute(Command command)
    {
        int count = _storageManagement.Count();
        Console.WriteLine($"Total students: {count}");
    }
}