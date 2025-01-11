namespace MES
{
    public class SolveSysEq
    {
        public int _nodesNumber { get; }
        public double[][] HG { get; }
        public double[][] CG { get; }
        public double[] PG { get; }

        public SolveSysEq(int nodesNumber)
        {
            _nodesNumber = nodesNumber;

            HG = new double[_nodesNumber][];
            CG = new double[_nodesNumber][];
            PG = new double[_nodesNumber];

            for (int i = 0; i < _nodesNumber; i++)
            {
                HG[i] = new double[_nodesNumber];
                CG[i] = new double[_nodesNumber];
            }
        }

        public double[][] Solve(int initialTemp, int simulationStepTime, int simulationTime)
        {
            double[] T = new double[_nodesNumber];
            Array.Fill(T, initialTemp);

            double[][] min_max = new double[simulationTime / simulationStepTime + 1][];

            double[][] A = new double[_nodesNumber][];

            for (int i = 0; i < _nodesNumber; i++)
            {
                A[i] = new double[_nodesNumber];

                for (int j = 0; j < _nodesNumber; j++)                
                    A[i][j] = HG[i][j] + (CG[i][j] / simulationStepTime);                
            }

            for (int t = 0; t * simulationStepTime <= simulationTime; t++)
            {
                min_max[t] = new double[3];

                min_max[t][0] = t * simulationStepTime;
                min_max[t][1] = T.Min();
                min_max[t][2] = T.Max();

                double[] B = new double[_nodesNumber];

                for (int i = 0; i < _nodesNumber; i++)
                {
                    for (int j = 0; j < _nodesNumber; j++)                    
                        B[i] += CG[i][j] / simulationStepTime * T[j];                   

                    B[i] += PG[i];
                }

                T = Gauss(A, B);
            }

            return min_max;
        }

        private double[] Gauss(double[][] matrix, double[] vector)
        {
            int n = vector.Length;

            // Augment the matrix with the vector
            double[,] augmentedMatrix = new double[n, n + 1];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    augmentedMatrix[i, j] = matrix[i][j];
                }
                augmentedMatrix[i, n] = vector[i];
            }

            // Perform Gauss Elimination
            for (int i = 0; i < n; i++)
            {
                // Partial Pivoting
                for (int k = i + 1; k < n; k++)
                {
                    if (Math.Abs(augmentedMatrix[k, i]) > Math.Abs(augmentedMatrix[i, i]))
                    {
                        for (int j = 0; j <= n; j++)
                        {
                            double temp = augmentedMatrix[i, j];
                            augmentedMatrix[i, j] = augmentedMatrix[k, j];
                            augmentedMatrix[k, j] = temp;
                        }
                    }
                }

                // Make the diagonal element 1 and eliminate below
                for (int k = i + 1; k < n; k++)
                {
                    double factor = augmentedMatrix[k, i] / augmentedMatrix[i, i];
                    for (int j = i; j <= n; j++)
                    {
                        augmentedMatrix[k, j] -= factor * augmentedMatrix[i, j];
                    }
                }
            }

            // Back Substitution
            double[] solution = new double[n];
            for (int i = n - 1; i >= 0; i--)
            {
                solution[i] = augmentedMatrix[i, n];
                for (int j = i + 1; j < n; j++)
                {
                    solution[i] -= augmentedMatrix[i, j] * solution[j];
                }
                solution[i] /= augmentedMatrix[i, i];
            }

            return solution;
        }
    }
}
