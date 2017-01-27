using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectionSimulator.VotingSystems
{
    /*
    class SRV : VotingSystem
    {
        public int topScore { get; }

        public SRV(int topScore) : base("SRV 0-" + topScore)
        {
            this.topScore = topScore;
        }

        public override ElectionResult getResult(Election election)
        {
            // Get the score vote
            ScoreVote scoreVote = election.getScoreVote(topScore);

            // Create the results
            ElectionResult result = new ElectionResult(name);
            int lastScoreCount = -1;
            foreach (CandidateScore candidateScore in scoreVote.candidateScoreList.OrderByDescending(c => c.score))
            {
                if (candidateScore.score == lastScoreCount)
                {
                    result.addTie(candidateScore.candidate);
                    continue;
                }

                result.addNext(candidateScore.candidate);
                lastScoreCount = candidateScore.score;
            }

            if (Tweakables.PRINT_RCV)
            {
                System.Console.WriteLine("First round " + result.ToString());
            }

            // A tie of more than two is unresolvable
            if (result.candidateOrder.First().Count() > 2)
            {
                return result;
            }

            // If there's a tie for second, then it is a tie with first
            if (result.candidateOrder[1] != null && result.candidateOrder[1].Count > 1)
            {
                Candidate firstCandidate = result.candidateOrder.First().First();
                result.candidateOrder.RemoveAt(0);
                result.candidateOrder.First().Add(firstCandidate);
                return result;
            }

            // Make the top two tied in the results
            if (result.candidateOrder.First().Count() == 1)
            {
                Candidate firstCandidate = result.candidateOrder.First().First();
                result.candidateOrder.RemoveAt(0);
                result.candidateOrder.First().Add(firstCandidate);
            }

            // Set first place
            List<Candidate> firstPlaceList = new List<Candidate>();
            if (scoreVote.isCandidatePreferredByMoreVoters(result.candidateOrder.First()[0], result.candidateOrder.First()[1]))
            {
                firstPlaceList.Add(result.candidateOrder.First()[0]);
                result.candidateOrder.First().RemoveAt(0);
                result.candidateOrder.Insert(0, firstPlaceList);
            }
            else if (scoreVote.isCandidatePreferredByMoreVoters(result.candidateOrder.First()[1], result.candidateOrder.First()[0]))
            {
                firstPlaceList.Add(result.candidateOrder.First()[1]);
                result.candidateOrder.First().RemoveAt(1);
                result.candidateOrder.Insert(0, firstPlaceList);
            }

            if (Tweakables.PRINT_RCV)
            {
                System.Console.WriteLine("Second round " + result.ToString());
            }

            return result;
        }
    }
    */
}
