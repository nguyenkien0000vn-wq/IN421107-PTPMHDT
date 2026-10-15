namespace EduHub.Ch02.Abstractions;

/// <summary>Abstraction cho logging — tách khỏi System.IO.File (DIP).</summary>
public interface IAppLogger
{
    void Log(string message);
}
