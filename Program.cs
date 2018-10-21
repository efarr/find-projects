using System;
using System.IO;
using System.Linq;

namespace find_projects
{
    class Program
    {
        static void Main(string[] args)
        {
            string parentDir = args.Length > 0 ? args[0] : Directory.GetCurrentDirectory();
            ProjectFinder finder = new ProjectFinder(parentDir);

            Node root = finder.GetProjectTree();
            PrintTree(root, "", true);
        }

        // Print with Markdown formatting
        static void PrintLine(Node node, int indent)
        {
            if (node.IsLeaf())
                Console.WriteLine($"- {node.Name}");
            else
            {
                Console.WriteLine($"\r\n{new string('#', indent)} {node.Name}");
                foreach (Node child in node.Children.Values)
                {
                    PrintLine(child, indent+1);
                }
            }
        }

        public static void PrintTree(Node tree, string indent, bool last)
        {
            Console.WriteLine(indent + "+- " + tree.Name);
            indent += last ? "   " : "|  ";

            for (int i = 0; i < tree.Children.Count; i++)
            {
                PrintTree(tree.Children.Values.ToArray()[i], indent, i == tree.Children.Count - 1);
            }
        }
    }
}
