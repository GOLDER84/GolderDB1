namespace UniDB.Engine;

public class Command
{
    public CommandType CommandType { get; set; }
    public List<string> Parameters { get; set; } = new List<string>();

    public Command(CommandType commandType , List<string> parameters)
    {
        commandType = commandType;
        Parameters = parameters;
    }

}