using System.Runtime.InteropServices;
using System.Text;

namespace MES
{
    public class GlobalHelpers
    {
        public static void PrintGlobalData(GlobalData globalData)
        {
            Console.WriteLine("SimulationTime {0}", globalData.SimulationTime);
            Console.WriteLine("SimulationStepTime {0}", globalData.SimulationStepTime);
            Console.WriteLine("Conductivity {0}", globalData.Conductivity);
            Console.WriteLine("Alfa {0}", globalData.Alfa);
            Console.WriteLine("Tot {0}", globalData.Tot);
            Console.WriteLine("InitialTemp {0}", globalData.InitialTemp);
            Console.WriteLine("Density {0}", globalData.Density);
            Console.WriteLine("SpecificHeat {0}", globalData.SpecificHeat);
            Console.WriteLine("Nodes number {0}", globalData.NodesNumber);
            Console.WriteLine("Elements number {0}", globalData.ElementsNumber);
            Console.WriteLine("*Node");

            foreach (Node node in globalData.Grid.Nodes)
                Console.WriteLine("\t\t{0},\t{1},\t{2}", node.Id, node.X, node.Y);

            Console.WriteLine("*Element, type=DC2D4");

            foreach (Element element in globalData.Grid.Elements)
            {
                Console.Write("\t{0},\t", element.Id);

                foreach (Node node in element.Nodes)
                    Console.Write("{0},\t", node.Id);

                Console.Write(Environment.NewLine);
            }
        }

        public static void PrintVector(double[] vec)
        {
            for (int i = 0; i < vec.Length; i++)
            {
                Console.Write("|");

                double val = vec[i];
                string output = "";

                if (Math.Abs(val) < 0.001)
                {
                    val = Math.Round(val, 0);
                    output = string.Format("{0,8:0}", val);
                }
                else
                {
                    val = Math.Round(val, 3);
                    output = string.Format("{0,8:0.000}", val);
                }

                Console.Write(output);
                Console.Write("{0,3}", "|");
                Console.WriteLine();
                Console.WriteLine();
            }
        }

        public static string BuildRawMatrix(double[][] matrix)
        {
            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j < matrix[i].Length; j++)
                {
                    double val = matrix[i][j];
                    string output = "";

                    if (Math.Abs(val) < 0.001)
                    {
                        val = Math.Round(val, 0);
                        output = string.Format("{0,8:0}", val);
                    }
                    else
                    {
                        val = Math.Round(val, 3);
                        output = string.Format("{0,8:0.000}", val);
                    }

                    stringBuilder.Append(output);
                }

                stringBuilder.AppendLine();
            }

            return stringBuilder.ToString();
        }

        public static string BuildTable(string title, string[] headers, double[][] data)
        {
            StringBuilder stringBuilder = new StringBuilder(title);

            stringBuilder.AppendLine();

            foreach (string header in headers)
                stringBuilder.AppendFormat("+{0,14}", "--------------");

            stringBuilder.Append("+");
            stringBuilder.AppendLine();

            foreach (string header in headers)
            {
                stringBuilder.AppendFormat("|{0,-14:0}", header);
            }

            stringBuilder.Append('|');
            stringBuilder.AppendLine();

            foreach(string header in headers)
                stringBuilder.AppendFormat("+{0,14}", "--------------");

            stringBuilder.Append("+");
            stringBuilder.AppendLine();

            for (int i = 0; i < data.Length; i++)
            {
                for (int j = 0; j < data[i].Length; j++)
                {
                    double val = data[i][j];
                    
                    if (Math.Abs(val) < 0.001)
                    {
                        val = Math.Round(val, 0);
                        stringBuilder.AppendFormat("|{0,14:0}", val);                      
                    }
                    else
                    {
                        val = Math.Round(val, 3);
                        stringBuilder.AppendFormat("|{0,14:0.000}", val);
                    }
                }
               
                stringBuilder.Append('|');
                stringBuilder.AppendLine();

                for (int j = 0; j < data[i].Length; j++)
                    stringBuilder.AppendFormat("+{0,14}", "--------------");

                stringBuilder.Append("+");
                stringBuilder.AppendLine();
            }

            return stringBuilder.ToString();
        }

        public static void PrintMatrix(double[][] matrix)
        {
            for (int i = 0; i < matrix.Length; i++)
            {
                Console.Write("|");

                for (int j = 0; j < matrix[i].Length; j++)
                {
                    double val = matrix[i][j];
                    string output = "";

                    if (Math.Abs(val) < 0.001)
                    {
                        val = Math.Round(val, 0);
                        output = string.Format("{0,8:0}", val);
                    }
                    else
                    {
                        val = Math.Round(val, 3);
                        output = string.Format("{0,8:0.000}", val);
                    }

                    Console.Write(output);
                }

                Console.Write("{0,2}", "|");
                Console.WriteLine();
                Console.WriteLine();
            }
        }

        public static void PrintHGlobal(SolveSysEq solveSysEq)
        {
            Console.WriteLine("H Global");
            PrintMatrix(solveSysEq.HG);
        }

        public static void PrintPGlobal(SolveSysEq solveSysEq)
        {
            Console.WriteLine("P Global");
            PrintVector(solveSysEq.PG);
        }

        public static void PrintCGlobal(SolveSysEq solveSysEq)
        {
            Console.WriteLine("C Global");
            PrintMatrix(solveSysEq.CG);
        }

        public static void PrintMinMax(double[][] min_max)
        {
            Console.Write(BuildMinMax(min_max));
        }

        public static string BuildMinMax(double[][] min_max)
        {            
            string[] headers = [ "Time [s]", "Min temp [*C]", "Max temp [*C]" ];
            return BuildTable("Minimum and Maximum Temperatures", headers, min_max);
        }

        public static int mod(int x, int m)
        {
            int r = x % m;
            return r < 0 ? r + m : r;
        }

        public static void PrintTestCase(int n, Element element)
        {
            if (element.Jacobians is null)
                return;

            for (int i = 0; i < n; i++)
            {
                Console.WriteLine("PCS {0}", i + 1);

                Console.WriteLine("J = ");
                PrintMatrix(element.Jacobians[i].J);
                Console.WriteLine();

                Console.WriteLine("InvJ = ");
                PrintMatrix(element.Jacobians[i].InvJ);
                Console.WriteLine();

                Console.WriteLine("det(J) = {0:0.000000000}", element.Jacobians[i].detJ);
                Console.WriteLine();
            }

            if (element.H is not null)
            {
                Console.WriteLine("H for element - {0}", element.Id);
                PrintMatrix(element.H);
            }

            if (element.Hbc is not null)
            {
                Console.WriteLine("Hbc for element - {0}", element.Id);
                PrintMatrix(element.Hbc);
            }

            if (element.P is not null)
            {
                Console.WriteLine("P for element - {0}", element.Id);
                PrintVector(element.P);
            }

            Console.WriteLine();
        }

        public static GlobalData GetGlobalData(
            string path,
            int n)
        {
            string? text = null;

            text = File.ReadAllText(path);

            ElemUniv elemUniv = new ElemUniv(n);
            DataParser parser = new DataParser(elemUniv);
            GlobalData globalData = parser.Parse(text);

            PrintGlobalData(globalData);

            return globalData;
        }
    }
}
