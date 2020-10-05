namespace Utils.ExternalServices
{
    public interface IEmailService
    {
        string EnviarCorreo(string to, string title, string body);
    }
}