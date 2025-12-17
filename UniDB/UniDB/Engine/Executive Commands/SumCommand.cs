using UniDB.Storage;

namespace UniDB.Engine.Executive_Commands;

public class SumCommand
{
    StorageManagement _storageManagement = StorageManagement.GetInstance();

    public void Execute(Command command)
    {
        double sum = _storageManagement.Sum(command.Parameters[0]);
        Console.WriteLine("Sum of " + command.Parameters[0] + "s: " + sum);
    }
}