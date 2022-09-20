using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace DTO.Utils
{
    public static class StringExtensions
    {
        public static string NumbersOnly(this string me) => string.IsNullOrWhiteSpace(me) ? null : Regex.Replace(me, @"[^\d]", string.Empty);
        public static bool Contains_InvariantCultureIgnoreCase(this string me, string s) => me.IndexOf(me, StringComparison.InvariantCultureIgnoreCase) > 0;
        public static string CNPJMask(this string me) => me == null ? "" : Int64.Parse(me).ToString(@"00\.000\.000\/0000\-00");
        public static string CPFMask(this string me) => me == null ? "" : Int64.Parse(me).ToString(@"000\.000\.000\-00");

        public static string CNPJOrCPFMask(this string me)
        {
            if (string.IsNullOrWhiteSpace(me)) return "";
            if (me.Length > 11) return me == null ? "" : Int64.Parse(me).ToString(@"00\.000\.000\/0000\-00");

            return me == null ? "" : Int64.Parse(me).ToString(@"000\.000\.000\-00");
        }

        public static string RGMask(this string me) => me == null ? "" : Int64.Parse(me).ToString(@"00\.000\.000\-00");
        public static string CEPMask(this string me) => me == null ? "" : Int64.Parse(me).ToString(@"00000\-000");
        public static string PhoneMask(this string me) => me == null ? "" : Int64.Parse(me).ToString(@"(00) 0000\-0000");
        public static string MobilePhoneMask(this string me) => me == null ? "" : Int64.Parse(me).ToString(@"(00) 00000\-0000");
        public static bool ContainNumbers(this string me) => me.Contains("1") || me.Contains("2") || me.Contains("3") || me.Contains("4") || me.Contains("5") || me.Contains("6") || me.Contains("7") || me.Contains("8") || me.Contains("9") || me.Contains("0");
        public static string IfNullChange(this string str1, string str2, string ignore = null) => string.IsNullOrWhiteSpace((str1 != null && ignore != null) ? str1.Replace(ignore, "") : str1) ? str2 : str1;

        public static string RemoveAccents(this string me)
        {
            byte[] tempBytes = System.Text.Encoding.GetEncoding("ISO-8859-8").GetBytes(me);
            return System.Text.Encoding.UTF8.GetString(tempBytes);
        }

        public static string GetReference()
        {
            var stringBuilder = new StringBuilder();
            var random = new Random();
            for (int j = 1; j <= 4; j++)
            {
                var charCode = random.Next(65, 90);
                stringBuilder.Append((char)charCode);
            }
            stringBuilder.Append(string.Format("{0:0000}", random.Next(0, 9999)));
            return $"{stringBuilder}";
        }

        public static string CondominiumMask(this int condominiumId) => $"COND{condominiumId:000000}";
        public static string AdministratorMask(this int administratorId) => $"ADM{administratorId:000000}";
        public static string WorkerMask(this int administratorId) => $"TRA{administratorId:000000}";
    }
}
