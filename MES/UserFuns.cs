using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MES
{
    public class UserFuns
    {
        public static double f1(double x) 
            => 5 * x * x + 3 * x + 6;
        public static double f2(double x, double y) 
            => 5 * x * x * y * y + 3 * x * y + 6;
    }
}
