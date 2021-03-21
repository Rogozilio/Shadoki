public class MoveManager
{
    private ICommand _command;
    public MoveManager()
    {
        _command = new NoCommand();
    }
    public void SetCommand(ICommand command)
    {
        _command = command;
    }
    public void Go(AI _ai)
    {
        _command.Execute(_ai);
    }
}
