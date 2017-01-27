using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectionSimulator.VotingSystems;

namespace ElectionSimulator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ReadKey();
            System.Console.WriteLine("");
            DeviationComparison comparison = new DeviationComparison("C:\\local\\voting.csv");
            comparison.runSimulation(Tweakables.TRIAL_COUNT);
            Console.ReadKey();
        }
    }
}
