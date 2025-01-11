namespace MES
{
    public class GlobalData
    {
        public int SimulationTime { get; set; }
        public int SimulationStepTime { get; set; }
        public double Conductivity { get; set; }
        public double Alfa { get; set; }
        public double Tot { get; set; }
        public int InitialTemp { get; set; }
        public double Density { get; set; }
        public double SpecificHeat { get; set; }
        public int NodesNumber { get; set; }
        public int ElementsNumber { get; set; }

        public Grid Grid { get; set; }

        public GlobalData(Grid grid)
        {
            Grid = grid;
        }
    }
}
