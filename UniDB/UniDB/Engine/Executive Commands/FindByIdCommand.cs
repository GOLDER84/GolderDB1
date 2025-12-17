using UniDB.Storage;

namespace UniDB.Engine.Executive_Commands;

public class FindByIdCommand
{
    public StorageManagement _storageManagement = StorageManagement.GetInstance();
    public void Execute(Command command)
    {
        int id = int.Parse(command.Parameters[0]);
        var student = _storageManagement.FindById(id);
        if (student != null)
        {
            Console.WriteLine($"Student found: ID={student.Id}, Name={student.Name}, GPA={student.Gpa}");
        }
        else
        {
            Console.WriteLine("Student not found.");
        }
    }
}