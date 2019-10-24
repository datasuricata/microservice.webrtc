using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace WebRTC.Helpers {
    public static class Helper {
        public static string EnumDisplay(this Enum value) {
            return !(value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DisplayAttribute), false)
                .SingleOrDefault() is DisplayAttribute attribute) ? value.ToString() : attribute.Description;
        }
        
        /// <summary>
        /// encrypt password with MD5 format encoding
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string Encrypt(this string password) {
            if (string.IsNullOrEmpty(password))
                return "";
            var hash = (password += "2sg68kf-fh56g-19ilf3-xvsg4y8aew");
            var md5 = MD5.Create();
            var data = md5.ComputeHash(Encoding.Default.GetBytes(hash));
            var builder = new StringBuilder();
            foreach (var x in data)
                builder.Append(x.ToString("x2"));
            return builder.ToString().ToUpper();
        }
    }
}