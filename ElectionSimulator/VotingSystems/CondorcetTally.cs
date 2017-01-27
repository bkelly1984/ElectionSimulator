using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectionSimulator.Ballots;
using ElectionSimulator.People;

namespace ElectionSimulator.VotingSystems
{
    public class CondorcetTally
    {
        private Roster roster;
        private int[,] votes;
        private int[] scores;

        public CondorcetTally(Roster roster, List<Ballot> ballotList)
        {
            this.roster = roster;
            votes = new int[Tweakables.CANDIDATE_COUNT, Tweakables.CANDIDATE_COUNT];
            scores = new int[Tweakables.CANDIDATE_COUNT];

            foreach (Ballot ballot in ballotList)
            {
                if (ballot.ballotInstructions.ballotType != BallotType.Rank)
                {
                    throw new Exception("Attempt to Condorcet tally a non-rank ballot");
                }

                foreach (Candidate subjectCandidate in roster.candidateList)
                {
                    foreach (Candidate objectCandidate in roster.candidateList)
                    {
                        if (subjectCandidate == objectCandidate)
                        {
                            continue;
                        }

                        if (ballot.getPreference(subjectCandidate, objectCandidate) > 0)
                        {
                            votes[subjectCandidate.index, objectCandidate.index]++;
                            scores[subjectCandidate.index]++;
                            continue;
                        }
                    }
                }
            }
        }

        public int getVotes(Candidate subjectCandidate, Candidate objectCandidate)
        {
            return votes[subjectCandidate.index, objectCandidate.index];
        }

        public int getVoteDifference(Candidate subjectCandidate, Candidate objectCandidate)
        {
            return votes[subjectCandidate.index, objectCandidate.index] - votes[objectCandidate.index, subjectCandidate.index];
        }

        public int getScore(Candidate candidate)
        {
            return scores[candidate.index];
        }

        public override string ToString()
        {
            string output = "Condorcet Tally: {" + Environment.NewLine;
            for (int i = 0; i < Tweakables.CANDIDATE_COUNT; i++)
            {
                output = output + "\t{ ";
                for (int j = 0; j < Tweakables.CANDIDATE_COUNT; j++)
                {
                    if (j == 0)
                    {
                        output = output + votes[i, j];
                        continue;
                    }
                    output = output + ", " + votes[i, j];
                }
                output = output + " }" + Environment.NewLine;
            }
            output = output + "}" + Environment.NewLine;

            output = output + "Condorcet Vote: { ";
            bool firstLine = true;
            for (int i = 0; i < Tweakables.CANDIDATE_COUNT; i++)
            {
                for (int j = i + 1; j < Tweakables.CANDIDATE_COUNT; j++)
                {
                    if (!firstLine)
                    {
                        output = output + ", ";
                    }
                    output = output + roster.candidateList[i] + " > " + roster.candidateList[j] + ": " + (votes[i, j] - votes[j, i]);
                    firstLine = false;
                }
            }
            return output + "}";

        }
    }
}
