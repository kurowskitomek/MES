namespace MES
{
    public class ElemUniv
    {
        private const double _X1_1 = 0.0;

        private readonly double[] _ksi1 =
        {
            _X1_1
        };

        private readonly double[] _eta1 =
        {
            _X1_1
        };

        private const double _X1_2 = 0.57735026918962576450914878050196;

        private readonly double[] _ksi2 = 
        { 
            -_X1_2, _X1_2, 
            -_X1_2, _X1_2
        };

        private readonly double[] _eta2 = 
        { 
            -_X1_2, -_X1_2,
            _X1_2, _X1_2
        };

        private const double _X1_3 = 0.0;
        private const double _X2_3 = 0.77459666924148337703585307995648;

        private readonly double[] _ksi3 = 
        { 
            -_X2_3, _X1_3, _X2_3, 
            -_X2_3, _X1_3, _X2_3, 
            -_X2_3, _X1_3, _X2_3
        };
        
        private readonly double[] _eta3 = 
        { 
            -_X2_3, -_X2_3, -_X2_3,
            _X1_3, _X1_3, _X1_3,
            _X2_3, _X2_3, _X2_3
        };

        private const double _X1_4 = 0.3399810435848562648026657591032;
        private const double _X2_4 = 0.8611363115940525752239464888928;

        private double[] _ksi4 =
        {
            -_X2_4, -_X1_4, _X1_4, _X2_4,
            -_X2_4, -_X1_4, _X1_4, _X2_4,
            -_X2_4, -_X1_4, _X1_4, _X2_4,
            -_X2_4, -_X1_4, _X1_4, _X2_4
        };

        private double[] _eta4 =
        {
            -_X2_4, -_X2_4, -_X2_4, -_X2_4,
            -_X1_4, -_X1_4, -_X1_4, -_X1_4,
            _X1_4, _X1_4, _X1_4, _X1_4,
            _X2_4, _X2_4, _X2_4, _X2_4
        };

        public double[][] dN_dKsi { get; }
        public double[][] dN_dEta { get; }

        public Surface[] Surfaces { get; }

        public double[][] N { get; }

        public int NCount { get; }

        public ElemUniv(int n)
        {
            NCount = n;

            double[][] _ksi = { _ksi1, _ksi2, _ksi3, _ksi4 };
            double[][] _eta = { _eta1, _eta2, _eta3, _eta4 };

            int _n = (int)(Math.Sqrt(n) - 1.0);

            double[] ksi = _ksi[_n];
            double[] eta = _eta[_n];

            dN_dKsi = new double[n][];
            dN_dEta = new double[n][];
            N = new double[n][];
            Surfaces = new Surface[4];

            Surface surface1 = new Surface(_n + 1);

            for (int i = 0; i < (_n + 1); i++)
            {
                double surfaceEta = -1.0;
                double surfaceKsi = ksi[i];

                double N1 = 0.25 * (1 - surfaceKsi) * (1 - surfaceEta);
                double N2 = 0.25 * (1 + surfaceKsi) * (1 - surfaceEta);
                double N3 = 0.25 * (1 + surfaceKsi) * (1 + surfaceEta);
                double N4 = 0.25 * (1 - surfaceKsi) * (1 + surfaceEta);

                surface1.N[i][0] = N1;
                surface1.N[i][1] = N2;
                surface1.N[i][2] = N3;
                surface1.N[i][3] = N4;
            }

            Surfaces[0] = surface1;

            Surface surface2 = new Surface(_n + 1);

            for (int i = 0; i < (_n + 1); i++)
            {
                double surfaceEta = eta[(i + 1) * (_n + 1) - 1];
                double surfaceKsi = 1.0;

                double N1 = 0.25 * (1 - surfaceKsi) * (1 - surfaceEta);
                double N2 = 0.25 * (1 + surfaceKsi) * (1 - surfaceEta);
                double N3 = 0.25 * (1 + surfaceKsi) * (1 + surfaceEta);
                double N4 = 0.25 * (1 - surfaceKsi) * (1 + surfaceEta);

                surface2.N[i][0] = N1;
                surface2.N[i][1] = N2;
                surface2.N[i][2] = N3;
                surface2.N[i][3] = N4;
            }

            Surfaces[1] = surface2;

            Surface surface3 = new Surface(_n + 1);

            for (int i = 0; i < (_n + 1); i++)
            {
                double surfaceEta = 1.0;
                double surfaceKsi = ksi[n - 1 - i];

                double N1 = 0.25 * (1 - surfaceKsi) * (1 - surfaceEta);
                double N2 = 0.25 * (1 + surfaceKsi) * (1 - surfaceEta);
                double N3 = 0.25 * (1 + surfaceKsi) * (1 + surfaceEta);
                double N4 = 0.25 * (1 - surfaceKsi) * (1 + surfaceEta);

                surface3.N[i][0] = N1;
                surface3.N[i][1] = N2;
                surface3.N[i][2] = N3;
                surface3.N[i][3] = N4;
            }

            Surfaces[2] = surface3;

            Surface surface4 = new Surface(_n + 1);

            for (int i = 0; i < (_n + 1); i++)
            {
                double surfaceEta = eta[n - ((i + 1) * (_n + 1))];
                double surfaceKsi = -1.0;

                double N1 = 0.25 * (1 - surfaceKsi) * (1 - surfaceEta);
                double N2 = 0.25 * (1 + surfaceKsi) * (1 - surfaceEta);
                double N3 = 0.25 * (1 + surfaceKsi) * (1 + surfaceEta);
                double N4 = 0.25 * (1 - surfaceKsi) * (1 + surfaceEta);

                surface4.N[i][0] = N1;
                surface4.N[i][1] = N2;
                surface4.N[i][2] = N3;
                surface4.N[i][3] = N4;
            }

            Surfaces[3] = surface4;

            for (int i = 0; i < n; i++)
            {
                double N1 = 0.25 * (1 - ksi[i]) * (1 - eta[i]);
                double N2 = 0.25 * (1 + ksi[i]) * (1 - eta[i]);
                double N3 = 0.25 * (1 + ksi[i]) * (1 + eta[i]);
                double N4 = 0.25 * (1 - ksi[i]) * (1 + eta[i]);

                N[i] = [ N1, N2, N3, N4 ];

                double dN1_dKsi = -0.25 * (1 - eta[i]);
                double dN2_dKsi = 0.25 * (1 - eta[i]);
                double dN3_dKsi = 0.25 * (1 + eta[i]);
                double dN4_dKsi = -0.25 * (1 + eta[i]);

                double dN1_dEta = -0.25 * (1 - ksi[i]);
                double dN2_dEta = -0.25 * (1 + ksi[i]);
                double dN3_dEta = 0.25 * (1 + ksi[i]);
                double dN4_dEta = 0.25 * (1 - ksi[i]);

                double[] _dN_dKsi =
                {
                    dN1_dKsi,
                    dN2_dKsi,
                    dN3_dKsi,
                    dN4_dKsi,
                };

                double[] _dn_dEta =
                {
                    dN1_dEta,
                    dN2_dEta,
                    dN3_dEta,
                    dN4_dEta,
                };

                dN_dEta[i] = _dn_dEta;
                dN_dKsi[i] = _dN_dKsi;
            }
        }  
    }
}
