using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectionSimulator.Ballots;

namespace ElectionSimulator.People
{
    public class DistanceViewpoint : LinearViewpoint
    {
        private Voter voter;

        public DistanceViewpoint(Voter voter) : base()
        {
            this.voter = voter;
        }

        // For the distance viewpoint, a candidate's rating is simply his/her distance in political spectrum space
        protected override double getCandidateRating(Candidate candidate)
        {
            return voter.position.getDistance(candidate.voter.position);
        }
    }
}
