using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConsoleApp1
{
    class Program
    {
        public static void Main1(string[] args)
        {
            StreamReader Input = new StreamReader("Input.txt");
            //StreamWriter Input2 = new StreamWriter("Input.txt");
            int States = int.Parse(Input.ReadLine());

            //Save Alphabets
            List<string> Alphabets = new List<string>();
            string[] alphabets = Input.ReadLine().Split(',');
            for (int i = 0; i < alphabets.Length; i++)
            {
                Alphabets.Add(alphabets[i]);
            }

            List<List<string>> Transitions = new List<List<string>>();
            List<string> Transition = new List<string>();
            Transition = Input.ReadLine().Split(',').ToList();
            for (int i = 0; Input.ReadLine() != null; i++)
            {
                //Input2.WriteLine("asd");
                Transitions.Add(Transition);
                Transition = Input.ReadLine().Split(',').ToList();
            }

            // for(int i=0;i<Transitions[1].Count;i++)
            //   Console.WriteLine($"{Transitions[1][i]}");

            NFAToDFA(Transitions, States, Alphabets);
        }
        public static List<List<string>> NFAToDFA(List<List<string>> Transitions, int States, List<string> Alphabets)
        {
            string InitialState = "i";
            for (int i = 0; i < Transitions.Count; i++)
            {
                if (Transitions[i][0].Substring(0, 2) == "->")
                    InitialState = Transitions[i][0].Substring(2);
            }
            int GroupsNum = 0;
            //int EachGroup = 0;
            List<string> Group = new List<string>();
            Group.Add(InitialState);
            List<List<string>> Groups = new List<List<string>>();
            Groups.Add(Group);

            List<string> group = new List<string>();
            for (int i = 0; i < Groups.Count; i++)
            {
                for (int j = 0; j < Groups[i].Count; j++)
                {
                    for (int a = 0; a < Alphabets.Count; a++)
                    {
                        for (int k = 0; k < Transitions.Count; k++)
                        {
                            if (Transitions[k][0] == Groups[i][j] && Transitions[k][1] == Alphabets[a])
                            {
                                //Groups[GroupsNum][EachGroup] = Transitions[k][2];
                                // Groups[GroupsNum][EachGroup] = Transitions[k][2];
                                group = new List<string>();
                                group.Add(Transitions[k][2]);
                                //Groups[GroupsNum].Add(Transitions[k][2]);
                                // EachGroup++;
                            }
                        }
                        Groups.Add(group);
                        //Groups.Add(Group);
                        // EachGroup = 0;
                        GroupsNum++;
                    }
                }
            }

            //string[][][] Q = new string[GroupsNum][][];
            List<List<List<string>>> Q = new List<List<List<string>>>();
            //string[] Results = new string[States];
            List<string> Results = new List<string>();
            for (int i = 0; i < GroupsNum; i++)
            {
                for (int j = 0; j < States; j++)
                {
                    for (int a = 0; a < Alphabets.Count; a++)
                    {
                        for (int k = 0; k < Transitions.Count; k++)
                        {
                            if (Transitions[k][0] == Groups[i][j] && Transitions[k][1] == Alphabets[a])
                            {
                                //Results[EachGroup] = Transitions[k][2];
                                //EachGroup++;
                                Q[i][1].Add(Transitions[k][2]);
                            }
                        }
                        //Q[i][0][0] = Alphabets[a];
                        Q[i][0].Add(Alphabets[a]);
                        //Q[GroupsNum][1] = Results;
                    }
                }
            }
            for (int i = 0; i < GroupsNum; i++)
            {
                for (int j = 0; j < GroupsNum; j++)
                {
                    if (Q[i][1] == Groups[j])
                        Q[i][1][0] = $"q{j}";

                }
            }

            //string[][] DFA = new string[GroupsNum][];
            List<List<string>> DFA = new List<List<string>>();
            for (int i = 0; i < GroupsNum; i++)
            {
                //DFA[i][0] = $"q{i}";
                //DFA[i][1] = Q[i][0][0];
                //DFA[i][2] = Q[i][1][0];
                DFA[i].Add($"q{i}");
                DFA[i].Add(Q[i][0][0]);
                DFA[i].Add(Q[i][1][0]);
            }

            for (int i = 0; i < DFA.Count; i++)
            {
                Console.WriteLine($"{DFA[i][0]},{DFA[i][1]},{DFA[i][2]}");
            }
            return DFA;
        }

        public static void Main(string[] args)
        {
            List<List<string>> dfaTransitions = new List<List<string>>();


            //--------------------------------------test for part 2-----------------------------------------------------------

            dfaTransitions.Add(new List<string>() { "5" });
            dfaTransitions.Add(new List<string>() { "a", "b" });
            dfaTransitions.Add(new List<string>() { "->q0", "a", "q1" });
            dfaTransitions.Add(new List<string>() { "q0", "b", "*q2" });
            dfaTransitions.Add(new List<string>() { "q1", "b", "*q2" });
            dfaTransitions.Add(new List<string>() { "q1", "a", "*q3" });
            dfaTransitions.Add(new List<string>() { "*q2", "a", "*q2" });
            dfaTransitions.Add(new List<string>() { "*q2", "b", "q4" });
            dfaTransitions.Add(new List<string>() { "*q3", "a", "*q3" });
            dfaTransitions.Add(new List<string>() { "*q3", "b", "*q3" });
            dfaTransitions.Add(new List<string>() { "q4", "a", "q1" });
            dfaTransitions.Add(new List<string>() { "q4", "b", "*q2" });

            //--------------------------------------test for dfa simplification-----------------------------------------------------------

            //int stateCount = 5;
            //int alphabetCount = 2;
            //string[] InputAlphabet = new string[] { "a", "b" };
            //HashSet<string> FinalStates = new HashSet<string> { "q2", "q3" };
            //string InitialState = "q0";

            //Dictionary<string, Dictionary<string, string>> transitions = new Dictionary<string, Dictionary<string, string>>();

            //transitions.Add("q0", new Dictionary<string, string>());
            //transitions.Add("q1", new Dictionary<string, string>());
            //transitions.Add("q2", new Dictionary<string, string>());
            //transitions.Add("q3", new Dictionary<string, string>());
            //transitions.Add("q4", new Dictionary<string, string>());

            //transitions["q0"].Add("a", "q1");
            //transitions["q0"].Add("b", "q2");
            //transitions["q1"].Add("b", "q2");
            //transitions["q1"].Add("a", "q3");
            //transitions["q2"].Add("a", "q2");
            //transitions["q2"].Add("b", "q4");
            //transitions["q3"].Add("a", "q3");
            //transitions["q3"].Add("b", "q3");
            //transitions["q4"].Add("a", "q1");
            //transitions["q4"].Add("b", "q2");
            //--------------------------------------------------------------------------------------------------------------


            //dfa simplification initialization

            string[] inputAlphabet = new string[0];
            HashSet<string> finalStates = new HashSet<string>();
            string initialState;
            var transitions = PreProccess(dfaTransitions, out finalStates, out initialState, out inputAlphabet);

            HashSet<int> newFinalStates;
            int newInitialState;
            var simplifiedTransitions = DFASimplification(inputAlphabet.Length, finalStates, initialState, transitions, inputAlphabet, out newFinalStates, out newInitialState);

            var result = SimplifyOutput(simplifiedTransitions, newFinalStates, newInitialState, inputAlphabet);



            Console.WriteLine(transitions.Keys.Count);

            StringBuilder str = new StringBuilder();
            foreach (string alphabet in inputAlphabet)
                str.Append($"{alphabet},");
            Console.WriteLine(str.ToString().TrimEnd(new char[] {',' }));

            foreach (string transition in result)
                Console.WriteLine(transition);
        }

        public static List<string> SimplifyOutput(Dictionary<int, Dictionary<string, int>> simplifiedTransitions, HashSet<int> newFinalStates, int newInitialState, string[] inputAlphabet)
        {

            List<string> result = new List<string>();
            StringBuilder str = new StringBuilder();
            bool appeared = false;
            foreach (var fromState in simplifiedTransitions)
            {
                foreach (string alphabet in inputAlphabet)
                {
                    str = new StringBuilder();
                    if (!fromState.Value.ContainsKey(alphabet))
                        continue;


                    if (fromState.Key == newInitialState && !appeared)
                    {
                        appeared = true;
                        str.Append("->");
                    }
                    if (newFinalStates.Contains(fromState.Key))
                        str.Append("*");
                    str.Append($"g{fromState.Key}");

                    str.Append(",");

                    str.Append(alphabet);

                    str.Append(",");

                    int toState = fromState.Value[alphabet];
                    if (newFinalStates.Contains(toState))
                        str.Append("*");
                    str.Append($"g{toState}");

                    result.Add(str.ToString());
                }
            }

            return result;
        }

        public static Dictionary<int, Dictionary<string, int>> DFASimplification(int alphabetCount, HashSet<string> finalStates, string initialState, Dictionary<string, Dictionary<string, string>> transitions, string[] inputAlphabet, out HashSet<int> newFinalStates, out int newInitialState)
        {
            Dictionary<string, int> groupNum = new Dictionary<string, int>();
            Dictionary<string, int> newGroupNum = new Dictionary<string, int>();
            Dictionary<string, int[]> checker;


            //level 0 grouping
            foreach (string item in finalStates)
                groupNum.Add(item, 1);

            foreach (var item in transitions)
            {
                if (finalStates.Contains(item.Key))
                    continue;

                groupNum.Add(item.Key, 2);
            }


            //--------------------------------------------------------------------------------------

            while (true)
            {
                newGroupNum = new Dictionary<string, int>();
                checker = new Dictionary<string, int[]>();


                //filling transition table
                foreach (var fromState in transitions)
                {
                    checker.Add(fromState.Key, new int[alphabetCount + 1]);

                    checker[fromState.Key][alphabetCount] = groupNum[fromState.Key];

                    for (int i = 0; i < inputAlphabet.Length; i++)
                    {
                        string toState = transitions[fromState.Key][inputAlphabet[i]];

                        if (toState != null)
                            checker[fromState.Key][i] = groupNum[toState];

                        else
                            checker[fromState.Key][i] = -1;
                    }
                }

                //identifying distinct outGroups
                Dictionary<int[], int> makeNewGroup = new Dictionary<int[], int>(new MyEqualityComparer());
                int counter = 1;
                foreach (int[] outGroup in checker.Values)
                {
                    if (!makeNewGroup.ContainsKey(outGroup))
                        makeNewGroup.Add(outGroup, counter++);
                }

                //matching outGroups with states and grouping states based on their outGroups
                foreach (var item in checker)
                {
                    int newGroupIdx = makeNewGroup[item.Value];

                    newGroupNum.Add(item.Key, newGroupIdx);
                }

                if (groupNum.SequenceEqual(newGroupNum))
                    break;

                groupNum = newGroupNum;
            }

            //making the result dfa
            var simplifiedTransitions = new Dictionary<int, Dictionary<string, int>>();
            newFinalStates = new HashSet<int>();


            foreach (var fromState in transitions)
            {
                int groupIdx = groupNum[fromState.Key];

                if (simplifiedTransitions.ContainsKey(groupIdx))
                    continue;

                simplifiedTransitions.Add(groupIdx, new Dictionary<string, int>());

                foreach (string alphabet in inputAlphabet)
                {
                    var toState = transitions[fromState.Key][alphabet];

                    simplifiedTransitions[groupIdx].Add(alphabet, groupNum[toState]);
                }
            }

            newInitialState = groupNum[initialState];

            foreach (string finalState in finalStates)
                newFinalStates.Add(groupNum[finalState]);

            return simplifiedTransitions;
        }

        public static Dictionary<string, Dictionary<string, string>> PreProccess(List<List<string>> dfaTransitions, out HashSet<string> finalStates, out string initialState, out string[] inputAlphabet)
        {
            initialState = "";
            finalStates = new HashSet<string>();
            var convertedTransitions = new Dictionary<string, Dictionary<string, string>>();

            int stateCount = int.Parse(dfaTransitions[0].First());
            inputAlphabet = dfaTransitions[1].ToArray();

            for (int i = 2; i < dfaTransitions.Count; i++)
            {
                var toks = dfaTransitions[i];

                if (toks[0].Contains("->")) // ->S1
                {
                    toks[0] = toks[0].Trim(new char[] { '-', '>' });
                    initialState = toks[0].Trim(new char[] { '*' });
                }

                if (toks[0].Contains("*")) // *S1
                {
                    toks[0] = toks[0].Trim(new char[] { '*' });
                    finalStates.Add(toks[0]);
                }

                if (toks[2].Contains("*"))     //*S2
                {
                    toks[2] = toks[2].Trim(new char[] { '*' });
                    finalStates.Add(toks[2]);
                }

                // s1 , a , s2
                if (!convertedTransitions.ContainsKey(toks[0]))
                    convertedTransitions.Add(toks[0], new Dictionary<string, string>());

                convertedTransitions[toks[0]].Add(toks[1], toks[2]);
            }

            return convertedTransitions;
        }
        public class MyEqualityComparer : IEqualityComparer<int[]>
        {
            public bool Equals(int[] x, int[] y)
            {
                if (x.Length != y.Length)
                {
                    return false;
                }
                for (int i = 0; i < x.Length; i++)
                {
                    if (x[i] != y[i])
                    {
                        return false;
                    }
                }
                return true;
            }

            public int GetHashCode(int[] obj)
            {
                int result = 17;
                for (int i = 0; i < obj.Length; i++)
                {
                    unchecked
                    {
                        result = result * 23 + obj[i];
                    }
                }
                return result;
            }
        }


    }
}
