using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectionSimulator.People;

namespace ElectionSimulator
{
    public class CandidateRating
    {
        public Candidate candidate { get; }
        public double distance { get; }
        public double score { get; private set; }
        public int rank { get; private set; }

        public CandidateRating(Candidate candidate, double distance)
        {
            this.candidate = candidate;
            this.distance = distance;
        }

        public void completeRatings(double minimumDistance, double maximumDistance, int rank)
        {
            score = 1 - (distance - minimumDistance) / (maximumDistance - minimumDistance);
            this.rank = rank;
        }

        override public string ToString()
        {
            return String.Format("Candidate {0} [{1}, {2}, {3} of {4}]", candidate.index, String.Format("{0:0.0000}", distance), String.Format("{0:0.0000}", score), rank, Tweakables.CANDIDATE_COUNT);
        }
    }
}
