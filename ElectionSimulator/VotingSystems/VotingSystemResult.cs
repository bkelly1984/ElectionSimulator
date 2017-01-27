using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectionSimulator.Ballots;
using ElectionSimulator.People;

namespace ElectionSimulator.VotingSystems
{
    public class VotingSystemResult
    {
        public VotingSystem votingSystem { get; }
        public Dictionary<int, List<Candidate>> scoreDictionary { get; } = new Dictionary<int, List<Candidate>>();

        public VotingSystemResult(VotingSystem votingSystem)
        {
            this.votingSystem = votingSystem;
        }

        public void addCandidate(Candidate candidate, int score)
        {
            List<Candidate> candidateList;

            if (scoreDictionary.ContainsKey(score))
            {
                candidateList = scoreDictionary[score];
            }
            else
            {
                candidateList = new List<Candidate>();
                scoreDictionary[score] = candidateList;
            }

            candidateList.Add(candidate);
        }

        public List<Candidate> getWinnerList()
        {
            if (scoreDictionary.Count == 0)
            {
                return null;
            }

            return scoreDictionary.OrderByDescending(s => s.Key).First().Value;
        }

        public override string ToString()
        {
            if (scoreDictionary.Count == 0)
            {
                return votingSystem.ToString() + " {}";
            }

            string output = votingSystem.ToString() + " results { ";

            bool firstLine = true;
            foreach (KeyValuePair<int, List<Candidate>> entry in scoreDictionary.OrderByDescending(e=>e.Key))
            {
                if (!firstLine)
                {
                    output = output + ", ";
                }

                output = output + Utils.ToString(entry.Value) + ": " + entry.Key;
                firstLine = false;
            }
            output = output + " }";
            return output;
        }
    }
}
