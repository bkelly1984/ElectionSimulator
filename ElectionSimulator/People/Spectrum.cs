using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectionSimulator.People
{
    // A class describing the political spectrum
    public class Spectrum
    {
        public double[] dimensionGaussianArray { get; } = new double[Tweakables.SPECTRUM_DIMENSIONS];
        public double[] dimensionSkewArray { get; } = new double[Tweakables.SPECTRUM_DIMENSIONS];

        // Create a random amount of skew for each dimension
        public Spectrum()
        {
            for (int i = 0; i < Tweakables.SPECTRUM_DIMENSIONS; i++)
            {
                dimensionGaussianArray[i] = (Tweakables.PERCENT_MIN_SPECTRUM_GAUSSIAN + Utils.getDouble() * (Tweakables.PERCENT_MAX_SPECTRUM_GAUSSIAN - Tweakables.PERCENT_MIN_SPECTRUM_GAUSSIAN)) / 100;
                dimensionSkewArray[i] = (Tweakables.PERCENT_MIN_SPECTRUM_SKEW + Utils.getDouble() * (Tweakables.PERCENT_MAX_SPECTRUM_SKEW - Tweakables.PERCENT_MIN_SPECTRUM_SKEW)) / 100 / 2;
            }
        }
    }
}
