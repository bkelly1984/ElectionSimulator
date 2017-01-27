using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectionSimulator.Ballots;

namespace ElectionSimulator.People
{
    // This viewpoint assumes all candidates can be given a rating and their position can be determined by the relative ratings
    public abstract class LinearViewpoint : Viewpoint
    {
        private List<CandidateRating> candidateRatingList = new List<CandidateRating>();

        public LinearViewpoint() : base()
        {
        }

        public override void setRoster(Roster roster)
        {
            candidateRatingList.Clear();
            double? minimumRating = null;
            double? maximumRating = null;

            foreach (Candidate candidate in roster.candidateList)
            {
                double rating = getCandidateRating(candidate);
                candidateRatingList.Add(new CandidateRating(candidate, rating));

                if (minimumRating == null || rating < minimumRating)
                {
                    minimumRating = rating;
                }

                if (maximumRating == null || rating > maximumRating)
                {
                    maximumRating = rating;
                }
            }

            foreach (CandidateRating candidateRating in candidateRatingList)
            {
                candidateRating.normalizeRating((double)minimumRating, (double)maximumRating);
            }
        }

        public override Ballot createBallot(BallotInstructions ballotInstructions)
        {
            if (ballotInstructions.ballotType == BallotType.Rank)
            {
                return createRankBallot(candidateRatingList, ballotInstructions);
            }

            if (ballotInstructions.ballotType == BallotType.Score)
            {
                return createScoreBallot(candidateRatingList, ballotInstructions);
            }

            throw new Exception("Encountered unhandled ballot type");
        }

        private Ballot createRankBallot(List<CandidateRating> candidateRatingList, BallotInstructions ballotInstructions)
        {
            Ballot ballot = new Ballot(ballotInstructions);

            int i = 0;
            foreach (CandidateRating candidateRating in candidateRatingList.OrderBy(r => r.rating))
            {
                // End the loop if we have already entered the maximum number allowed to list
                if (i >= ballotInstructions.maximumPreferredCandidates)
                {
                    break;
                }

                ballot.preferredCandidateList.Add(candidateRating.candidate);
                i++;
            }

            i = 0;
            foreach (CandidateRating candidateRating in candidateRatingList.OrderByDescending(r => r.rating))
            {
                // End the loop if we have already listed this candidate as preferred
                // Or we are not allowed to list any more
                if (ballot.preferredCandidateList.Contains(candidateRating.candidate) ||
                        (i >= ballotInstructions.maximumDispreferredCandidates))
                {
                    break;
                }

                ballot.dispreferredCandidateList.Add(candidateRating.candidate);
                i++;
            }

            return ballot;
        }

        private Ballot createScoreBallot(List<CandidateRating> candidateRatingList, BallotInstructions ballotInstructions)
        {
            Ballot ballot = new Ballot(ballotInstructions);

            int i = 0;
            foreach (CandidateRating candidateRating in candidateRatingList.OrderBy(r => r.rating))
            {
                // End the loop if we have already entered the maximum number allowed to list
                if (i >= ballotInstructions.maximumCandidatesToScore)
                {
                    break;
                }

                // A round nearest in a 0-10 scale would round 0 to .05 down to 0 and .05 to .15 to 1
                // This means twice the range rounds to 1 as rounds to 0
                // What we want is 0 to .0909 rounds to 0 and .0909 to .1818 rounds to 1
                int score = Convert.ToInt32(candidateRating.rating * (ballotInstructions.maximumScore + 1) - .5);

                if (score < 0)
                {
                    score = 0;
                }

                if (score > ballotInstructions.maximumScore)
                {
                    score = ballotInstructions.maximumScore;
                }

                ballot.candidateScoreList.Add(new CandidateScore(candidateRating.candidate, score));
                i++;
            }

            return ballot;
        }

        protected abstract double getCandidateRating(Candidate candidate);

        public override string ToString()
        {
            string output = this.GetType().Name + " { ";
            bool firstLine = true;
            foreach (CandidateRating rating in candidateRatingList)
            {
                if (!firstLine)
                {
                    output = output + ", ";
                }
                output = output + rating.ToString();
                firstLine = false;
            }
            output = output + " }";
            return output;
        }
    }


    public class CandidateRating
    {
        public Candidate candidate { get; }
        public double rating { get; private set; }

        public CandidateRating(Candidate candidate, double rating)
        {
            this.candidate = candidate;
            this.rating = rating;
        }

        public void normalizeRating(double minimumScore, double maximumScore)
        {
            rating = (rating - minimumScore) / (maximumScore - minimumScore);
        }

        public override string ToString()
        {
            return candidate.ToString() + ": " + rating.ToString("0.0000");
        }
    }
}
