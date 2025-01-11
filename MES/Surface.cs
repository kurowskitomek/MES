namespace MES
{
    public class Surface
    {
        public double[][] N;

        public Surface(int n)
        {
            N = new double[n][];

            for (int i = 0; i < n; i++)            
                N[i] = new double[4];            
        }
    }
}
