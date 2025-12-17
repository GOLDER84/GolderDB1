namespace UniDB.Engine.Executive_Commands;

public interface IExecutableCommand
{
    void Execute(Command command);
}