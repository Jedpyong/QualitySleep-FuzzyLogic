using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QualitySleep_FuzzyLogic.Class
{
    internal class FuzzySet
    {
        FuzzyLogic accessor;
        public double classification_COA = 0.0;
        public double membershipOfweight = 0.0;

        public FuzzyLogic[] maskedRegion = new FuzzyLogic[5];
        public FuzzyLogic[] variance = new FuzzyLogic[5];
        public FuzzyLogic[] classification = new FuzzyLogic[3];

        //input variables
        private const int VERYLOW = 0;
        private const int LOW = 1;
        private const int MODERATE = 2;
        private const int HIGH = 3;
        private const int VERYHIGH = 4;

        //output
        private const int NOTINFECTED = 0;
        private const int MODERATELYINFECTED = 1;
        private const int HIGHLYINFECTED = 2;

        int[,] matched_features = new int[5, 5];

        public void InitializeFuzzySets()
        {
            //input variables
            for (int i = 0; i < 5; i++)
            {
                maskedRegion[i] = new FuzzyLogic();
                variance[i] = new FuzzyLogic();
            }

            //output variable
            for (int i = 0; i < 3; i++)
            {
                classification[i] = new FuzzyLogic();
            }

            //Membership function for Masked Region Input (Infected region)
            maskedRegion[0].Set("VERYLOW", 0, 0, 1, 5, 1, 5, 1, 15, 0);
            maskedRegion[1].Set("LOW", 1, 10, 0, 17.5, 1, 17.5, 1, 25, 0);
            maskedRegion[2].Set("MODERATE", 2, 20, 0, 30, 1, 30, 1, 40, 0);
            maskedRegion[3].Set("HIGH", 3, 35, 0, 47.5, 1, 47.5, 1, 60, 0);
            maskedRegion[4].Set("VERYHIGH", 4, 55, 0, 77.5, 1, 77.5, 1, 100, 0);

            //Membership function for Variance Input
            variance[0].Set("VERYLOW", 0, 0, 1, 40, 1, 40, 1, 65, 0);
            variance[1].Set("LOW", 1, 50, 0, 75, 1, 75, 1, 100, 0);
            variance[2].Set("MODERATE", 2, 85, 0, 110, 1, 110, 1, 135, 0);
            variance[3].Set("HIGH", 3, 120, 0, 145, 1, 145, 1, 170, 0);
            variance[4].Set("VERYHIGH", 4, 155, 0, 180, 1, 180, 1, 300, 0);

            classification[0].Set("NOTINFECTED", 0, 0, 1, 17.5, 1, 17.5, 1, 35, 0);
            classification[1].Set("MODERATELYINFECTED", 1, 30, 0, 47.5, 1, 47.5, 1, 65, 0);
            classification[2].Set("HIGHLYINFECTED", 2, 60, 0, 75, 1, 75, 1, 100, 0);

            //Fuzzy Rules
            matched_features[0, 0] = MODERATELYINFECTED;
            matched_features[0, 1] = MODERATELYINFECTED;
            matched_features[0, 2] = NOTINFECTED;
            matched_features[0, 3] = NOTINFECTED;
            matched_features[0, 4] = NOTINFECTED;

            matched_features[1, 0] = HIGHLYINFECTED;
            matched_features[1, 1] = MODERATELYINFECTED;
            matched_features[1, 2] = MODERATELYINFECTED;
            matched_features[1, 3] = MODERATELYINFECTED;
            matched_features[1, 4] = MODERATELYINFECTED;

            matched_features[2, 0] = HIGHLYINFECTED;
            matched_features[2, 1] = HIGHLYINFECTED;
            matched_features[2, 2] = MODERATELYINFECTED;
            matched_features[2, 3] = MODERATELYINFECTED;
            matched_features[2, 4] = MODERATELYINFECTED;

            matched_features[3, 0] = HIGHLYINFECTED;
            matched_features[3, 1] = HIGHLYINFECTED;
            matched_features[3, 2] = HIGHLYINFECTED;
            matched_features[3, 3] = MODERATELYINFECTED;
            matched_features[3, 4] = MODERATELYINFECTED;

            matched_features[4, 0] = HIGHLYINFECTED;
            matched_features[4, 1] = HIGHLYINFECTED;
            matched_features[4, 2] = HIGHLYINFECTED;
            matched_features[4, 3] = HIGHLYINFECTED;
            matched_features[4, 4] = MODERATELYINFECTED;
        }

        public double computeCentroid(double masked_reg, double var)
        {
            accessor = new FuzzyLogic();
            int i = 0, j = 0;
            double area = 0, centroid = 0, numerator = 0, denominator = 0, minimum = 0.0, centerOfArea = 0.0;

            for (i = 0; i < 5; i++) // maskedRegion input
                for (j = 0; j < 5; j++) // variance input
                {
                    minimum = accessor.min(maskedRegion[i].membership(masked_reg), variance[j].membership(var));
                    if (minimum != 0)
                    {
                        area = classification[matched_features[maskedRegion[i].GetIndex(), variance[j].GetIndex()]].Area(minimum);
                        centroid = classification[matched_features[maskedRegion[i].GetIndex(), variance[j].GetIndex()]].CenterOfArea(minimum);
                        numerator += area * centroid;
                        denominator += area;
                    }
                }

            centerOfArea = numerator / denominator;

            if (denominator == 0.0)
                return 0.0;
            else
                return centerOfArea;
        }

        //Get the membership inference value
        public string defuzzify(double masked_reg, double var)
        {

            InitializeFuzzySets();
            classification_COA = computeCentroid(masked_reg, var);
            double[] membershipArray = new double[5];

            double maxMembership = membershipArray[0];
            string classification_linguistic = "";
            for (int i = 0; i < 3; i++)
            {
                if ((classification[i].membership(classification_COA)) > maxMembership)
                {
                    maxMembership = classification[i].membership(classification_COA);
                    classification_linguistic = classification[i].GetLinguistic();
                }
            }
            return classification_linguistic;
        }
    }

}
