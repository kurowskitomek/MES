using System.Globalization;

namespace MES
{
    internal class Program
    {       
        private static void SetCulture()
        {
            CultureInfo ci = new CultureInfo("en-us");
            Thread.CurrentThread.CurrentCulture = ci;
        }

        static void Main(string[] args)
        {
            SetCulture();

            //TestCases.TestCase0();           
            //TestCases.TestCase1();
            //TestCases.TestCase2();
            GlobalData globalData_test3 = TestCases.TestCase3();
            //GlobalData globalData_test4 = TestCases.TestCase4();
            
            Console.ReadLine();
        }
    }
}




//double gaussNodes[3][3] = { { 0.0}, { -1 / sqrt(3), 1 / sqrt(3)}, { -sqrt(3.0 / 5.0), 0.0, sqrt(3.0 / 5.0)} }

//double[][] points =
//[
//    [ 0.0 ],
//    [ 1.0 / Math.Sqrt(3) ],
//    [ 0.0, Math.Sqrt(3.0 / 5.0) ],
//    [ Math.Sqrt(3.0 / 7.0 - Math.Sqrt(6.0 / 5.0) * 2.0 / 7.0), Math.Sqrt(3.0 / 7.0 + Math.Sqrt(6.0 / 5.0) * 2.0 / 7.0) ],
//    [ 1.0 / 3.0 * Math.Sqrt(5.0 - 2 * Math.Sqrt(10.0 / 7.0)), 1.0 / 3.0 * Math.Sqrt(5.0 + 2.0 * Math.Sqrt(10.0 / 7.0)) ]
//];

//double[][] weights =
//[
//    [ 2.0 ],
//    [ 1.0 ],
//    [ 8.0 / 9.0, 5.0 / 9.0 ],
//    [ (18 + Math.Sqrt(30)) / 36.0, (18 - Math.Sqrt(30)) / 36.0 ],
//    [ 128.0 / 225.0, (322 + 13 * Math.Sqrt(70.0)) / 900.0, (322 - 13 * Math.Sqrt(70.0)) / 900.0 ]
//];

//double[] ksi = { -1.0 / Math.Sqrt(3), 1.0 / Math.Sqrt(3), -1.0 / Math.Sqrt(3), 1.0 / Math.Sqrt(3) };
//double[] eta = { -1.0 / Math.Sqrt(3), -1.0 / Math.Sqrt(3), 1.0 / Math.Sqrt(3), 1.0 / Math.Sqrt(3) };

//double[] w = { 1.0, 1.0 };
