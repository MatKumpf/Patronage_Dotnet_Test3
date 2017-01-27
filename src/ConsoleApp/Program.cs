using FileLibrary;
using System;

namespace ConsoleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Tree tree = new Tree(args);
            tree.StartBuildTree();
            tree.PrintTreeData();
            Console.ReadKey();
        }
    }
}
