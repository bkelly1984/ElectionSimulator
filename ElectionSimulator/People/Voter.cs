using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectionSimulator.Ballots;

namespace ElectionSimulator.People
{
    public class Voter
    {
        public int index { get; }
        public SpectrumPosition position { get; }
        public Viewpoint viewpoint { get; private set; }

        public Voter(int index, Spectrum spectrum)
        {
            this.index = index;
            viewpoint = new DistanceViewpoint(this);
            position = new SpectrumPosition(spectrum);

            if (Tweakables.PRINT_POSITION)
            {
                System.Console.WriteLine(ToString() + " " + position.ToString());
            }
        }

        public void setRoster(Roster roster)
        {
            viewpoint.setRoster(roster);

            if (Tweakables.PRINT_VIEWPOINT)
            {
                System.Console.WriteLine(ToString() + " " + viewpoint.ToString());
            }
        }

        public Ballot createBallot(BallotInstructions instructions)
        {
            Ballot ballot = viewpoint.createBallot(instructions);
            ballot.voter = this;

            if (Tweakables.PRINT_BALLOTS)
            {
                System.Console.WriteLine(ToString() + " " + ballot.ToString());
            }

            return ballot;
        }

        public static List<Voter> createVoterList(int count, Spectrum spectrum)
        {
            List<Voter> voterList = new List<Voter>();

            for (int i = 0; i < count; i++)
            {
                voterList.Add(new Voter(i, spectrum));
            }

            return voterList;
        }

        public override string ToString()
        {
            return "V" + index;
        }
    }
}
