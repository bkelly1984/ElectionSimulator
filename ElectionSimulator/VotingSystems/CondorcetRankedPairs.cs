using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectionSimulator.Ballots;
using ElectionSimulator.People;

namespace ElectionSimulator.VotingSystems
{
    /*
    class CondorcetRankedPairs : VotingSystem
    {
        public CondorcetRankedPairs(BallotInstructions ballotInstructions) : base("Condorcet (Ranked Pairs)", ballotInstructions)
        {
        }

        public override VotingSystemResult getResult(Roster roster, List<Ballot> ballotList)
        {
            LockNode.Clear();

            foreach(Candidate candidate in roster.candidateList)
            {
                new LockNode(candidate);
            }

            CondorcetTally condorcetTally = new CondorcetTally(roster, ballotList);
            List<Majority> majorityList = createMajorities(roster, condorcetTally);
            createCandidateMaps(majorityList);

            VotingSystemResult result = buildVotingSystemResult();

            if (Tweakables.PRINT_CONDORCET_RANKED_PAIRS)
            {
                string output = "Condorcet ranked pairs results [ ";

                bool firstLine = true;
                foreach (KeyValuePair<int, List<Candidate>> entry in result.scoreDictionary.OrderByDescending(e => e.Key))
                {
                    if (!firstLine)
                    {
                        output = output + ", ";
                    }

                    if (entry.Value.Count == 1)
                    {
                        output = output + entry.Value.First().index;
                    }
                    else
                    {
                        output = output + "{ ";
                        bool firstCandidate = true;
                        foreach (Candidate candidate in entry.Value)
                        {
                            if (!firstCandidate)
                            {
                                output = output + ", ";
                            }
                            output = output + candidate.index;
                            firstCandidate = false;
                        }
                        output = output + " }";
                    }

                    firstLine = false;
                }
                output = output + " ]";
                System.Console.WriteLine(output);
            }

            return result;
        }

        private List<Majority> createMajorities(Roster roster, CondorcetTally condorcetTally)
        {
            List<Majority> majorityList = new List<Majority>();

            foreach (Candidate subjectCandidate in roster.candidateList)
            {
                foreach (Candidate objectCandidate in roster.candidateList)
                {
                    if (subjectCandidate == objectCandidate || subjectCandidate.index < objectCandidate.index)
                    {
                        continue;
                    }

                    int subjectVotes = condorcetTally.getVotes(subjectCandidate, objectCandidate);
                    int objectVotes = condorcetTally.getVotes(objectCandidate, subjectCandidate);
                    Majority majority = new Majority(subjectCandidate, objectCandidate, subjectVotes, objectVotes);
                    majorityList.Add(majority);
                }
            }

            return majorityList;
        }

        private void createCandidateMaps(List<Majority> majorityList)
        {
            foreach (Majority majority in majorityList.OrderByDescending(m => m.tally).ThenByDescending(m => m.winnerVotes))
            {
                if (majority.tally > 0)
                {
                    if (LockNode.addVictory(majority.winningCandidate, majority.losingCandidate))
                    {
                        //System.Console.WriteLine("Candidate {0} defeats candidate {1}", majority.winningCandidate.index, majority.losingCandidate.index);
                    }
                    else
                    {
                        //System.Console.WriteLine("Candidate {0} defeats candidate {1} but not recorded as it would cause a loop", majority.winningCandidate.index, majority.losingCandidate.index);
                    }
                }
                else
                {
                    if (LockNode.addTie(majority.winningCandidate, majority.losingCandidate))
                    {
                        //System.Console.WriteLine("Candidate {0} ties candidate {1}", majority.winningCandidate.index, majority.losingCandidate.index);
                    }
                    else
                    {
                        //System.Console.WriteLine("Candidate {0} ties candidate {1} but not recorded as it would cause a loop", majority.winningCandidate.index, majority.losingCandidate.index);
                    }
                }
            }
        }

        private VotingSystemResult buildVotingSystemResult()
        {
            VotingSystemResult result = new VotingSystemResult(this);
            List<Candidate> remainingCandidates = LockNode.lockNodeDict.Keys.ToList();

            while (remainingCandidates.Count > 0)
            {
                result.add(getNextWinners(remainingCandidates));
            }

            return result;
        }

        private List<Candidate> getNextWinners(List<Candidate> remainingCandidates)
        {
            Candidate winningCandidate = LockNode.getCondorcetWinner(remainingCandidates);
            List<Candidate> candidateList = new List<Candidate>();

            if (winningCandidate != null)
            {
                //System.Console.WriteLine("{0} is the Condorcet winner of remaining candidates", winningCandidate.getName());
                candidateList.Add(winningCandidate);
                remainingCandidates.Remove(winningCandidate);
                return candidateList;
            }

            //System.Console.WriteLine("No Condorcet winner.  Building loops.");
            winningCandidate = LockNode.getBestCandidate(remainingCandidates);
            //System.Console.WriteLine("{0} appears to be the best.  Looking for candidates in that loop.", winningCandidate.getName());
            candidateList.Add(winningCandidate);
            remainingCandidates.Remove(winningCandidate);
            candidateList.AddRange(LockNode.getAllCandidatesThatTieOrBeat(winningCandidate, remainingCandidates));
            return candidateList;
        }

        public class Majority
        {
            public Candidate winningCandidate { get; }
            public Candidate losingCandidate { get; }
            public int tally { get; }
            public int winnerVotes { get; } = 0;

            public Majority(Candidate subjectCandidate, Candidate objectCandidate, int subjectVotes, int objectVotes)
            {
                if (subjectVotes > objectVotes)
                {
                    this.winningCandidate = subjectCandidate;
                    this.losingCandidate = objectCandidate;
                    this.tally = subjectVotes - objectVotes;
                    this.winnerVotes = subjectVotes;
                    return;
                }

                if (subjectVotes < objectVotes)
                {
                    this.winningCandidate = objectCandidate;
                    this.losingCandidate = subjectCandidate;
                    this.tally = objectVotes - subjectVotes;
                    this.winnerVotes = objectVotes;
                    return;
                }

                this.winningCandidate = subjectCandidate;
                this.losingCandidate = objectCandidate;
                this.tally = 0;
                this.winnerVotes = subjectVotes;
            }
        }

        public class LockNode
        {
            public static Dictionary<Candidate, LockNode> lockNodeDict { get; } = new Dictionary<Candidate, LockNode>();
   
            public Candidate candidate { get; }
            public List<Candidate> defeatedCandidateList { get; } = new List<Candidate>();
            public List<Candidate> tiedCandidateList { get; } = new List<Candidate>();

            public LockNode(Candidate candidate)
            {
                this.candidate = candidate;
                lockNodeDict[candidate] = this;
            }

            public static bool addVictory(Candidate winningCandidate, Candidate losingCandidate)
            {
                lockNodeDict[winningCandidate].defeatedCandidateList.Add(losingCandidate);

                if (lockNodeDict[winningCandidate].isTreeConsistent())
                {
                    return true;
                }

                lockNodeDict[winningCandidate].defeatedCandidateList.Remove(losingCandidate);
                return false;
            }

            public static bool addTie(Candidate candidateOne, Candidate candidateTwo)
            {
                lockNodeDict[candidateOne].tiedCandidateList.Add(candidateTwo);
                lockNodeDict[candidateTwo].tiedCandidateList.Add(candidateOne);

                if (lockNodeDict[candidateOne].isTreeConsistent())
                {
                    return true;
                }

                lockNodeDict[candidateOne].tiedCandidateList.Remove(candidateTwo);
                lockNodeDict[candidateTwo].tiedCandidateList.Remove(candidateOne);
                return false;
            }

            public static Candidate getCondorcetWinner(List<Candidate> remainingCandidates)
            {
                foreach (Candidate candidate in remainingCandidates)
                {
                    LockNode lockNode = lockNodeDict[candidate];
                    bool condorcetCandidate = true;

                    foreach (Candidate competingCandidate in remainingCandidates)
                    {
                        if (candidate == competingCandidate)
                        {
                            continue;
                        }

                        if (!lockNode.defeatedCandidateList.Contains(competingCandidate))
                        {
                            condorcetCandidate = false;
                            break;
                        }
                    }

                    if (condorcetCandidate)
                    {
                        return candidate;
                    }
                }

                return null;
            }

            public static Candidate getBestCandidate(List<Candidate> remainingCandidates)
            {
                if (remainingCandidates.Count == 0)
                {
                    return null;
                }

                Candidate bestCandidate = null;
                int bestCandidateLosses = 0;

                foreach (Candidate candidate in remainingCandidates)
                {
                    LockNode lockNode = lockNodeDict[candidate];
                    int candidateLosses = 0;

                    foreach (Candidate competingCandidate in remainingCandidates)
                    {
                        if (candidate == competingCandidate)
                        {
                            continue;
                        }
                         if (lockNodeDict[competingCandidate].defeatedCandidateList.Contains(candidate))
                        {
                            candidateLosses++;
                        }
                    }

                    if (bestCandidate == null || candidateLosses < bestCandidateLosses)
                    {
                        bestCandidate = candidate;
                        bestCandidateLosses = candidateLosses;
                    }
                }

                return bestCandidate;
            }

            public static List<Candidate> getAllCandidatesThatTieOrBeat(Candidate candidate, List<Candidate> remainingCandidates)
            {
                LockNode lockNode = lockNodeDict[candidate];
                List<Candidate> foundCandidateList = new List<Candidate>();

                foreach (Candidate remainingCandidate in remainingCandidates)
                {
                    if (lockNodeDict[remainingCandidate].defeatedCandidateList.Contains(candidate))
                    {
                        //System.Console.WriteLine("{0} beats {1} so it is added to the list", remainingCandidate.getName(), candidate.getName());
                        foundCandidateList.Add(remainingCandidate);
                        continue;
                    }

                    if (lockNodeDict[remainingCandidate].tiedCandidateList.Contains(candidate))
                    {
                        //System.Console.WriteLine("{0} ties {1} so it is added to the list", remainingCandidate.getName(), candidate.getName());
                        foundCandidateList.Add(remainingCandidate);
                        continue;
                    }
                }

                foreach (Candidate foundCandidate in foundCandidateList)
                {
                    remainingCandidates.Remove(foundCandidate);
                }

                List<Candidate> staticFoundCandidateList = foundCandidateList.ToList();
                foreach (Candidate foundCandidate in staticFoundCandidateList)
                {
                    foundCandidateList.AddRange(getAllCandidatesThatTieOrBeat(candidate, remainingCandidates));
                }

                return foundCandidateList;
            }

            private bool isTreeConsistent()
            {
                return isNodeConsistent(candidate, new List<Candidate>());
            }

            private bool isNodeConsistent(Candidate candidate, List<Candidate> unbeatenCandidates)
            {
                //System.Console.WriteLine("Checking for tree consistency at {0}", candidate.getName());
                // Create a new unbeaten list and add ourselves
                // We don't want to add ourselves to the existing list because future iterations may be above us
                // For example, if the order is A>B>C>D and we could be C who is called from A
                // If C was checked before B, B will defeat us when called resulting in inconsistency
                List<Candidate> newUnbeatenCandidatesList = unbeatenCandidates.ToList();
                newUnbeatenCandidatesList.Add(candidate);

                // Create a list of nodes reflecting our location in the tree
                List<LockNode> currentLocationNodeList = new List<LockNode>();
                currentLocationNodeList.Add(lockNodeDict[candidate]);

                // A candidate has its own relationships and the relationships of candidates it is equal to
                foreach (Candidate tiedCandidate in lockNodeDict[candidate].tiedCandidateList)
                {
                    // If we are tied with any candidate that is unbeaten then a loop exists
                    if (unbeatenCandidates.Contains(tiedCandidate))
                    {
                        //System.Console.WriteLine("{0} is tied with {1} which is on the unbeatenCandidates list so tree is inconsistent.", candidate.getName(), tiedCandidate.getName());
                        return false;
                    }

                    //System.Console.WriteLine("We are tied with {0} so we will consider its victories ours", tiedCandidate.getName());
                    currentLocationNodeList.Add(lockNodeDict[tiedCandidate]);
                    newUnbeatenCandidatesList.Add(tiedCandidate);
                }

                // Find all the candidates we defeat at this part of the tree
                List<Candidate> newDefeatedCandidates = new List<Candidate>();
                foreach (LockNode currentLocationNode in currentLocationNodeList)
                {
                    foreach (Candidate defeatedCandidate in currentLocationNode.defeatedCandidateList.Except(newDefeatedCandidates))
                    {
                        // If we beat any candidates that are unbeaten, there is a loop
                        if (unbeatenCandidates.Contains(defeatedCandidate))
                        {
                            //System.Console.WriteLine("{0} is defeated by {1} but it is on the unbeatenCandidates list so tree is inconsistent.", defeatedCandidate.getName(), currentLocationNode.candidate.getName());
                            return false;
                        }

                        //System.Console.WriteLine("We defeat {0}", defeatedCandidate.getName());
                        newDefeatedCandidates.Add(defeatedCandidate);
                    }
                }

                // Check every newly defeated candidate for consistency
                foreach (Candidate newDefeatedCandidate in newDefeatedCandidates)
                {
                    //System.Console.WriteLine("Recursively checking to see if {0} is consistent", newDefeatedCandidate.getName());
                    if (!isNodeConsistent(newDefeatedCandidate, newUnbeatenCandidatesList))
                    {
                        return false;
                    }
                }

                //System.Console.WriteLine("Node {0} is consistent", candidate.getName());
                return true;
            }

            public static void Clear()
            {
                foreach (LockNode lockNode in lockNodeDict.Values)
                {
                    lockNode.defeatedCandidateList.Clear();
                    lockNode.tiedCandidateList.Clear();
                }

                lockNodeDict.Clear();
            }
        }
    }
    */
}
