using System;
using System.IO;

namespace find_projects
{
    class Program
    {
        static void Main(string[] args)
        {
            string parentDir = args.Length > 0 ? args[0] : Directory.GetCurrentDirectory();
            ProjectFinder finder = new ProjectFinder(parentDir);

            Node root = finder.GetProjectTree();
            foreach (Node node in root.Children.Values)
            {
                PrintLine(node, 1);
            }
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
    }
}
