namespace HBD.Services.Email.Configurations
{
    public interface IEmailInfo
    {
        string Name { get; }
        string[] ToEmails { get; }
        string[] CcEmails { get; }
        string[] BccEmails { get; }
        string Subject { get; }
        string Body { get; }
        bool IsBodyHtml { get; }
    }
}