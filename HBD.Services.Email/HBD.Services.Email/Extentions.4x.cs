#if !NETSTANDARD2_0
using System.Configuration;
using System.Net.Configuration;
using System.Net.Mail;

namespace HBD.Services.Email
{
    public static partial class Extentions
    {
        internal static MailAddress GetDefaultFromEmail()
            => ConfigurationManager.GetSection("system.net/mailSettings/smtp") is SmtpSection smtp
                ? new MailAddress(smtp.From) : null;

    }
}
#endif
