using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectionSimulator.Ballots;
using ElectionSimulator.People;
using ElectionSimulator.VotingSystems;

namespace ElectionSimulator
{
    class DeviationComparison
    {
        Spectrum politicalSpectrum;
        List<VotingSystem> votingSystemList;
        List<Voter> voterList;
        string outputPath;

        public DeviationComparison(string outputPath)
        {
            this.outputPath = outputPath;
            votingSystemList = getVotingSystemList();
            politicalSpectrum = new Spectrum();
            voterList = Voter.createVoterList(Tweakables.VOTER_COUNT, politicalSpectrum);

            System.IO.File.WriteAllText(@outputPath, Tweakables.TRIAL_COUNT + " trials run with " + Tweakables.CANDIDATE_COUNT + " candidates and " + Tweakables.VOTER_COUNT + " voters and " + Tweakables.SPECTRUM_DIMENSIONS + " political spectrum dimensions." + Environment.NewLine);
            System.IO.File.AppendAllText(@outputPath, ElectionSummary.getHeader(votingSystemList));
            System.IO.File.AppendAllText(@outputPath, Environment.NewLine);
        }

        private List<VotingSystem> getVotingSystemList()
        {
            BallotInstructions condorcetBallot = new BallotInstructions(BallotType.Rank);
            condorcetBallot.setRankInstructions(Tweakables.CANDIDATE_COUNT, 0);

            BallotInstructions score2Ballot = new BallotInstructions(BallotType.Score);
            score2Ballot.setScoreInstructions(2, Tweakables.CANDIDATE_COUNT);

            BallotInstructions score4Ballot = new BallotInstructions(BallotType.Score);
            score4Ballot.setScoreInstructions(4, Tweakables.CANDIDATE_COUNT);

            BallotInstructions score6Ballot = new BallotInstructions(BallotType.Score);
            score6Ballot.setScoreInstructions(6, Tweakables.CANDIDATE_COUNT);

            BallotInstructions score10Ballot = new BallotInstructions(BallotType.Score);
            score10Ballot.setScoreInstructions(10, Tweakables.CANDIDATE_COUNT);

            BallotInstructions score100Ballot = new BallotInstructions(BallotType.Score);
            score100Ballot.setScoreInstructions(100, Tweakables.CANDIDATE_COUNT);


            List<VotingSystem> votingSystemList = new List<VotingSystem>();
            votingSystemList.Add(new Condorcet(condorcetBallot));
            votingSystemList.Add(new FPTP(condorcetBallot));
            //votingSystemList.Add(new Score(score2Ballot));
            //votingSystemList.Add(new Score(score4Ballot));
            //votingSystemList.Add(new Score(score6Ballot));
            //votingSystemList.Add(new Score(score10Ballot));
            //votingSystemList.Add(new Score(score100Ballot));
            return votingSystemList;
        }

        public void runSimulation(int trials)
        {
            Roster roster;
            List<Ballot> ballotList;
            Dictionary<BallotInstructions, List<Ballot>> ballotDictionary = new Dictionary<BallotInstructions, List<Ballot>>();
            VotingSystemResult votingSystemResult;
            List<VoteSummary> voteSummaryList = new List<VoteSummary>();

            for (int i = 0; i < trials; i++)
            {
                roster = new Roster(voterList);
                foreach (Voter voter in voterList)
                {
                    voter.setRoster(roster);
                }

                ballotDictionary.Clear();

                foreach (VotingSystem votingSystem in votingSystemList)
                {
                    // Get or create the ballots
                    if (ballotDictionary.ContainsKey(votingSystem.ballotInstructions))
                    {
                        ballotList = ballotDictionary[votingSystem.ballotInstructions];
                    }
                    else
                    {
                        ballotList = new List<Ballot>();
                        foreach (Voter voter in voterList)
                        {
                            ballotList.Add(voter.createBallot(votingSystem.ballotInstructions));
                        }
                        ballotDictionary.Add(votingSystem.ballotInstructions, ballotList);
                    }

                    // Get the vote summary
                    votingSystemResult = votingSystem.getResult(roster, ballotList);
                    voteSummaryList.Add(getVoteSummary(votingSystemResult));
                }

                ElectionSummary summary = new ElectionSummary(
                    roster.lowestDeviationCandidate.voter.position.deviation,
                    roster.highestDeviationCandidate.voter.position.deviation,
                    voteSummaryList
                );

                System.IO.File.AppendAllText(@outputPath, summary.ToString() + Environment.NewLine);

                System.Console.WriteLine("Simulation {0} complete", i + 1);
                Console.ReadKey();
                System.Console.WriteLine("");
            }
        }

        private void printResult(VotingSystemResult result)
        {
            List<Candidate> winnerList = result.getWinnerList();

            if (winnerList.Count == 0)
            {
                System.Console.WriteLine("{0}: No Winner", result.votingSystem.name);
                return;
            }

            if (winnerList.Count > 1)
            {
                System.Console.WriteLine("{0}: Tie", result.votingSystem.name);
                return;
            }

            Candidate winner = winnerList.First();
            System.Console.WriteLine("{0}: Winner is {1} with deviation {2}", result.votingSystem.name, winner.index, winner.voter.position.deviation);
        }

        private void printSummary(VoteSummary summary)
        {
            if (summary.noWinnerFlag)
            {
                System.Console.WriteLine("{0}: No Winner", summary.votingSystemName);
                return;
            }

            if (summary.tieFlag)
            {
                System.Console.WriteLine("{0}: Tie with deviation {1}", summary.votingSystemName, summary.deviation);
                return;
            }

            System.Console.WriteLine("{0}: Winner with deviation {1}", summary.votingSystemName, summary.deviation);
        }

        private VoteSummary getVoteSummary(VotingSystemResult votingSystemResult)
        {
            VoteSummary summary = new VoteSummary(votingSystemResult.votingSystem.name);
            List<Candidate> winnerList = votingSystemResult.getWinnerList();

            if (winnerList == null || winnerList.Count == 0)
            {
                summary.noWinnerFlag = true;
                return summary;
            }

            if (winnerList.Count == 1)
            {
                summary.deviation = winnerList.First().voter.position.deviation;
                return summary;
            }

            double averageDeviation = 0.0;
            foreach (Candidate winner in winnerList) {
                averageDeviation += winner.voter.position.deviation;
            }
            averageDeviation /= winnerList.Count;

            summary.deviation = averageDeviation;
            summary.tieFlag = true;
            return summary;
        }

        private class VoteSummary
        {
            public string votingSystemName;
            public bool noWinnerFlag = false;
            public bool tieFlag = false;
            public double deviation;

            public VoteSummary(string name)
            {
                votingSystemName = name;
            }
        }

        private class ElectionSummary
        {
            public double minDeviation;
            public double maxDeviation;
            public List<VoteSummary> voteSummaryList;

            public ElectionSummary(double minDeviation, double maxDeviation, List<VoteSummary> voteSummaryList)
            {
                this.minDeviation = minDeviation;
                this.maxDeviation = maxDeviation;
                this.voteSummaryList = voteSummaryList;
            }

            public override string ToString()
            {
                string output = minDeviation + "," + maxDeviation;

                foreach (VoteSummary voteSummary in voteSummaryList)
                {
                    output = output + "," + getResult(voteSummary);
                    output = output + "," + voteSummary.deviation;
                }

                return output;
            }

            public static string getHeader(List<VotingSystem> votingSystemList)
            {
                string output = "Minimum Deviation,Maximum Deviation";

                foreach (VotingSystem votingSystem in votingSystemList)
                {
                    output = output + "," + votingSystem.name + " Result";
                    output = output + "," + votingSystem.name + " Deviation";
                }

                return output;
            }

            private static char getResult(VoteSummary summary)
            {
                if (summary.noWinnerFlag)
                {
                    return 'N';
                }
                if (summary.tieFlag)
                {
                    return 'T';
                }
                return ' ';
            }
        }
    }
}

