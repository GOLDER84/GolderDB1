using UniDB.Engine.Executive_Commands;


namespace UniDB.Engine;

public class ExecutionEngine
{
    private readonly Queue<Command> _commandQueue;
    private readonly TransactionStack _transactionStack;
    private readonly Dictionary<CommandType, IExecutableCommand> _commandHandlers;
    private bool _isBatchMode = false;
    private bool _inTransactionMode = false;

    public ExecutionEngine()
    {
        _commandQueue = new Queue<Command>();
        _transactionStack = new TransactionStack();
        _commandHandlers = new Dictionary<CommandType, IExecutableCommand>
        {
            { CommandType.InsertOne, new InsertOneCommand() },
            { CommandType.DeleteOne, new DeleteOneCommand() },
            { CommandType.FindById, new FindByIdCommand() },
            { CommandType.FindAll, new FindAllCommand() },
            { CommandType.Filter, new FilterCommand() },
            { CommandType.Count, new CountCommand() },
            { CommandType.Sum, new SumCommand() },
            { CommandType.Average, new AverageCommand() },
            { CommandType.Import, new ImportCommand() }
        };
    }
    public void ExecuteQuery(Command command)
    {
        switch (command.CommandType)
        {
            case CommandType.BeginTransaction:
                _inTransactionMode = true;
                break;
            case CommandType.Commit:
                _inTransactionMode = false;
                _transactionStack.Commit();
                break;
            case CommandType.Rollback:
                _inTransactionMode = false;
                List<Command>? commandsToRollback = _transactionStack.Rollback();
                commandsToRollback?.ForEach(ExecuteCommand);
                break;
            case CommandType.Batch when command.Parameters[0] == "start":
                _isBatchMode = true;
                break;
            case CommandType.Batch when command.Parameters[0] == "execute":
                _isBatchMode = false;
                while (_commandQueue.Count > 0)
                    ExecuteCommand(_commandQueue.Dequeue());
                break;
            default:
                if (_inTransactionMode)
                {
                    ExecuteCommand(command);
                    _transactionStack.BeginTransaction(command);
                }
                else if (_isBatchMode)
                    _commandQueue.Enqueue(command);
                else
                    ExecuteCommand(command);
                break;
        }
    }
    private void ExecuteCommand(Command command)
    {
        if (_commandHandlers.TryGetValue(command.CommandType, out var handler))
        {
            handler.Execute(command);
        }
        else
            Console.WriteLine($"Unknown command type: {command.CommandType}");
    }
}