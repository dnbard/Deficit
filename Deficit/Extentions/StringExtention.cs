using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deficit.Extentions
{
    public static class StringExtention
    {
        public static int AsInt(this string str)
        {
            int result = 0;
            Int32.TryParse(str, out result);
            return result;
        }
    }
}
