using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectionSimulator.Ballots;
using ElectionSimulator.People;

namespace ElectionSimulator.VotingSystems
{
    class FPTP : VotingSystem
    {
        public FPTP(BallotInstructions ballotInstructions) : base("FPTP", ballotInstructions)
        {
        }

        public override VotingSystemResult getResult(Roster roster, List<Ballot> ballotList)
        {
            Dictionary<Candidate, int> voteCountDictionary = new Dictionary<Candidate, int>();

            foreach (Candidate candidate in roster.candidateList)
            {
                voteCountDictionary[candidate] = 0;
            }

            foreach (Ballot ballot in ballotList)
            {
                Candidate candidate = null;

                if (ballot.ballotInstructions.ballotType == BallotType.Score && ballot.candidateScoreList.Count > 0)
                {
                    candidate = ballot.candidateScoreList.OrderByDescending(s => s.score).First().candidate;
                }
                else if (ballot.ballotInstructions.ballotType == BallotType.Rank && ballot.preferredCandidateList.Count > 0)
                {
                    candidate = ballot.preferredCandidateList.First();
                }

                voteCountDictionary[candidate]++;

                if (Tweakables.PRINT_FPTP)
                {
                    System.Console.WriteLine(ballot.voter.ToString() + ": " + candidate.ToString());
                }
            }

            // Create the results
            VotingSystemResult result = new VotingSystemResult(this);
            foreach (Candidate candidate in roster.candidateList)
            {
                result.addCandidate(candidate, voteCountDictionary[candidate]);
            }

            if (Tweakables.PRINT_RESULTS)
            {
                System.Console.WriteLine(result.ToString());
            }

            return result;
        }
    }
}
