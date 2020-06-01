using Kuhpik;

public sealed class CommandProcessor
{
    private ICommand[] commands;
    private int index;

    public CommandProcessor(int maxCount)
    {
        commands = new ICommand[maxCount];
    }

    public void Process(ICommand command)
    {
        commands.Push(ref index, command);
        command.Execute();
    }

    public void Undo()
    {
        if (index < 0) return;
        commands[index--].Undo();
    }
}