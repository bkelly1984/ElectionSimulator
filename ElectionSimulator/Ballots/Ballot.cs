using ElectionSimulator.People;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectionSimulator.Ballots
{

    public class Ballot
    {
        public Voter voter;
        public BallotInstructions ballotInstructions { get; }

        // Rank ballot
        public List<Candidate> preferredCandidateList { get; } = new List<Candidate>();
        public List<Candidate> dispreferredCandidateList { get; } = new List<Candidate>();

        // Score ballot
        public List<CandidateScore> candidateScoreList { get; } = new List<CandidateScore>();
        public CandidateScore[] candidateScoreLookup { get; } = new CandidateScore[Tweakables.CANDIDATE_COUNT];

        public Ballot(BallotInstructions ballotInstructions)
        {
            this.ballotInstructions = ballotInstructions;
        }

        public int getPreference(Candidate subjectCandidate, Candidate objectCandidate)
        {
            if (ballotInstructions.ballotType == BallotType.Rank)
            {
                return getRankPreference(subjectCandidate, objectCandidate);
            }

            if (ballotInstructions.ballotType == BallotType.Rank)
            {
                return getScorePreference(subjectCandidate, objectCandidate);
            }

            throw new Exception("Request to get preference from ballot of unknown type");
        }

        // Returns 1 if prefferred, -1 if dispreferred, and 0 if tied
        public int getRankPreference(Candidate subjectCandidate, Candidate objectCandidate)
        {
            if (preferredCandidateList.Contains(subjectCandidate))
            {
                if (preferredCandidateList.Contains(objectCandidate))
                {
                    // First in the list is the winning candidate
                    foreach (Candidate candidate in preferredCandidateList)
                    {
                        if (candidate == subjectCandidate)
                        {
                            return 1;
                        }

                        if (candidate == objectCandidate)
                        {
                            return -1;
                        }
                    }

                    throw new Exception("Somehow got to the bottom of a list without finding a winning candidate");
                }

                return 1;
            }

            if (dispreferredCandidateList.Contains(subjectCandidate))
            {
                if (dispreferredCandidateList.Contains(objectCandidate))
                {
                    // First in the list is the losing candidate
                    foreach (Candidate candidate in dispreferredCandidateList)
                    {
                        if (candidate == subjectCandidate)
                        {
                            return -1;
                        }

                        if (candidate == objectCandidate)
                        {
                            return 1;
                        }
                    }

                    throw new Exception("Somehow got to the bottom of a list without finding a losing candidate");
                }

                return -1;
            }

            // Subject candidate is in neither list
            if (preferredCandidateList.Contains(objectCandidate))
            {
                return -1;
            }

            // Subject candidate is in neither list
            if (dispreferredCandidateList.Contains(objectCandidate))
            {
                return 1;
            }

            return 0;
        }

        // Returns the number of votes for the subject candidate over the object candidate
        public int getScorePreference(Candidate subjectCandidate, Candidate objectCandidate)
        {
            return candidateScoreLookup[subjectCandidate.index].score - candidateScoreLookup[objectCandidate.index].score;
        }

        public override string ToString()
        {
            string output = "";

            if (ballotInstructions.ballotType == BallotType.Rank)
            {
                output = "rank ballot " + RankToString();
            }

            if (ballotInstructions.ballotType == BallotType.Score)
            {
                output = "score ballot " + ScoreToString();
            }

            return output;
        }

        public string RankToString()
        {
            string output = "{ ";
            bool firstCandidate = true;

            foreach (Candidate candidate in preferredCandidateList)
            {
                if (!firstCandidate)
                {
                    output = output + " > ";
                }

                output = output + candidate.ToString();
                firstCandidate = false;
            }

            if (!firstCandidate)
            {
                output = output + " > ";
            }

            output = output + "{ others }";

            List<Candidate> reversedList = dispreferredCandidateList.ToList();
            reversedList.Reverse();
            foreach (Candidate candidate in reversedList)
            {
                if (!firstCandidate)
                {
                    output = output + " > ";
                }

                output = output + candidate.ToString();
                firstCandidate = false;
            }

            output = output + " }";
            return output;
        }

        public string ScoreToString()
        {
            string output = "{ ";
            bool firstCandidate = true;

            foreach (CandidateScore candidateScore in candidateScoreList)
            {
                if (!firstCandidate)
                {
                    output = output + ", ";
                }

                output = output + candidateScore.ToString();
                firstCandidate = false;
            }

            output = output + " }";
            return output;
        }

    }
}

/*
 *             foreach (CandidateRating rating in voter.perspective.ratingList)
            {
                // Commented out.  Scores are not this easy
                //int score = Convert.ToInt32(rating.score * topScore);

                // A round nearest in a 0-10 scale would round 0 to .05 down to 0 and .05 to .15 to 1
                // This means twice the range rounds to 1 as rounds to 0
                // What we want is 0 to .0909 rounds to 0 and .0909 to .1818 rounds to 1
                int score = Convert.ToInt32(rating.score * (topScore + 1) - .5);

                if (score < 0)
                {
                    score = 0;
                }

                if (score > topScore)
                {
                    score = topScore;
                }

                CandidateScore candidateScore = new CandidateScore(rating.candidate, score);
                candidateScoreList.Add(candidateScore);
                candidateScoreLookup[rating.candidate.index] = candidateScore;
            }
*/
