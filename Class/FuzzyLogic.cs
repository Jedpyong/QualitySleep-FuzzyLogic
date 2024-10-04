using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QualitySleep_FuzzyLogic.Class
{
    internal class FuzzyLogic
    {
        String name;    //linguistic
        int ndex;       //linguistic index
        int n;          //N of points in membership function def
        double[] x;     //Domain point
        double[] y;     //Corresponding function values;

        //*************************
        public FuzzyLogic()
        {
            x = new double[4];
            y = new double[4];
        }

        public void Set(String nm, int index, double x0, double y0, double x1, double y1, double x2, double y2, double x3, double y3)
        {
            double[] xx = new double[4];
            double[] yy = new double[4];
            name = nm;
            ndex = index;
            xx[0] = x0;
            yy[0] = y0;
            n = 1;

            if ((x1 != x0) || (y1 != y0))
            {
                xx[n] = x1;
                yy[n] = y1;
                ++n;
            }

            if ((x2 != x1) || (y2 != y1))
            {
                xx[n] = x2;
                yy[n] = y2;
                ++n;
            }

            if ((x3 != x2) || (y3 != y2))
            {
                xx[n] = x3;
                yy[n] = y3;
                ++n;
            }

            for (int i = 0; i < n; i++)
            {
                x[i] = xx[i];
                y[i] = yy[i];
            }
        }

        //returns the degree of membership of pt
        public double membership(double pt)
        {
            int lo, mid, hi;
            double yy;

            if (n == 0)
                return 0.0;

            if (pt <= x[0])
                return y[0];

            if (pt >= x[n - 1])
                return y[n - 1];

            lo = 0;
            hi = n - 1;

            for (; ; )
            {
                mid = (lo + hi) / 2;
                if (mid == lo)
                    break;
                if (x[mid] < pt)
                    lo = mid;
                else
                    hi = mid;
            }

            yy = (pt - x[hi - 1]) / (x[hi] - x[hi - 1]) * (y[hi] - y[hi - 1]);
            return yy + y[hi - 1];
        }

        public double Area(double mf)
        {
            if (n == 0) return 0.0;
            else if (n == 2 && (y[0] == 0 || y[0] == 1)) return 0.5 * mf * (x[1] - x[0]) * (2 - mf);
            else return 0.5 * mf * (x[2] - x[0]) * (2 - mf);

        }

        public double CenterOfArea(double mf)
        {
            if (mf == 0) return 0.0;
            if (n == 2 && x[0] == 0)
                return ((1 - mf + mf * mf / 3) * (x[1]) / (2 - mf));
            else if (n == 2)
                return (mf * (x[0] + 2.0 / 3 * mf * (x[1] - x[0])) + 2 * (1 - mf) * (x[0] + 0.5 * (x[1] - x[0]) * (1 + mf))) / (2 - mf);
            else
                return x[1];
        }

        public String GetLinguistic()
        {
            return name;
        }

        public int GetIndex()
        {
            return ndex;
        }

        public double min(double x, double y)
        {
            if (x < y) return x;
            else return y;
        }

        public double max(double x, double y)
        {
            if (x > y)
                return x;
            else return y;
        }
    }



}
