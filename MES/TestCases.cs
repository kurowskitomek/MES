namespace MES
{
    public class TestCases
    {
        private static void BaseTestCaseBasic(
            int n,
            double conductivity,
            double alfa,
            double tot,
            double density,
            double specificHeat,
            List<Node> nodes)
        {
            ElemUniv elemUniv = new ElemUniv(n);
            Element element = new Element(0, nodes, elemUniv, conductivity, alfa, tot, density, specificHeat);
            Grid grid = new Grid { Elements = new List<Element> { element } };
            GlobalData globalData = new GlobalData(grid) { Conductivity = conductivity };
            SolveSysEq solveSysEq = new SolveSysEq(nodes.Count);

            element.Calculate(solveSysEq);

            GlobalHelpers.PrintTestCase(n, element);
        }

        public static GlobalData BaseTestCaseInput(string path, int n)
        {
            GlobalData globalData = GlobalHelpers.GetGlobalData(path, n);
            SolveSysEq solveSysEq = new SolveSysEq(globalData.NodesNumber);

            foreach (Element element in globalData.Grid.Elements)
            {
                element.Calculate(solveSysEq);
                GlobalHelpers.PrintTestCase(n, element);
            }

            double[][] solution = solveSysEq.Solve(
                globalData.InitialTemp,               
                globalData.SimulationStepTime, 
                globalData.SimulationTime);

            GlobalHelpers.PrintHGlobal(solveSysEq);
            GlobalHelpers.PrintPGlobal(solveSysEq);
            GlobalHelpers.PrintCGlobal(solveSysEq);
            GlobalHelpers.PrintMinMax(solution);

            return globalData;
        }

        public static GlobalData TestCase4()
        {
            string path = "./Test2_4_4_MixGrid.txt";
            int n = 16;

            return BaseTestCaseInput(path, n);
        }

        public static GlobalData TestCase3()
        {
            string path = "./Test1_4_4.txt";
            int n = 16;
            
            return BaseTestCaseInput(path, n);
        }

        public static void TestCase2()
        {
            int n = 4;
            double conductivity = 30.0;
            double alfa = 25.0;
            double tot = 1200.0;
            double density = 7800;
            double specificHeat = 700;

            List<Node> nodes = new List<Node>
            {
                new Node { Id = 1, X = 0.01, Y = -0.01 },
                new Node { Id = 2, X = 0.025, Y = 0.0 },
                new Node { Id = 3, X = 0.025, Y = 0.025, BC = true },
                new Node { Id = 4, X = 0.0, Y = 0.025, BC = true }
            };

            BaseTestCaseBasic(n, conductivity, alfa, tot, density, specificHeat, nodes);
        }

        public static void TestCase1()
        {
            int n = 16;
            double conductivity = 30.0;
            double alfa = 25.0;
            double tot = 1200.0;
            double density = 7800;
            double specificHeat = 700;

            List<Node> nodes = new List<Node>
            {
                new Node { Id = 1, X = 0.0, Y = 0.0 },
                new Node { Id = 2, X = 0.025, Y = 0.0 },
                new Node { Id = 3, X = 0.025, Y = 0.025 },
                new Node { Id = 4, X = 0.0, Y = 0.025 }
            };

            BaseTestCaseBasic(n, conductivity, alfa, tot, density, specificHeat, nodes);
        }

        public static void TestCase0()
        {
            int n = 4;
            double conductivity = 30.0;
            double alfa = 25.0;
            double tot = 1200.0;
            double density = 7800;
            double specificHeat = 700;

            List<Node> nodes = new List<Node>
            {
                new Node { Id = 1, X = 0.0, Y = 0.0 },
                new Node { Id = 2, X = 0.025, Y = 0.0 },
                new Node { Id = 3, X = 0.025, Y = 0.025, BC = true },
                new Node { Id = 4, X = 0.0, Y = 0.025, BC = true }
            };

            BaseTestCaseBasic(n, conductivity, alfa, tot, density, specificHeat, nodes);
        }
    }
}
