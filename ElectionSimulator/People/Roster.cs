using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectionSimulator.People
{
    public class Roster
    {
        public List<Candidate> candidateList;

        public Candidate highestDeviationCandidate { get; private set; } = null;
        public Candidate lowestDeviationCandidate { get; private set; } = null;

        // Instance Constructor.
        public Roster(List<Voter> voterList)
        {
            candidateList = Candidate.createCandidateList(voterList, Tweakables.CANDIDATE_COUNT);

            foreach (Candidate candidate in candidateList)
            {
                if (highestDeviationCandidate == null || highestDeviationCandidate.voter.position.deviation < candidate.voter.position.deviation)
                {
                    highestDeviationCandidate = candidate;
                }

                if (lowestDeviationCandidate == null || lowestDeviationCandidate.voter.position.deviation > candidate.voter.position.deviation)
                {
                    lowestDeviationCandidate = candidate;
                }
            }

            if (Tweakables.PRINT_ROSTER)
            {
                System.Console.WriteLine(ToString());
            }
        }

        public override string ToString()
        {
            string output = "Roster { ";
            bool firstLine = true;
            foreach (Candidate candidate in candidateList)
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