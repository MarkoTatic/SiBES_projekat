using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Formatter
    {
        public static string ParseName(string winLogonName)
        {
            string[] parts = new string[] { };

            if (winLogonName.Contains("\\"))
            {
                /// SPN format
                parts = winLogonName.Split('\\');
                return parts[1];
            }
            else
            {
                return winLogonName;
            }
        }
    }
}
