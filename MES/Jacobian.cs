namespace MES
{
    public class Jacobian
    {
        public double[][] J { get; }

        public double[][] InvJ
        {
            get
            {
                double a = 1.0 / detJ;

                double[][] j1 =
                {
                    [J[1][1] * a, -J[0][1] * a],
                    [-J[1][0] * a, J[0][0] * a]
                };

                return j1;
            }
        }

        public double detJ => 
            J[0][0] * J[1][1] - J[0][1] * J[1][0];


        public Jacobian(
            double[] dN_dKsi, 
            double[] dN_dEta,
            double[] x,
            double[] y)
        {
            double dx_dKsi = 
                dN_dKsi[0] * x[0] + 
                dN_dKsi[1] * x[1] + 
                dN_dKsi[2] * x[2] + 
                dN_dKsi[3] * x[3];

            double dx_dEta = 
                dN_dEta[0] * x[0] + 
                dN_dEta[1] * x[1] + 
                dN_dEta[2] * x[2] + 
                dN_dEta[3] * x[3];

            double dy_dKsi =
                dN_dKsi[0] * y[0] +
                dN_dKsi[1] * y[1] +
                dN_dKsi[2] * y[2] +
                dN_dKsi[3] * y[3];

            double dy_dEta =
                dN_dEta[0] * y[0] +
                dN_dEta[1] * y[1] +
                dN_dEta[2] * y[2] +
                dN_dEta[3] * y[3];

            double[][] j =
            {
                [dx_dKsi, dy_dKsi],
                [dx_dEta, dy_dEta]
            };

            J = j;
        }
    }
}
