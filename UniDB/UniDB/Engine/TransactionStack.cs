using UniDB.Domain;
using UniDB.Engine.Executive_Commands;
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
            command.CommandType = CommandType.DeleteOne;
            command.Parameters[1] = "";
            command.Parameters[2] = "0";
            _transactions.Push(command);
        }else if (command.CommandType == CommandType.DeleteOne)
        {
            var student = _storageManagement.FindById(int.Parse(command.Parameters[0]));
            command.CommandType = CommandType.InsertOne;
            command.Parameters.Add(student.Name);
            command.Parameters.Add(student.Gpa.ToString());
            _transactions.Push(command);
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