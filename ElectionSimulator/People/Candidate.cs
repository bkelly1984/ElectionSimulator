using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectionSimulator.People
{
    public class Candidate
    {
        public int index { get; }
        public Voter voter { get; }

        public Candidate(int index, Voter voter)
        {
            this.index = index;
            this.voter = voter;
        }

        public static List<Candidate> createCandidateList(List<Voter> voterList, int count)
        {
            if (voterList.Count < count)
            {
                throw new Exception("Request to create more candidates than there are voters");
            }

            List<Candidate> candidateList = new List<Candidate>();
            List<Voter> remainingVoters = voterList.ToList();

            for (int i = 0; i < count; i++)
            {
                int randomIndex = Convert.ToInt32(Math.Floor(Utils.getDouble() * remainingVoters.Count));
                candidateList.Add(new People.Candidate(i, remainingVoters[randomIndex]));
                remainingVoters.RemoveAt(randomIndex);
            }

            return candidateList;
        }

        public override string ToString()
        {
            return "C" + index + " (" + voter.ToString() + ")";
        }
    }
}
