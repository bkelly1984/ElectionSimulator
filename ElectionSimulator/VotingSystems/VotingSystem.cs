using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectionSimulator.Ballots;
using ElectionSimulator.People;

namespace ElectionSimulator.VotingSystems
{
    public abstract class VotingSystem
    {
        public string name { get; }
        public BallotInstructions ballotInstructions { get; }

        public VotingSystem(String name, BallotInstructions ballotInstructions)
        {
            this.name = name;
            this.ballotInstructions = ballotInstructions;
        }

        public abstract VotingSystemResult getResult(Roster roster, List<Ballot> ballotList);

        public override string ToString()
        {
            return name;
        }
    }
}
