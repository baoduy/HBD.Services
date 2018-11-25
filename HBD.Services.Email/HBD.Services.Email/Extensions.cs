﻿using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HBD.Services.Transformation;

[assembly: InternalsVisibleTo("HBD.Services.Email.Tests")]
namespace HBD.Services.Email
{
    public static partial class Extensions
    {
        public static ICollection<T> AddRange<T>(this ICollection<T> @this, IEnumerable<T> collection)
        {
            if (@this == null || @this.IsReadOnly) return @this;
            foreach (var item in collection)
            {
                @this.Add(item);
            }

            return @this;
        }

        internal static string[] SplitBySeparator(this string @this)
            => string.IsNullOrWhiteSpace(@this) ? new string[0] : @this.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);


        private static readonly Regex ValidEmailRegex = CreateValidEmailRegex();

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

        internal static async Task FromAsync(this MailAddressCollection @this, string emailTemplates, ITransformer transformer, params object[] transformData)
        {
            if (emailTemplates == null) return;

            foreach (var s in emailTemplates.SplitBySeparator())
            {
                if (s.IsEmail())
                {
                    @this.Add(s);
                    continue;
                }

                var e = await transformer.TransformAsync(s, transformData).ConfigureAwait(false);
                @this.Add(e);
            }
        }
    }
}
