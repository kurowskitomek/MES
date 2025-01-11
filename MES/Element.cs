using System.Runtime.CompilerServices;

namespace MES
{
    public class Element
    {
        private readonly ElemUniv _elemUniv;
        private readonly double _conductivity;
        private readonly double _alfa;
        private readonly double _tot;
        private readonly double _density;
        private readonly double _specificHeat;

        private readonly double[] _w1 = { 1.0, 1.0 };

        private readonly double[] _w2 = 
        { 
            0.55555555555555555555555555555556, 
            0.88888888888888888888888888888889, 
            0.55555555555555555555555555555556 
        };

        private readonly double[] _w3 = 
        { 
            0.347854845137453857373063949222, 
            0.652145154862546142626936050778, 
            0.652145154862546142626936050778, 
            0.347854845137453857373063949222 
        };

        public int Id { get; }
        public List<Node> Nodes { get; }
        public Jacobian[]? Jacobians { get; private set; }
        public double[][]? H { get; private set; }
        public double[][]? C { get; private set; }
        public double[][]? Hbc { get; private set; }
        public double[]? P { get; private set; }

        public Element(
            int id,
            List<Node> nodes,
            ElemUniv elemUniv,
            double conductivity,
            double alfa,
            double tot,
            double density,
            double specificHeat)
        {
            Id = id;
            Nodes = nodes;  
            _elemUniv = elemUniv;
            _conductivity = conductivity;
            _alfa = alfa;
            _tot = tot;
            _density = density;
            _specificHeat = specificHeat;
        }   
        
        public void Calculate(SolveSysEq solveSysEq)
        {
            //Agregacja macierzy H

            double[] x = new double[Nodes.Count];
            double[] y = new double[Nodes.Count];

            for (int i = 0; i < Nodes.Count; i++)
            {
                x[i] = Nodes[i].X;
                y[i] = Nodes[i].Y;
            }

            int n = _elemUniv.NCount;
            double[][] _w = { _w1, _w2, _w3 };
            int _n = (int)(Math.Sqrt(n) - 2.0);
            double[] w = _w[_n];

            Jacobian[] jacobians = new Jacobian[n];

            for (int i = 0; i < n; i++)
                jacobians[i] = new Jacobian(
                    _elemUniv.dN_dKsi[i],
                    _elemUniv.dN_dEta[i],
                    x,
                    y);

            double[][] dN_dx = new double[n][];
            double[][] dN_dy = new double[n][];

            for (int i = 0; i < n; i++)
            {
                double dN1_dx =
                    jacobians[i].InvJ[0][0] * _elemUniv.dN_dKsi[i][0] +
                    jacobians[i].InvJ[0][1] * _elemUniv.dN_dEta[i][0];

                double dN2_dx =
                    jacobians[i].InvJ[0][0] * _elemUniv.dN_dKsi[i][1] +
                    jacobians[i].InvJ[0][1] * _elemUniv.dN_dEta[i][1];

                double dN3_dx =
                    jacobians[i].InvJ[0][0] * _elemUniv.dN_dKsi[i][2] +
                    jacobians[i].InvJ[0][1] * _elemUniv.dN_dEta[i][2];

                double dN4_dx =
                    jacobians[i].InvJ[0][0] * _elemUniv.dN_dKsi[i][3] +
                    jacobians[i].InvJ[0][1] * _elemUniv.dN_dEta[i][3];

                double[] _dN_dx = { dN1_dx, dN2_dx, dN3_dx, dN4_dx };

                double dN1_dy =
                    jacobians[i].InvJ[1][0] * _elemUniv.dN_dKsi[i][0] +
                    jacobians[i].InvJ[1][1] * _elemUniv.dN_dEta[i][0];

                double dN2_dy =
                    jacobians[i].InvJ[1][0] * _elemUniv.dN_dKsi[i][1] +
                    jacobians[i].InvJ[1][1] * _elemUniv.dN_dEta[i][1];

                double dN3_dy =
                    jacobians[i].InvJ[1][0] * _elemUniv.dN_dKsi[i][2] +
                    jacobians[i].InvJ[1][1] * _elemUniv.dN_dEta[i][2];

                double dN4_dy =
                    jacobians[i].InvJ[1][0] * _elemUniv.dN_dKsi[i][3] +
                    jacobians[i].InvJ[1][1] * _elemUniv.dN_dEta[i][3];

                double[] _dN_dy = { dN1_dy, dN2_dy, dN3_dy, dN4_dy };

                dN_dx[i] = _dN_dx;
                dN_dy[i] = _dN_dy;
            }

            Jacobians = jacobians;      
          
            H = new double[4][];
            C = new double[4][];
           
            for (int i = 0; i < 4; i++)
            {
                H[i] = new double[4];
                C[i] = new double[4];

                for (int j = 0; j < 4; j++)
                    for (int k = 0; k < n; k++)
                    {
                        int w1 = k % (int)Math.Sqrt(n);
                        int w2 = k / (int)Math.Sqrt(n);

                        double add =
                            _conductivity * jacobians[k].detJ * w[w1] * w[w2] *
                            (dN_dx[k][i] * dN_dx[k][j] + dN_dy[k][i] * dN_dy[k][j]);

                        double add_C =
                            _specificHeat * _density * jacobians[k].detJ * w[w1] * w[w2] * 
                            _elemUniv.N[k][i] * _elemUniv.N[k][j];

                        H[i][j] += add;
                        C[i][j] += add_C;
                        solveSysEq.HG[Nodes[i].Id - 1][Nodes[j].Id - 1] += add;
                        solveSysEq.CG[Nodes[i].Id - 1][Nodes[j].Id - 1] += add_C;
                    }
            }

            Hbc = new double[4][];
            P = new double[4];

            for (int i = 0; i < 4; i++)
                Hbc[i] = new double[4];

            for (int t = 0; t < 4; t++)
                if (Nodes[t].BC && Nodes[GlobalHelpers.mod(t + 1, 4)].BC)
                {
                    double detJ = Math.Sqrt(
                        Math.Pow(Nodes[t].X - Nodes[GlobalHelpers.mod(t - 1, 4)].X, 2) +
                        Math.Pow(Nodes[t].Y - Nodes[GlobalHelpers.mod(t - 1, 4)].Y, 2)) / 2.0;

                    for (int k = 0; k < _elemUniv.Surfaces[t].N.Length; k++)
                        for (int i = 0; i < 4; i++)
                        {
                            P[i] += _alfa * detJ * w[k] * _tot *
                                _elemUniv.Surfaces[t].N[k][i];

                            for (int j = 0; j < 4; j++)
                            {
                                double add =
                                    _alfa * detJ * w[k] *
                                    _elemUniv.Surfaces[t].N[k][i] *
                                    _elemUniv.Surfaces[t].N[k][j];

                                Hbc[i][j] += add;
                                solveSysEq.HG[Nodes[i].Id - 1][Nodes[j].Id - 1] += add;
                            }
                        }
                }

            for (int i = 0; i < 4; i++)           
                solveSysEq.PG[Nodes[i].Id - 1] += P[i];  
            

        }
    }
}
