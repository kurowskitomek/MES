using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MES
{
    public class Integration
    {
        public double Gauss1_1D(Func<double, double> f)
        {
            int range = 2;
            double y = f(0);

            return y * range;
        }

        public double Gauss2_1D(Func<double, double> f)
        {
            double[] xk = { -1.0 / Math.Sqrt(3), 1.0 / Math.Sqrt(3) };
            double[] Ak = { 1.0, 1.0 };

            double result = 0;

            for (int i = 0; i < xk.Length; i++)
                result += f(xk[i]) * Ak[i];

            return result;
        }

        public double Gauss3_1D(Func<double, double> f)
        {
            double[] xk = { -Math.Sqrt(3.0 / 5.0), 0.0, Math.Sqrt(3.0 / 5.0) };
            double[] Ak = { 5.0 / 9.0, 8.0 / 9.0, 5.0 / 9.0 };

            double result = 0;

            for (int i = 0; i < xk.Length; i++)
                result += f(xk[i]) * Ak[i];

            return result;
        }

        public double Gauss1_2D(Func<double, double, double> f)
        {
            int range = 2;
            double y = f(0, 0);

            return y * range * range;
        }

        public double Gauss2_2D(Func<double, double, double> f)
        {
            double[] xk = { -1.0 / Math.Sqrt(3), 1.0 / Math.Sqrt(3) };
            double[] Ak = { 1.0, 1.0 };

            double result = 0;

            for (int i = 0; i < xk.Length; i++)        
                for (int j = 0; j < xk.Length; j++)                
                    result += f(xk[i], xk[j]) * Ak[i] * Ak[j];               

            return result;
        }

        public double Gauss3_2D(Func<double, double, double> f)
        {
            double[] xk = { -Math.Sqrt(3.0 / 5.0), 0.0, Math.Sqrt(3.0 / 5.0) };
            double[] Ak = { 5.0 / 9.0, 8.0 / 9.0, 5.0 / 9.0 };

            double result = 0;

            for (int i = 0; i < xk.Length; i++)
                for (int j = 0; j < xk.Length; j++)
                    result += f(xk[i], xk[j]) * Ak[i] * Ak[j];

            return result;
        }

        public double Rect_1D(Func<double, double> f, int n)
        {
            double delta_x = 2.0 / n;
            double result = 0;

            for (int i = 0; i < n; i++)
            {
                double x = (i * delta_x + (i + 1) * delta_x) / 2.0;
                result += f(x) * delta_x;
            }
                

            return result;
        }

        public double Rect_2D(Func<double, double, double> f, int n)
        {
            double delta_x = 2.0 / n;
            double result = 0;

            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    result += f(i * delta_x, j * delta_x) * delta_x * delta_x;

            return result;
        }
    }
}
