using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HBD.Services.Email.Configurations;
using HBD.Services.Transformation;
using Newtonsoft.Json;

[assembly: InternalsVisibleTo("HBD.Services.Email.Tests")]
namespace HBD.Services.Email
{
    public static partial class Extentions
    {
        internal static IList<EmailTemplate> GetEmailTemplates(this EmailOptions options)
        {
            var list = options.Templates;

            if (string.IsNullOrWhiteSpace( options.TemplateJsonFile)) return list;

            var value = File.ReadAllText(options.TemplateJsonFile);
            var tmps = JsonConvert.DeserializeObject<IEnumerable<EmailTemplate>>(value);

            foreach (var template in tmps)
            {
                template.FromJsonFile = options.TemplateJsonFile;
                list.Add(template);
            }

            return list;
        }

        internal static List<string> GetDuplicated(this IList<EmailTemplate> templates)
            => templates.GroupBy(i => i.Name).Where(g => g.Count() > 1).Select(g => g.Key).Distinct().ToList();

        internal static string[] SplitBySeparator(this string @this)
            =>string.IsNullOrWhiteSpace( @this) ? new string[0] : @this.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);


        static readonly Regex ValidEmailRegex = CreateValidEmailRegex();

        /// <summary>
        /// Taken from http://haacked.com/archive/2007/08/21/i-knew-how-to-validate-an-email-address-until-i.aspx
        /// </summary>
        /// <returns></returns>
        private static Regex CreateValidEmailRegex()
        {
            const string pattern = @"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$";
            return new Regex(pattern, RegexOptions.IgnoreCase);
        }

        private static bool IsEmail(this string emailAddress) => ValidEmailRegex.IsMatch(emailAddress);

        private static async Task FromAsync(this MailAddressCollection @this, string[] templates, ITransformer transformer)
        {
            if (templates == null) return;

            foreach (var s in templates)
            {
                if (s.IsEmail())
                {
                    @this.Add(s);
                    continue;
                }

                var e = await transformer.TransformAsync(s);
                @this.Add(e);
            }
        }

        private static void From(this MailAddressCollection @this, string[] templates, ITransformer transformer)
        {
            if (templates == null) return;

            foreach (var s in templates)
            {
                if (s.IsEmail())
                {
                    @this.Add(s);
                    continue;
                }

                var e = transformer.Transform(s);
                @this.Add(e);
            }
        }

        private static ITransformer CreateEmailTransformer(params object[] trasformData)
            => new Transformer(op =>
            {
                op.TransformData = trasformData;

                foreach (var t in Transformer.DefaultTokenExtractors.Where(i =>
                    !(i is HBD.Services.Transformation.TokenExtractors.AngledBracketTokenExtractor)))
                {
                    op.TokenExtractors.Add(t);
                }
                
            });

        internal static async Task<MailMessage> ToMailMessageAsync(this EmailInfo @this, params object[] trasformData)
        {
            var mail = new MailMessage();

            //Remove the AngledBracketTokenExtractor from Transformer as it might impacts to the html format. 
            using (var ts = CreateEmailTransformer(trasformData))
            {
                await Task.WhenAll(
                    mail.To.FromAsync(@this.ToEmails, ts),
                    mail.CC.FromAsync(@this.CcEmails, ts),
                    mail.Bcc.FromAsync(@this.BccEmails, ts)
                );

                mail.Subject = await ts.TransformAsync(@this.Subject);
                mail.Body = await ts.TransformAsync(@this.Body);
                mail.IsBodyHtml = @this.IsBodyHtml;
            }

            return mail;
        }

        internal static MailMessage ToMailMessage(this EmailInfo @this, params object[] trasformData)
        {
            var mail = new MailMessage();

            //Remove the AngledBracketTokenExtractor from Transformer as it might impacts to the html format. 
            using (var ts = CreateEmailTransformer(trasformData))
            {
                mail.To.From(@this.ToEmails, ts);
                mail.CC.From(@this.CcEmails, ts);
                mail.Bcc.From(@this.BccEmails, ts);

                mail.Subject = ts.Transform(@this.Subject);
                mail.Body = ts.Transform(@this.Body);
                mail.IsBodyHtml = @this.IsBodyHtml;
            }

            return mail;
        }
    }
}
