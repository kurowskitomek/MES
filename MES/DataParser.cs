using System.Globalization;

namespace MES
{
    public class DataParser
    {
        private readonly ElemUniv elemUniv;

        public DataParser(ElemUniv _elemUniv)
        {
            elemUniv = _elemUniv;
        }

        private void DecodeArray(
            string arrayName, 
            int index, 
            string[] lineArr, 
            GlobalData globalData)
        {
            double[] values = new double[lineArr.Length - 1];
            double[] valuesBC = new double[lineArr.Length];
            Grid grid = globalData.Grid;

            for (int i = 1; i < lineArr.Length; i++)
            {
                double.TryParse(lineArr[i], CultureInfo.InvariantCulture, out double result);
                values[i - 1] = result;
            }

            for (int i = 0; i < lineArr.Length; i++)
            {
                int.TryParse(lineArr[i], CultureInfo.InvariantCulture, out int result);
                valuesBC[i] = result;
            }

            switch (arrayName)
            {
                case "*Node":
                    Node node = new Node();
                    node.Id = index;
                    node.X = values[0];
                    node.Y = values[1];
                    grid.Nodes.Add(node);
                    return;

                case "*Element, type=DC2D4":
                    List<Node> nodes = new List<Node>();

                    foreach (int value in values)                    
                        nodes.Add(grid.Nodes[value - 1]);

                    Element element = new Element(
                        index, 
                        nodes, 
                        elemUniv, 
                        globalData.Conductivity, 
                        globalData.Alfa, 
                        globalData.Tot, 
                        globalData.Density, 
                        globalData.SpecificHeat);

                    grid.Elements.Add(element);

                    break;

                case "*BC":
                    foreach (int value in valuesBC)                    
                        grid.Nodes[value - 1].BC = true;                  

                    break;
            }
        }

        private void DecodeLine(string[] lineArr, GlobalData globalData)
        {
            int count = lineArr.Count();

            if (count < 2 || 
                !double.TryParse(lineArr[count - 1], CultureInfo.InvariantCulture, out double result))
                return;

            switch (lineArr[0])
            {
                case "SimulationTime":
                    globalData.SimulationTime = (int)Math.Round(result);
                    return;
                case "SimulationStepTime":
                    globalData.SimulationStepTime = (int)Math.Round(result);
                    return;
                case "Conductivity":
                    globalData.Conductivity = result;
                    return;
                case "Alfa":
                    globalData.Alfa = result;
                    return;
                case "Tot":
                    globalData.Tot = result;
                    return;
                case "InitialTemp":
                    globalData.InitialTemp = (int)Math.Round(result);
                    return;
                case "Density":
                    globalData.Density = result;
                    return;
                case "SpecificHeat":
                    globalData.SpecificHeat = result;
                    return;
            }

            switch (lineArr[0] + lineArr[1])
            {
                case "Nodesnumber":
                    globalData.NodesNumber = (int)result;
                    return;
                case "Elementsnumber":
                    globalData.ElementsNumber = (int)result;
                    return;
            }
        }

        public GlobalData Parse(string? text)
        {
            Grid grid = new Grid();
            GlobalData globalData = new GlobalData(grid);

            globalData.Grid = grid;

            if (text is null)
                return globalData;

            string? arrayName = null;

            foreach (string? line in text.Split(Environment.NewLine))
            {
                if (string.IsNullOrEmpty(line)) 
                    continue;

                string start = line.TrimStart();

                if (arrayName is not null && 
                    int.TryParse(line.TrimStart().Split(", ")[0], CultureInfo.InvariantCulture, out int index))
                {
                    string[] lineArr = line.Split(", ");
                    DecodeArray(arrayName, index, lineArr, globalData);
                }
                else if (!(line[0] == '*'))
                {
                    string[] lineArr = line.Split(' ');
                    DecodeLine(lineArr, globalData);
                }
                else if (line[0] == '*')
                    arrayName = line;           
            }

            return globalData;
        }
    }
}
