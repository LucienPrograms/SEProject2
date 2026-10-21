using SEProject2.Models;

namespace SEProject2.Domain;

public interface ICommand
{
    public void Execute(Character c);
}