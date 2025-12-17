using UniDB.Storage;

namespace UniDB.Engine.Executive_Commands;

public class DeleteOneCommend
{
    private StorageManagement _storageManagement = StorageManagement.GetInstance();

    public void Execute(Command command)
    {
        int id = int.Parse(command.Parameters[0]);
        bool operation = _storageManagement.DeleteOne(id);;
        if (operation)
        {
            Console.WriteLine("Student deleted.");
        }
        else
        {
            Console.WriteLine("Student not found.");
        }
    }
}