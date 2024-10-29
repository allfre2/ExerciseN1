namespace AuthService.Exceptions;

public class RoleNotFoundException : Exception
{
    public RoleNotFoundException(string msg) : base(msg) { }
}
