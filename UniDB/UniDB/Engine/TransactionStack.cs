using UniDB.Storage;

namespace UniDB.Engine;

public class TransactionStack
{
    private readonly Stack<Command> _transactions = new Stack<Command>();
    private StorageManagement _storageManagement = StorageManagement.GetInstance();

    public void BeginTransaction(Command command)
    {
        if (command.CommandType == CommandType.InsertOne)
        {
            var reverseCommand = new Command(CommandType.DeleteOne, new List<string>
            {
                command.Parameters[0]
            });
            _transactions.Push(reverseCommand);
        }
        else if (command.CommandType == CommandType.DeleteOne)
        {
            var student = _storageManagement.FindById(int.Parse(command.Parameters[0]));
            if (student == null)
            {
                Console.WriteLine("Student not found for rollback.");
            }

            var reverseCommand = new Command(CommandType.InsertOne, new List<string>
            {
                student.Id.ToString(),
                student.Name,
                student.Gpa.ToString()
            });
            _transactions.Push(reverseCommand);
        }
    }

    public List<Command>? Rollback()
    {
        List<Command> commands = new List<Command>();
        if (_transactions.Count == 0)
        {
            return null;
        }

        while (_transactions.Count > 0)
        {
            commands.Add(_transactions.Pop());
        }

        return commands;
    }

    public void Commit()
    {
        _transactions.Clear();
    }
}