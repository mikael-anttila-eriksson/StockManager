using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilitiesLib.NET6._0
{
    public static class GenerateError
    {
        public static void DivideByZero()
        {
            int i = 0;
            int j = 1 / i;
        }
        public static void NullRef()
        {
            object m = null;
            string s = m.ToString();
        }
    }
}
