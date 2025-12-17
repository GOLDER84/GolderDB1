using UniDB.Domain;
using UniDB.Storage;

namespace UniDB.Engine.Executive_Commands;

public class InsertOneCommend
{
    private StorageManagement _storageManagement = StorageManagement.GetInstance();
    public void Execute(Command command)
    {
        int id = int.Parse(command.Parameters[0]);
        string name = command.Parameters[1];
        double gpa = int.Parse(command.Parameters[2]);
        var student = new Student(id, name, gpa);
        bool operation = _storageManagement.InsertOne(student);
        if (operation)
        {
            Console.WriteLine("Student inserted.");
        }
        else
        {
            Console.WriteLine("Failed to insert student.");
        }
    }
}