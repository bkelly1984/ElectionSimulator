using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectionSimulator.Ballots;

namespace ElectionSimulator.People
{
    // A perspective is how a voter converts the multi-dimensional political spectrum into a 
    // one-dimensional preference
    public abstract class Viewpoint
    {
        public Viewpoint()
        {
        }

        public abstract void setRoster(Roster roster);
        public abstract Ballot createBallot(BallotInstructions ballotInstructions);


        /*
        public List<CandidateRating> ratingList { get; } = new List<CandidateRating>();
        private CandidateRating[] ratingCache = new CandidateRating[Tweakables.CANDIDATE_COUNT];

        protected void fillInCandidateRankings()
        {
            // Fill in the rest of the CandidateRating fields
            int rank = 1;
            IEnumerable<CandidateRating> orderedRatings = ratingList.OrderBy(p => p.distance);
            double minimumDistance = orderedRatings.First().distance;
            double maximumDistance = orderedRatings.Last().distance;

            foreach (CandidateRating rating in orderedRatings)
            {
                rating.completeRatings(minimumDistance, maximumDistance, rank++);
                ratingCache[rating.candidate.index] = rating;
            }
        }

        public IEnumerable<CandidateRating> getRatingsByIndex()
        {
            return ratingList.OrderBy(r => r.candidate.index);
        }

        public IEnumerable<CandidateRating> getRatingsByRank()
        {
            return ratingList.OrderBy(r => r.rank);
        }

        public IEnumerable<CandidateRating> getRatingsByScore()
        {
            return ratingList.OrderBy(r => r.score);
        }

        public bool isBetter(Candidate subjectCandidate, Candidate objectCandidate)
        {
            return ratingCache[subjectCandidate.index].score > ratingCache[objectCandidate.index].score;
        }

        public bool isWorse(Candidate subjectCandidate, Candidate objectCandidate)
        {
            return ratingCache[subjectCandidate.index].score < ratingCache[objectCandidate.index].score;
        }
        */
    }
}
