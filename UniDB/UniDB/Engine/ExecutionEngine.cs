using UniDB.Domain;
using UniDB.Storage;


namespace UniDB.Engine;

public class ExecutionEngine
{
    private StorageManagement _storageManagement = StorageManagement.GetInstance();
    private readonly Queue<Command> _commandQueue;
    private readonly TransactionStack _transactionStack;
    private readonly Dictionary<CommandType, Action<Command>> _commandHandlers;
    private bool _isBatchMode;
    private bool _inTransactionMode;
    private static ExecutionEngine? _instance;

    public static ExecutionEngine GetInstance()
    {
        if (_instance == null)
        {
            _instance = new ExecutionEngine();
        }

        return _instance;
    }


    public ExecutionEngine()
    {
        _commandQueue = new Queue<Command>();
        _transactionStack = new TransactionStack();
        _commandHandlers = new Dictionary<CommandType, Action<Command>>
        {
            { CommandType.InsertOne, InsertOneHandle },
            { CommandType.DeleteOne, DeleteOneHandle },
            { CommandType.FindById, FindByIdHandle },
            { CommandType.FindAll, FindAllHandle },
            { CommandType.Filter, FilterHandle },
            { CommandType.Count, CountHandle },
            { CommandType.Sum, SumHandle },
            { CommandType.Average, AverageHandle },
            { CommandType.Import, ImportHandle }
        };
    }

    public void ExecuteQuery(Command command)
    {
        switch (command.CommandType)
        {
            case CommandType.BeginTransaction:
                _inTransactionMode = true;
                Console.WriteLine("Transaction started.");
                break;
            case CommandType.Commit:
                _inTransactionMode = false;
                _transactionStack.Commit();
                Console.WriteLine("Transaction completed.");
                break;
            case CommandType.Rollback:
                _inTransactionMode = false;
                List<Command>? commandsToRollback = _transactionStack.Rollback();
                commandsToRollback?.ForEach(ExecuteCommand);
                Console.WriteLine("Rollback completed.");
                break;
            case CommandType.Batch when command.Parameters[0] == "start":
                _isBatchMode = true;
                Console.WriteLine("Batch started.");
                break;
            case CommandType.Batch when command.Parameters[0] == "execute":
                _isBatchMode = false;
                while (_commandQueue.Count > 0)
                {
                    ExecuteCommand(_commandQueue.Dequeue());
                }

                Console.WriteLine("Batch completed.");
                break;
            default:
                if (_inTransactionMode)
                {
                    _transactionStack.BeginTransaction(command);
                    ExecuteCommand(command);
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
            handler(command);
        }
        else
            Console.WriteLine($"Unknown command type: {command.CommandType}");
    }

    public void InsertOneHandle(Command command)
    {
        int id = int.Parse(command.Parameters[0]);
        string name = command.Parameters[1];
        double gpa = double.Parse(command.Parameters[2]);
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

    public void FindByIdHandle(Command command)
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

    public void DeleteOneHandle(Command command)
    {
        int id = int.Parse(command.Parameters[0]);
        bool operation = _storageManagement.DeleteOne(id);
        ;
        if (operation)
        {
            Console.WriteLine("Student deleted.");
        }
        else
        {
            Console.WriteLine("Student not found.");
        }
    }

    public void FindAllHandle(Command command)
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

    public void FilterHandle(Command command)
    {
        var results = _storageManagement.Filter(command.Parameters[0], command.Parameters[1]);
        if (results.Count == 0)
        {
            Console.WriteLine("No matching students found.");
            return;
        }

        Console.WriteLine("Filtered Student: ");
        foreach (var student in results)
        {
            Console.WriteLine($"ID={student.Id}, Name={student.Name}, GPA={student.Gpa}");
        }
    }

    public void CountHandle(Command command)
    {
        int count = _storageManagement.Count();
        Console.WriteLine($"Total students: {count}");
    }

    public void AverageHandle(Command command)
    {
        double average = _storageManagement.Average(command.Parameters[0]);
        Console.WriteLine($"Average of {command.Parameters[0]}s: {average}");
    }

    public void SumHandle(Command command)
    {
        double sum = _storageManagement.Sum(command.Parameters[0]);
        Console.WriteLine($"Sum of {command.Parameters[0]}s: {sum}");
    }

    public void ImportHandle(Command command)
    {
        string BasePath = @"E:\University\Term3\Data Structure\AllProject\UniDB\project01-unidb-GOLDER84\UniDB\UniDB\";
        string fileName = command.Parameters[0];
        string filePath = Path.Combine(BasePath, fileName);
        Console.WriteLine(filePath);

        if (!File.Exists(filePath))
        {
            Console.WriteLine($"File not found: {filePath}");
            return;
        }

        foreach (string line in File.ReadLines(filePath))
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            try
            {
                string[] parts = line.Split(" - ");

                List<string> p = new List<string>();
                p.Add(parts[0]);
                p.Add(parts[1]);
                p.Add(parts[2]);
                Command c = new Command(CommandType.InsertOne, p);
                ExecuteQuery(c);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing line: {line} - {ex.Message}");
            }
        }

        Console.WriteLine("Import completed");
    }
}