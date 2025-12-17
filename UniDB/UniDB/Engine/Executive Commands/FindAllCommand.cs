using UniDB.Storage;

namespace UniDB.Engine.Executive_Commands;

public class FindAllCommand : IExecutableCommand
{
    public StorageManagement _storageManagement = StorageManagement.GetInstance();

    public void Execute(Command command)
    {
        var students = _storageManagement.FindAll();
        if (students.Count > 0)
        {
            Console.WriteLine("Students found:");
            foreach (var student in students)
            {
                Console.WriteLine($"ID={student.Id}, Name={student.Name}, GPA={student.Gpa}");
            }
        }
        else
        {
            Console.WriteLine("No students found.");
        }
    }
}