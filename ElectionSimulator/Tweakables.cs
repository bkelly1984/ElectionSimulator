using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectionSimulator
{
    class Tweakables
    {
        public static int TRIAL_COUNT = 1000;

        // Election tweakables
        public static int CANDIDATE_COUNT = 5;  // How many candidates in each race
        public static int VOTER_COUNT = 20;  // How many voters in each race

        // Political spectrum tweakables
        public static int SPECTRUM_DIMENSIONS = 2;  // Number of dimensions the political spectrum has
        public static int PERCENT_MIN_SPECTRUM_GAUSSIAN = 0; // Minimum percent the distribution will be a gaussian distribution instead of linear
        public static int PERCENT_MAX_SPECTRUM_GAUSSIAN = 100; // Maximum percent the distribution will be a gaussian distribution instead of linear
        public static int PERCENT_MIN_SPECTRUM_SKEW = 0; // Minimum percent the center of the distribution will be shifted to one side
        public static int PERCENT_MAX_SPECTRUM_SKEW = 90; // Maximum percent the center of the distribution will be shifted to one side (<=90 is reasonable)

        // Individual Biases
        //public static double CONFIRMATION_BIAS_PERCENT = 0;  // Maximum percent increase/decrease of distance for positions close/distant to ours on the political spectrum
        //public static double HALO_EFFECT_BIAS_PERCENT = 0;  // Maximum percent increase/decrease of distance for candidates close/distant to us on the political spectrum
        //public static double PREFERENCE_FROM_DISTANCE_PERCENT = 100;  // What percent of perceived political spectrum distance defines a voter preference for a candidate (as opposed to random)

        // Debugging
        public static bool PRINT_POSITION = false;  // Prints a person's spectrum position to the console
        public static bool PRINT_VIEWPOINT = true;  // Print's a voter's perspective on the candidates to the console
        public static bool PRINT_ROSTER = false;  // Print's which voters become candidates

        public static bool PRINT_BALLOTS = true;  // Prints ballots cast by voters
        public static bool PRINT_RESULTS = true;  // Prints voting system results

        public static bool PRINT_BORDA = false;  // Prints results of a borda count election
        public static bool PRINT_CONDORCET = false;  // Prints results of a borda count election
        public static bool PRINT_CONDORCET_RANKED_PAIRS = false;  // Prints results of a borda count election
        public static bool PRINT_FPTP = false;  // Prints results of a ranked choice election
        public static bool PRINT_RCV = false;  // Prints results of a ranked choice election
        public static bool PRINT_SCORE = false;  // Prints results of a borda count election
        public static bool PRINT_SCORE_PLUS_TOP_TWO = false;  // Prints results of a score followed by a top two runoff election
        public static bool PRINT_THREE_STEP_VOTING = false;  // Prints results of a 1-2-3 voting

    }
}
