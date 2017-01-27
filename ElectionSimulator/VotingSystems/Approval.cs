using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectionSimulator.Ballots;
using ElectionSimulator.People;

namespace ElectionSimulator.VotingSystems
{
    class Approval : VotingSystem
    {
        public Approval(BallotInstructions ballotInstructions) : base("Approval", ballotInstructions)
        {
        }

        public override VotingSystemResult getResult(Roster roster, List<Ballot> ballotList)
        {
            Dictionary<Candidate, int> candidateScores = new Dictionary<Candidate, int>();

            foreach (Candidate candidate in roster.candidateList)
            {
                candidateScores[candidate] = 0;
            }

            // Count the ballots
            foreach (Ballot ballot in ballotList)
            {
                if (ballot.ballotInstructions != ballotInstructions)
                {
                    throw new Exception("Approval voting system asked to count a ballot that did not use the correct instructions");
                }

                foreach (CandidateScore candidateScore in ballot.candidateScoreList)
                {
                    candidateScores[candidateScore.candidate] += candidateScore.score;
                }
                break;
            }

            // Convert the final tally to a result object
            VotingSystemResult result = new VotingSystemResult(this);
            foreach (Candidate candidate in roster.candidateList)
            {
                result.addCandidate(candidate, candidateScores[candidate]);
            }

            return result;
        }
    }
}
