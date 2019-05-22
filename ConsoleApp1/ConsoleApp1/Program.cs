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
        static void Main(string[] args)
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
            for (int i = 0; Input.ReadLine()!=null; i++)
            {
               //Input2.WriteLine("asd");
                Transitions.Add(Transition);
                Transition = Input.ReadLine().Split(',').ToList();
            }

           // for(int i=0;i<Transitions[1].Count;i++)
           //   Console.WriteLine($"{Transitions[1][i]}");

            NFAToDFA(Transitions, States, Alphabets);
        }

        static List<List<string>> NFAToDFA(List<List<string>> Transitions, int States, List<string> Alphabets)
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

            for(int i = 0; i < DFA.Count; i++)
            {
                Console.WriteLine($"{DFA[i][0]},{DFA[i][1]},{DFA[i][2]}");
            }
                return DFA;
            }
        }
}
