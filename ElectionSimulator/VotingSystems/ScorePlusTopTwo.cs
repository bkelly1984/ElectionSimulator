using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectionSimulator.VotingSystems
{
    /*
    class ScorePlusTopTwo : VotingSystem
    {
        public int topScore { get; }

        public ScorePlusTopTwo(int topScore) : base("Score 0-" + topScore + " + Top Two")
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

            if (Tweakables.PRINT_SCORE_PLUS_TOP_TWO)
            {
                System.Console.WriteLine("First vote " + result.ToString());
            }

            // Prepare for the runoff
            Candidate[] runoffCandidates = new Candidate[2];

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

            // Determine the top two winners
            // A tie of two for first is fine
            if (result.candidateOrder.First().Count() == 2)
            {
                runoffCandidates[0] = result.candidateOrder.First()[0];
                runoffCandidates[1] = result.candidateOrder.First()[1];
            }
            else
            {
                runoffCandidates[0] = result.candidateOrder.First().First();
                runoffCandidates[1] = result.candidateOrder[1].First();
            }

            CondorcetVote condorcetVote = election.getCondorcetVote();

            // If second place beat first, swap 'em
            if (condorcetVote.isBetter(runoffCandidates[1], runoffCandidates[0]))
            {
                List<Candidate> firstPlaceList = result.candidateOrder[0];
                List<Candidate> secondPlaceList = result.candidateOrder[1];
                result.candidateOrder[0] = secondPlaceList;
                result.candidateOrder[1] = firstPlaceList;
            }

            // If second and first place tie, combine them
            else if (condorcetVote.isTied(runoffCandidates[1], runoffCandidates[0]))
            {
                Candidate firstPlace = result.candidateOrder[0].First();
                result.candidateOrder.RemoveAt(0);
                result.candidateOrder[0].Add(firstPlace);
            }

            if (Tweakables.PRINT_SCORE_PLUS_TOP_TWO)
            {
                System.Console.WriteLine("Second vote " + result.ToString());
            }

            return result;
        }
    }
    */
}
