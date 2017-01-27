using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectionSimulator.People;

namespace ElectionSimulator
{
    class Utils
    {
        private static Random random = new Random();

        // Return a double between 0 and 1
        public static double getDouble()
        {
            return random.NextDouble();
        }

        public static double getGaussian(double standardDev)
        {
            double u1 = getDouble();
            double u2 = getDouble();

            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
            return standardDev * randStdNormal;
        }

        // Return a gaussian distributed double between .5 and -.5
        public static double getGaussian()
        {
            double result = getGaussian(.15);

            if (result >= .5)
            {
                return 0.5;
            }

            if (result <= -0.5)
            {
                return -0.5;
            }

            return result;
        }

        // Return a gaussian distributed double between 0 and 1
        public static double getGaussianDouble()
        {
            return .5 + getGaussian();
        }

        public static string ToString(List<Candidate> candidateList)
        {
            if (candidateList.Count == 1)
            {
                return candidateList.First().ToString();
            }

            if (candidateList.Count == 0)
            {
                return "{}";
            }

            string output = "{ ";

            bool firstLine = true;
            foreach (Candidate candidate in candidateList.OrderBy(c => c.index))
            {
                if (!firstLine)
                {
                    output = output + ", ";
                }

                output = output + candidate.ToString();
                firstLine = false;
            }
            output = output + " }";
            return output;
        }
    }
}
