using ElectionSimulator.People;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectionSimulator.Ballots
{
    public class CandidateScore
    {
        public Candidate candidate { get; }
        public int score { get; private set; } = 0;

        public CandidateScore(Candidate candidate, int score)
        {
            this.candidate = candidate;
            this.score = score;
        }

        public CandidateScore(Candidate candidate) : this(candidate, 0)
        {
        }

        public int addScore(int score)
        {
            this.score = this.score + score;
            return this.score;
        }

        public override string ToString()
        {
        return candidate.ToString() + ": " + score;
        }
    }
}