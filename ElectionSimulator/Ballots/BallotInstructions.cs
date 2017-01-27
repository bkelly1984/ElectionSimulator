using ElectionSimulator.People;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectionSimulator.Ballots
{
    public class BallotInstructions
    {
        public BallotType ballotType { get; }

        // Rank ballot instructions
        public int maximumPreferredCandidates { get; private set; }
        public int maximumDispreferredCandidates { get; private set; }

        // Score ballot instructions
        public int maximumScore { get; private set; }
        public int maximumCandidatesToScore { get; private set; }

        public BallotInstructions(BallotType ballotType)
        {
            this.ballotType = ballotType;
        }

        public void setRankInstructions(int maximumPreferredCandidates, int maximumDispreferredCandidates)
        {
            this.maximumPreferredCandidates = maximumPreferredCandidates;
            this.maximumDispreferredCandidates = maximumDispreferredCandidates;
        }

        public void setScoreInstructions(int maximumScore, int maximumCandidatesToScore)
        {
            this.maximumScore = maximumScore;
            this.maximumCandidatesToScore = maximumCandidatesToScore;
        }
    }
}
