using UniDB.Engine;

namespace UniDB.Parser;

public class QueryParser
{
    private readonly ExecutionEngine engine = new ExecutionEngine();
    public void Parse(string query)
    {
        if (string.IsNullOrEmpty(query))
        {
            Console.WriteLine("Query cannot be null or empty");
        }

        var parts = query.Split('.', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length < 2 || parts[0] != "db")
        {
            Console.WriteLine("Invalid query format. Expected: db.collection.command(...) or db.command()");
        }
        
        if (parts.Length == 3 && parts[1] == "batch")
        {
            var cPart = parts[2];
            var cNameEndIndex = cPart.IndexOf('(');
            if (cNameEndIndex == -1 || !cPart.EndsWith(")"))
            {
                Console.WriteLine("Invalid command syntax. Missing parentheses.");
            }
            string batchCommand = cPart.Substring(0, cNameEndIndex); // "start" or "execute"

            if (batchCommand != "start" && batchCommand != "execute")
            {
                Console.WriteLine($"Batch command '{batchCommand}' is not supported. Use 'start' or 'execute'.");
            }
            engine.ExecuteQuery(new Command(CommandType.Batch, new List<string> { batchCommand }));
            return;
        }

        string commandPart;
        if (parts.Length == 2)
        {
            commandPart = parts[1];
        }
        else
        {
            commandPart = string.Join(".", parts.Skip(2));
            // commandPart = parts[2];
        }

        var commandNameEndIndex = commandPart.IndexOf('(');
        if (commandNameEndIndex == -1 || !commandPart.EndsWith(")"))
        {
            Console.WriteLine("Invalid command syntax. Missing parentheses.");
        }

        string commandName = commandPart.Substring(0, commandNameEndIndex);
        string parametersRaw = commandPart.Substring(commandNameEndIndex + 1, commandPart.Length - commandNameEndIndex - 2).Trim();
        CommandType type = ParseCommandType(commandName);

        List<string> parameters;
        if (string.IsNullOrEmpty(parametersRaw))
        {
            parameters = new List<string>();
        }
        else if (parametersRaw.StartsWith("{") && parametersRaw.EndsWith("}"))
        {
            string objectContent = parametersRaw.Substring(1, parametersRaw.Length - 2);
            parameters = string.IsNullOrEmpty(objectContent)
                ? new List<string>()
                : objectContent.Split(',')
                    .Select(p => p.Split(':')[1].Trim().Trim('"'))
                    .ToList();
        }
        else
        {
            parameters = parametersRaw.Split(',')
                .Select(p => p.Trim().Trim('"'))
                .ToList();
        }
        engine.ExecuteQuery(new Command(type, parameters));
    }

    private CommandType ParseCommandType(string commandName)
    {
        return commandName.ToLower() switch
        {
            "insertone" => CommandType.InsertOne,
            "deleteone" => CommandType.DeleteOne,
            "findbyid" => CommandType.FindById,
            "findall" => CommandType.FindAll,
            "filter" => CommandType.Filter,
            "count" => CommandType.Count,
            "sum" => CommandType.Sum,
            "average" => CommandType.Average,
            "import" => CommandType.Import,
            "begintransaction" => CommandType.BeginTransaction,
            "commit" => CommandType.Commit,
            "rollback" => CommandType.Rollback,
            "createindex" => CommandType.CreateIndex,
            "search" => CommandType.Search,
            _ => throw new NotSupportedException($"Command '{commandName}' is not supported.")
        };
    }
}
