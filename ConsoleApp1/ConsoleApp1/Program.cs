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
        public static void Main(string[] args)
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
            string temp;
            for (int i = 0; (temp = Input.ReadLine()) != null; i++)
            {
                Transition = temp.Split(',').ToList();
                Transitions.Add(Transition);
            }

            // for(int i=0;i<Transitions[1].Count;i++)
            //   Console.WriteLine($"{Transitions[1][i]}");


            List<List<string>> dfaTransitions = NFAToDFA(Transitions, States, Alphabets);


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
            Console.WriteLine(str.ToString().TrimEnd(new char[] { ',' }));

            foreach (string transition in result)
                Console.WriteLine(transition);
        }

        //--------------------------------------part 1-----------------------------------------------------

        static List<List<string>> NFAToDFA(List<List<string>> Transitions, int States, List<string> Alphabets)
        {
            string InitialState = "i";
            for (int i = 0; i < Transitions.Count; i++)
            {
                if (Transitions[i][0].Substring(0, 2) == "->")
                {
                    InitialState = Transitions[i][0].Substring(2);
                    Transitions[i][0] = InitialState;
                }
            }

            List<string> finalStates = new List<string>();
            for (int i = 0; i < Transitions.Count; i++)
            {

                if (Transitions[i][0].Substring(0, 1) == "*" && !finalStates.Contains(Transitions[i][0]))
                {
                    finalStates.Add(Transitions[i][0]);
                }
                if (Transitions[i][2].Substring(0, 1) == "*" && !finalStates.Contains(Transitions[i][2]))
                    finalStates.Add(Transitions[i][2]);
            }

            bool isFinalState = false;
            List<string> group = new List<string>();
            group.Add(InitialState);
            List<List<string>> groups = new List<List<string>>();
            groups.Add(group);
            List<List<string>> results = new List<List<string>>();
            group = new List<string>();

            for (int i = 0; i < groups.Count; i++)
            {
                for (int a = 0; a < Alphabets.Count; a++)
                {
                    for (int j = 0; j < groups[i].Count; j++)
                    {
                        for (int k = 0; k < Transitions.Count; k++)
                        {
                            if (Transitions[k][0] == groups[i][j] && (Transitions[k][1] == Alphabets[a]))
                            {
                                if (finalStates.Contains(groups[i][j]))
                                    isFinalState = true;
                                group.Add(Transitions[k][2]);

                            }
                        }
                    }
                    if (!CheckDuplicate(groups, group) && group.Count != 0)
                    {
                        List<string> result = new List<string>();
                        if (isFinalState)
                        {
                            result.Add($"*q{i}");
                            isFinalState = false;
                        }
                        else
                            result.Add($"q{i}");
                        result.Add(Alphabets[a]);
                        result.Add($"q{groups.Count}");
                        results.Add(result);
                        groups.Add(group);

                    }
                    else
                    {
                        List<string> result = new List<string>();
                        if (isFinalState)
                        {
                            result.Add($"*q{i}");
                            isFinalState = false;
                        }
                        else
                            result.Add($"q{i}");
                        result.Add(Alphabets[a]);
                        result.Add($"q{Duplicate(groups, group)}");
                        results.Add(result);
                    }
                    group = new List<string>();
                }
            }

            List<string> final = new List<string>();
            for (int i = 0; i < results.Count; i++)
            {
                if (results[i][0].Substring(0, 1) == "*" && !final.Contains(results[i][0]))
                    final.Add(results[i][0].Substring(1));
            }

            for (int i = 0; i < results.Count; i++)
            {
                if (final.Contains(results[i][2]))
                    results[i][2] = $"*{results[i][2]}";
            }
            //List<List<List<string>>> Q = new List<List<List<string>>>(groups.Count); 
            //for (int i = 0; i < groups.Count; i++)
            //{
            //    for (int a = 0; a < Alphabets.Count; a++)
            //    {
            //        List<string> list = new List<string>();
            //        List<string> list2 = new List<string>();
            //        List<List<string>> listlist = new List<List<string>>();
            //        for (int j = 0; j < groups[i].Count; j++)
            //        {
            //            for (int k = 0; k < Transitions.Count; k++)
            //            {
            //                if (Transitions[k][0] == groups[i][j] && Transitions[k][1] == Alphabets[a])
            //                {


            //                    list.Add(Transitions[k][2]);

            //                    list2.Add(Alphabets[a]);


            //                }
            //            }

            //          //  Q[i][0].Add(Alphabets[a]);

            //        }
            //        listlist.Add(list);
            //        listlist.Add(list2);
            //        Q.Add(listlist);
            //    }
            //}
            //for (int i = 0; i < groups.Count; i++)
            //{
            //    for (int j = 0; j < groups.Count; j++)
            //    {
            //        if (Q[i][0].All(groups[j].Contains) && groups[j].All(Q[i][0].Contains))
            //        {

            //            //Q[i][2] = new List<string>();
            //            //List<string> list = new List<string>();
            //            //list.Add($"q{j}");
            //            //Q[i].Add(list);
            //            Q[i][0][0] = $"q{j}";
            //        }

            //    }
            //}

            ////string[][] DFA = new string[GroupsNum][];
            //List<List<string>> DFA = new List<List<string>>();
            //for (int i = 0; i < groups.Count; i++)
            //{
            //    //DFA[i][0] = $"q{i}";
            //    //DFA[i][1] = Q[i][0][0];
            //    //DFA[i][2] = Q[i][1][0];
            //    List<string> list = new List<string>();
            //    list.Add($"q{i}");
            //    list.Add(Q[i][1][0]);
            //    list.Add(Q[i][0][0]);
            //    DFA.Add(list);
            //    //DFA[i].Add($"q{i}");
            //    //DFA[i].Add(Q[i][0][0]);
            //    //DFA[i].Add(Q[i][1][0]);
            //}

            Console.WriteLine($"{groups.Count}");
            for (int i = 0; i < Alphabets.Count; i++)
            {
                Console.Write($"{Alphabets[i]},");
            }
            Console.WriteLine();
            Console.WriteLine($"->{results[0][0]},{results[0][1]},{results[0][2]}");
            for (int i = 1; i < results.Count; i++)
            {
                Console.WriteLine($"{results[i][0]},{results[i][1]},{results[i][2]}");
            }
            return results;
        }

        private static bool CheckDuplicate(List<List<string>> groups, List<string> group)
        {
            for (int i = 0; i < groups.Count; i++)
            {
                if (groups[i].All(group.Contains) && group.All(groups[i].Contains))
                {
                    return true;
                }
            }
            return false;
        }

        private static int Duplicate(List<List<string>> groups, List<string> group)
        {
            for (int i = 0; i < groups.Count; i++)
            {
                if (groups[i].All(group.Contains) && group.All(groups[i].Contains))
                {
                    return i;
                }
            }
            return 0;
        }



        //--------------------------------------part 2-----------------------------------------------------

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
