using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectionSimulator.VotingSystems
{
    /*
    class ThreeStepVoting : VotingSystem
    {
        public int topScore { get; }

        public ThreeStepVoting(int topScore) : base("3-2-1 Voting 0-" + topScore)
        {
            this.topScore = topScore;
        }

        public override ElectionResult getResult(Election election)
        {
            // Get the vote
            ScoreVote scoreVote = election.getScoreVote(topScore);

            VoteCount[] voteCountArray = new VoteCount[Tweakables.CANDIDATE_COUNT];

            foreach (Candidate candidate in election.roster.candidateArray)
            {
                voteCountArray[candidate.index] = new VoteCount(candidate);
            }

            // Give votes to everyone who got the top score
            foreach (ScoreBallot scoreBallot in scoreVote.ballotList)
            {
                foreach (CandidateScore candidateScore in scoreBallot.candidateScoreList)
                {
                    if (candidateScore.score == topScore)
                    {
                        voteCountArray[candidateScore.candidate.index].votes++;
                    }
                }
            }

            // Create the results
            ElectionResult result = new ElectionResult(name);
            int lastVoteCount = -1;
            foreach (VoteCount voteCount in voteCountArray.OrderByDescending(c=>c.votes))
            {
                if (voteCount.votes == lastVoteCount)
                {
                    result.addTie(voteCount.candidate);
                    continue;
                }

                result.addNext(voteCount.candidate);
                lastVoteCount = voteCount.votes;
            }

            List<Candidate> semifinalList = new List<Candidate>();
            foreach (List<Candidate> candidateList in result.candidateOrder.ToList())
            {
                result.candidateOrder.Remove(candidateList);
                semifinalList.AddRange(candidateList);

                // When we have at least 3 semifinalists
                if (semifinalList.Count >= 3)
                {
                    // Put our semifinalists back into the results as tied
                    result.candidateOrder.Insert(0, semifinalList);

                    if (Tweakables.PRINT_THREE_STEP_VOTING)
                    {
                        System.Console.WriteLine("First round " + result.ToString());
                    }

                    // If we get more than three semifinalists, then all of them tie
                    if (semifinalList.Count > 3)
                    {
                        return result;
                    }

                    break;
                }
            }

                // Reinitialize the vote count for the semifinalists
            List<VoteCount> semifinalVoteCountList = new List<VoteCount>();
            foreach (Candidate candidate in semifinalList)
            {
                voteCountArray[candidate.index].votes = 0;
                semifinalVoteCountList.Add(voteCountArray[candidate.index]);
            }

            // Give votes to everyone who got a score of at least 2 below top
            foreach (ScoreBallot scoreBallot in scoreVote.ballotList)
            {
                foreach (CandidateScore candidateScore in scoreBallot.candidateScoreList)
                {
                    if (semifinalList.Contains(candidateScore.candidate) && candidateScore.score < topScore - 1)
                    {
                        voteCountArray[candidateScore.candidate.index].votes++;
                    }
                }
            }

            // Find the rejected candidate(s)
            int maximumRejections = 0;
            List<Candidate> rejectedCandidateList = new List<Candidate>();
            foreach (VoteCount voteCount in semifinalVoteCountList)
            {
                if (voteCount.votes > maximumRejections)
                {
                    rejectedCandidateList.Clear();
                    rejectedCandidateList.Add(voteCount.candidate);
                    maximumRejections = voteCount.votes;
                    continue;
                }

                else if (voteCount.votes == maximumRejections)
                {
                    rejectedCandidateList.Add(voteCount.candidate);
                }
            }

            // If all are equally rejected, we have another tie
            if (rejectedCandidateList.Count == 3)
            {
                if (Tweakables.PRINT_THREE_STEP_VOTING)
                {
                    System.Console.WriteLine("Second round " + result.ToString());
                }

                return result;
            }

            // Remove rejected candidates and add them behind the first position
            foreach (Candidate candidate in rejectedCandidateList)
            {
                result.candidateOrder.First().Remove(candidate);
            }
            result.candidateOrder.Insert(1, rejectedCandidateList);

            if (Tweakables.PRINT_THREE_STEP_VOTING)
            {
                System.Console.WriteLine("Second round " + result.ToString());
            }

            // If two are rejected, we have a winner
            if (rejectedCandidateList.Count == 2)
            {
                return result;
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

            // Leave a tie as the results are already set for thast

            if (Tweakables.PRINT_THREE_STEP_VOTING)
            {
                System.Console.WriteLine("Third round " + result.ToString());
            }

            return result;
        }
    }
    */
}
