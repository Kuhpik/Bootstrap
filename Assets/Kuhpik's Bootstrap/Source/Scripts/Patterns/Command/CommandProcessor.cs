using System.Collections.Generic;

namespace Kuhpik
{
    public sealed class CommandProcessor
    {
        Queue<ICommand> commands;

        public CommandProcessor(int maxCount)
        {
            commands = new Queue<ICommand>(maxCount);
        }

        public void Process(ICommand command)
        {
            commands.Enqueue(command);
            command.Execute();
        }

        public void Undo()
        {
            if (commands.Count == 0) return;
            commands.Dequeue().Undo();
        }
    }
}