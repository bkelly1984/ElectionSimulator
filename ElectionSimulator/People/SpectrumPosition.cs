using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectionSimulator.People
{
    public class SpectrumPosition
    {
        private float[] positions = new float[Tweakables.SPECTRUM_DIMENSIONS];
        public float deviation { get; private set; }

        // Create a random position in the political spectrum
        public SpectrumPosition(Spectrum spectrum)
        {
            for (int i = 0; i < Tweakables.SPECTRUM_DIMENSIONS; i++)
            {
                positions[i] = getRandomPosition(spectrum.dimensionGaussianArray[i], spectrum.dimensionSkewArray[i]);
            }
        }

        // Return a gaussian distributed double between 0 and 1
        private float getRandomPosition(double gaussianFraction, double skewFraction)
        {
            double position = Utils.getDouble() - .5;

            // Initial position is a combination of a random and gaussian
            position = (1 - gaussianFraction) * position + gaussianFraction * Utils.getGaussian();

            // Skew works by translating a -.5 to .5 gaussian so the old center is moved to skew
            // For example, is skew = -.2 then -.5 to 0 of the old gaussian becomes -.5 to -.2 of the new one
            // and 0 to .5 of the old becomes -.2 to .5 of the new

            if (skewFraction == 0)
            {
                return (float)(.5 + position);
            }

            if (skewFraction > position)
            {
                return (float)(.5 * (.5 + position) / (.5 + skewFraction));
            }

            return (float)(1 - .5 * (.5 - position) / (.5 - skewFraction));
        }

        public void calculatePosition(Electorate electorate)
        {
            float distance;
            double sumOfSquaredDistance = 0.0;

            foreach (Voter otherVoter in electorate.voterArray)
            {
                distance = getDistance(otherVoter.position);
                sumOfSquaredDistance += Math.Pow(distance, 2);
            }

            deviation = (float)Math.Sqrt(sumOfSquaredDistance / electorate.voterArray.Length);
        }

        // Don't forget that the distance in multiple dimensions of distance 1 can be greater than 1! 
        public float getDistance(SpectrumPosition otherPosition)
        {
            double distanceSquared = 0.0;

            for (int i = 0; i < Tweakables.SPECTRUM_DIMENSIONS; i++)
            {
                distanceSquared += Math.Pow(positions[i] - otherPosition.positions[i], 2);
            }

            return (float) Math.Sqrt(distanceSquared);
        }

        /*
        public float getBiasedDistance(SpectrumPosition observerPosition)
        {
            double distance;
            double distanceSquared = 0.0;
            double biasedDistanceSquared = 0.0;

            for (int i = 0; i < Tweakables.SPECTRUM_DIMENSIONS; i++)
            {
                distance = positions[i] - observerPosition.positions[i];
                distanceSquared += Math.Pow(distance, 2);

                // Calculate in confirmation bias for this dimension
                distance *= 1 + Tweakables.CONFIRMATION_BIAS_PERCENT / 100 * Math.Cos(Math.PI * Math.Abs(distance));
                biasedDistanceSquared += Math.Pow(distance, 2);
            }

            // Now calculate the halo effect bias for this candidate
            distance = Math.Sqrt(distanceSquared);
            distance *= 1 + Tweakables.HALO_EFFECT_BIAS_PERCENT / 100 * Math.Cos(Math.PI * Math.Abs(distance));
            return (float) distance;
        }
        */

        override public string ToString()
        {
            string output = "Position: [ ";
            bool firstLine = true;
            foreach (double position in positions)
            {
                if (!firstLine)
                {
                    output = output + ", ";
                }

                output = output + String.Format("{0:0.0000}", position);
                firstLine = false;
            }
            output = output + " ]";
            return output;
        }
    }
}
