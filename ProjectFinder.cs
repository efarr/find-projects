using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace find_projects
{
    public class Node
    {
        public Node(string name)
        {
            Name = name;
            Children = new Dictionary<string, Node>();
        }

        public Node AddChild(string name)
        {
            if (!Children.ContainsKey(name))
                Children[name] = new Node(name);

            return Children[name];
        }

        public bool IsLeaf()
        {
            if (Children.Count > 1)
                return false;
            if (Children.Count == 0)
                return true;

            Node onlyChild = Children.First().Value;

            if (onlyChild.Children.Count == 0)
                return true;

            if (onlyChild.Children.Count > 1)
                return false;

            if (onlyChild.Children.Values.First().Children.Count > 0)
                return false;

            // If my only child name matches my name, it's most likely just superfluous nesting 
            return String.Equals(Name, onlyChild.Name, StringComparison.CurrentCultureIgnoreCase);
        }

        public string Name { get; set; }
        public Dictionary<string,Node> Children { get; set; }
    }

    public class ProjectFinder
    {
        private readonly string _parent;
        readonly List<string> _files = new List<string>();

        public ProjectFinder(string parent)
        {
            _parent = parent;
        }

        private void GetFilesRecursive(string dirPath)
        {
            try
            {
                foreach (string d in Directory.GetDirectories(dirPath))
                {
                    GetFilesRecursive(d);
                }
                foreach (var file in Directory.GetFiles(dirPath))
                {
                    if (IsProjectFile(file))
                        ProcessFile(file);
                }
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private bool IsProjectFile(string file)
        {
            return file.ToLower().EndsWith(".csproj");
        }

        private void ProcessFile(string file)
        {
            _files.Add(file.Substring(_parent.Length));
        }

        public Node GetProjectTree()
        {
            GetFilesRecursive(_parent);
            Node root = new Node("root");

            foreach (string file in _files)
            {
                Stack<string> stack = new Stack<string>(file.Split('\\').Reverse());
                AddFile(root, stack);
            }

            return root;
        }

        private void AddFile(Node parent, Stack<string> stack)
        {
            string name = stack.Pop();
            Node node = parent.AddChild(name);
            if (stack.Count > 0)
                AddFile(node, stack);
        }
    }
}