using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace TamagochiLib
{
    class Misc
    {
        // Возвращает имя объекта
        public static string GetObjectName(object o)
        {
            string type = o.GetType().ToString();
            Regex regex = new Regex(@"^(\w*)\.");
            return regex.Replace(type, "");

        }

        // кидает исключения
        public static void ThrowEX(string e) => throw new Exception(e);
    }
}
