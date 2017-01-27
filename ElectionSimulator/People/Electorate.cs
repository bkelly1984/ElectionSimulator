using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectionSimulator.People
{
    public class Electorate
    {
        public Voter[] voterArray { get; } = new Voter[Tweakables.VOTER_COUNT];

        public Electorate(Spectrum spectrum)
        {
            for (int i = 0; i < Tweakables.VOTER_COUNT; i++)
            {
                voterArray[i] = new Voter(i, spectrum);
            }
        }
    }
}
