using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

/// <summary>
/// Summary description for WebsystemStrings
/// </summary>
/// 
namespace WebSystem
{
    public class Strings
    {
        public Strings()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static string removeCharFromString(string strDelete, string strFull)
        {
            string retVal = Regex.Replace(strFull, strDelete, "");

            return retVal;
        }


        public static string Right(string str, int length)
        {
            return str.Substring(str.Length - length, length);
        }


        public static string Left(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            maxLength = Math.Abs(maxLength);

            return (value.Length <= maxLength
                   ? value
                   : value.Substring(0, maxLength)
                   );
        }

        //split stringu, jeden delimiter
        public static string splitString(string _source, string _delimiter, int _poradi)
        {
            char delimiter = Convert.ToChar(_delimiter);
            string[] source = _source.Split(delimiter);
            string result = "";

            int strcount = 0;
            foreach (string word in source)
            {
                if (strcount == _poradi)
                {
                    result = word;
                }
                strcount = strcount + 1;
            }

            return result;
        }

        //generuje náhodný alphanumerický string
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrtstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private static Random random = new Random((int)DateTime.Now.Ticks);

        public static string RandomStringEx(int size)
        {
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }
    }
}

