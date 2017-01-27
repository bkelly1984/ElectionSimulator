using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectionSimulator.Ballots;
using ElectionSimulator.People;

namespace ElectionSimulator.VotingSystems
{
    class Condorcet : VotingSystem
    {
        public Condorcet(BallotInstructions ballotInstructions) : base("Condorcet", ballotInstructions)
        {
        }

        public override VotingSystemResult getResult(Roster roster, List<Ballot> ballotList)
        {
            CondorcetTally condorcetTally = new CondorcetTally(roster, ballotList);

            if (Tweakables.PRINT_CONDORCET)
            {
                System.Console.WriteLine(condorcetTally.ToString());
            }

            List<List<Candidate>> candidateLoopList = findCandidateLoops(roster.candidateList, condorcetTally);
            candidateLoopList = getOrderedCandidateLoopList(candidateLoopList, condorcetTally);

            VotingSystemResult result = new VotingSystemResult(this);
            foreach (List<Candidate> candidateLoop in candidateLoopList)
            {
                int score = 0;

                foreach (Candidate candidate in candidateLoop)
                {
                    score += condorcetTally.getScore(candidate);
                }

                score = score / candidateLoop.Count;

                foreach (Candidate candidate in candidateLoop)
                {
                    result.addCandidate(candidate, score);
                }
           }

            if (Tweakables.PRINT_RESULTS)
            {
                System.Console.WriteLine(result.ToString());
            }

            return result;
        }

        private List<List<Candidate>> findCandidateLoops(List<Candidate> candidateList, CondorcetTally condorcetTally)
        {
            List<List<Candidate>> candidateLoopList = new List<List<Candidate>>();
            List<Candidate> remainingCandidateList = candidateList.ToList();

            while (remainingCandidateList.Count > 0)
            {
                Candidate candidate = remainingCandidateList.First();
                remainingCandidateList.Remove(candidate);
                List<Candidate> candidateLoop = findCandidateLoop(candidate, candidate, remainingCandidateList, new List<Candidate>(), condorcetTally);

                // No loop so consider this candidate to be in his/her own loop
                if (candidateLoop == null)
                {
                    candidateLoop = new List<Candidate>();
                    candidateLoop.Add(candidate);
                }

                if (Tweakables.PRINT_CONDORCET)
                {
                    System.Console.WriteLine("Found loop: " + Utils.ToString(candidateLoop));
                }

                candidateLoopList.Add(candidateLoop);
                remainingCandidateList = remainingCandidateList.Except(candidateLoop).ToList();
            }

            return candidateLoopList;
        }

        private List<Candidate> findCandidateLoop(Candidate currentCandidate, Candidate originalCandidate, List<Candidate> remainingCandidateList, List<Candidate> exploredCandidateList, CondorcetTally condorcetTally)
        {
            //System.Console.WriteLine("Finding loop from " + currentCandidate.ToString() + " to candidate " + originalCandidate.ToString());
            List<Candidate> currentEquivelantCandidateList = new List<Candidate>();
            currentEquivelantCandidateList.Add(currentCandidate);

            // Any candidated tied with the current candidate is functionally the same for any loop
            foreach (Candidate remainingCandidate in remainingCandidateList)
            {
                if (exploredCandidateList.Contains(remainingCandidate))
                {
                    continue;
                }

                if (condorcetTally.getVoteDifference(currentCandidate, remainingCandidate) == 0)
                {
                    //System.Console.WriteLine(remainingCandidate.ToString() + " is tied with our current one");
                    currentEquivelantCandidateList.Add(remainingCandidate);
                }
            }

            List<Candidate> defeatedCandidateList = new List<Candidate>();

            // Find all the non-explored, remaining candidates the passed canidate beats
            foreach (Candidate equivelantCandidate in currentEquivelantCandidateList)
            {
                foreach (Candidate remainingCandidate in remainingCandidateList)
                {
                    // If we have a candidate that has been explored or we have already beaten, continue
                    if (exploredCandidateList.Contains(remainingCandidate) || defeatedCandidateList.Contains(remainingCandidate))
                    {
                        continue;
                    }

                    if (condorcetTally.getVoteDifference(equivelantCandidate, remainingCandidate) > 0)
                    {
                        // See if we beat our original candidate.  If so, return our equivelant candidate list
                        return currentEquivelantCandidateList;
                    }

                    defeatedCandidateList.Add(remainingCandidate);
                }
            }

            if (defeatedCandidateList.Count == 0)
            {
                return null;
            }

            // We will look at all the found candidates, so consider them explored
            exploredCandidateList.AddRange(defeatedCandidateList);

            // Explore each candidate 
            foreach (Candidate beatenCandidate in defeatedCandidateList)
            {
                List<Candidate> loopList = findCandidateLoop(beatenCandidate, originalCandidate, remainingCandidateList, exploredCandidateList, condorcetTally);

                if (loopList != null)
                {
                    loopList.AddRange(currentEquivelantCandidateList);
                    return loopList;
                }
            }

            return null;
        }

        private List<List<Candidate>> getOrderedCandidateLoopList(List<List<Candidate>> candidateLoopList, CondorcetTally condorcetTally)
        {
            List<List<Candidate>> orderedCandidateLoopList = new List<List<Candidate>>();
            List<List<Candidate>> remainingCandidateLoopList = candidateLoopList.ToList();

            if (Tweakables.PRINT_CONDORCET)
            {
                System.Console.WriteLine("Ordering loops");
            }

            while (remainingCandidateLoopList.Count > 0)
            {
                List<Candidate> candidateLoop = findTopCandidateLoop(remainingCandidateLoopList, condorcetTally);
                orderedCandidateLoopList.Add(candidateLoop);
                remainingCandidateLoopList.Remove(candidateLoop);

                if (Tweakables.PRINT_CONDORCET)
                {
                    System.Console.WriteLine("Next loop: " + Utils.ToString(candidateLoop));
                }
            }

            return orderedCandidateLoopList;
        }

        private List<Candidate> findTopCandidateLoop(List<List<Candidate>> candidateLoopList, CondorcetTally condorcetTally)
        {
            foreach (List<Candidate> subjectCandidateLoop in candidateLoopList)
            {
                Candidate subjectCandidate = subjectCandidateLoop.First();
                bool subjectLoopBeaten = false;

                foreach (List<Candidate> objectCandidateLoop in candidateLoopList)
                {
                    if (subjectCandidateLoop == objectCandidateLoop)
                    {
                        continue;
                    }

                    Candidate objectCandidate = subjectCandidateLoop.First();

                    if (condorcetTally.getVoteDifference(subjectCandidate, objectCandidate) < 0)
                    {
                        subjectLoopBeaten = true;
                        break;
                    }
                }

                if (!subjectLoopBeaten)
                {
                    return subjectCandidateLoop;
                }
            }

            throw new Exception("Unable to find a top Condorcet loop");
        }
    }
}
