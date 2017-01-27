using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectionSimulator.Ballots;
using ElectionSimulator.People;

namespace ElectionSimulator.VotingSystems
{
    class Borda : VotingSystem
    {
        public Borda(BallotInstructions ballotInstructions) : base("Borda", ballotInstructions)
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
                    throw new Exception("Borda count voting system asked to count a ballot that did not use the correct instructions");
                }

                int score = roster.candidateList.Count;
                if (ballot.ballotInstructions.ballotType == BallotType.Score && ballot.candidateScoreList.Count > 0)
                {
                    foreach (CandidateScore candidateScore in ballot.candidateScoreList.OrderByDescending(c => c.score))
                    {
                        candidateScores[candidateScore.candidate] += score;
                        score--;
                    }
                }
                else if (ballot.ballotInstructions.ballotType == BallotType.Rank && ballot.preferredCandidateList.Count > 0)
                {
                    foreach (Candidate candidate in ballot.preferredCandidateList)
                    {
                        candidateScores[candidate] += score;
                        score--;
                    }

                    score -= (roster.candidateList.Count - ballot.preferredCandidateList.Count - ballot.dispreferredCandidateList.Count) / 2;

                    foreach (Candidate candidate in roster.candidateList.Except(ballot.preferredCandidateList).Except(ballot.dispreferredCandidateList))
                    {
                        candidateScores[candidate] += score;
                    }

                    score = 1;
                    foreach (Candidate candidate in ballot.dispreferredCandidateList)
                    {
                        candidateScores[candidate] += score;
                        score++;
                    }

                }

                foreach (CandidateScore candidateScore in ballot.candidateScoreList.OrderByDescending(c => c.score))
                {
                    candidateScores[candidateScore.candidate] += score;
                    score--;
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
