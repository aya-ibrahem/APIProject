namespace APIProject.Helper
{
    public interface IAppSession
    {
        string AuthorizationToken { get; }
        string UserName { get; }
    }
}
