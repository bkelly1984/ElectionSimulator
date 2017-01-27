using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectionSimulator.Ballots;
using ElectionSimulator.People;

namespace ElectionSimulator.VotingSystems
{
    class IRV : VotingSystem
    {
        public IRV(BallotInstructions ballotInstructions) : base("IRV", ballotInstructions)
        {
        }

        public override VotingSystemResult getResult(Roster roster, List<Ballot> ballotList)
        {
            Dictionary<Candidate, List<Ballot>> candidateSupportDictionary = new Dictionary<Candidate, List<Ballot>>();
            List<Candidate> remainingCandidates = roster.candidateList.ToList();
            List<List<Candidate>> lostCandidates = new List<List<Candidate>>();

            // Create the count for each candidate
            foreach (Candidate candidate in roster.candidateList)
            {
                candidateSupportDictionary[candidate] = new List<Ballot>();
            }

            // Have every voter vote
            foreach (Ballot ballot in ballotList)
            {
                countBallot(ballot, candidateSupportDictionary, remainingCandidates);
            }

            // While we still have multiple candidates
            while (remainingCandidates.Count > 0) {

                // Find the candidate with the fewest supporters
                List<Candidate> losingCandidates = new List<Candidate>();
                //KeyValuePair<Candidate, List<Ballot>> bottomCandidate = candidateSupportDictionary.OrderBy(c => c.Value.Count).First();

                int? ballotCount = null;
                foreach (KeyValuePair<Candidate, List<Ballot>> bottomCandidate in candidateSupportDictionary.OrderBy(c => c.Value.Count))
                {
                    if (ballotCount == null)
                    {
                        ballotCount = bottomCandidate.Value.Count;
                        losingCandidates.Add(bottomCandidate.Key);
                        continue;
                    }

                    if (ballotCount != bottomCandidate.Value.Count)
                    {
                        break;
                    }

                    losingCandidates.Add(bottomCandidate.Key);
                }

                // Remove the losing candidates
                foreach (Candidate losingCandidate in losingCandidates)
                {
                    remainingCandidates.Remove(losingCandidate);
                }

                // Redistribute losing candidate votes
                foreach (Candidate losingCandidate in losingCandidates)
                {
//                    System.Console.WriteLine("Candidate {0} has lost", losingCandidate.candidate.getIndex());
                    foreach (Ballot ballot in candidateSupportDictionary[losingCandidate])
                    {
                        countBallot(ballot, candidateSupportDictionary, remainingCandidates);
                    }
                }

                // Count these candidates as lost
                lostCandidates.Add(losingCandidates);
            }

            // Run backwards through the list of losers to find the winner
            VotingSystemResult result = new VotingSystemResult(this);
            lostCandidates.Reverse();
            foreach (List<Candidate> lostCandidateList in lostCandidates)
            {
                foreach (Candidate lostCandidate in lostCandidateList)
                {
                    result.addCandidate(lostCandidate, candidateSupportDictionary[lostCandidate].Count);
                }
            }

            return result;
        }

        public void countBallot(Ballot ballot, Dictionary<Candidate, List<Ballot>> candidateSupportDictionary, List<Candidate> remainingCandidates)
        {
            foreach (Candidate candidate in ballot.preferredCandidateList)
            {
                if (remainingCandidates.Contains(candidate))
                {
                    candidateSupportDictionary[candidate].Add(ballot);
                    return;
                }
            }
        }
    }
}
