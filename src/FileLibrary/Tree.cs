using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Text;

namespace FileLibrary
{
    public class Tree
    {
        private List<Tree> _directories;
        private List<FileModel> _files;
        private string fullPath;
        private static StringBuilder treeStructure = new StringBuilder("");
        private static StringBuilder typeStructure = new StringBuilder("");
        private static StringBuilder timeStructure = new StringBuilder("");

        public Tree(string[] path)
        {
            if (path.Length == 0)
                throw new ArgumentException("Path cannot be null.");
            if (path.Length != 1)
                throw new ArgumentException("There cannot be more than one parameter.");
            if (!ValidatePath(path[0]))
            {
                throw new FormatException("Incorrect path format.");
            }
            try
            {
                DirectoryInfo di = new DirectoryInfo(path[0]);
                di.GetDirectories();
            }
            catch (PathTooLongException)
            {
                throw new PathTooLongException("Path is longer than the system-defined maximum length.");
            }
            catch (DirectoryNotFoundException)
            {
                throw new DirectoryNotFoundException("Directory does not exist.");
            }
            catch (UnauthorizedAccessException)
            {
                throw new UnauthorizedAccessException("Access to directory is denied.");
            }
            if (path[0][path[0].Length - 1] != Path.DirectorySeparatorChar)
                fullPath = path[0] + Path.DirectorySeparatorChar;
            else
                fullPath = path[0];
            _directories = new List<Tree>();
            _files = new List<FileModel>();
        }

        // Używany tylko w obrębie klasy przy tworzeniu drzewa
        private Tree(string path)
        {
            if (path == null)
                throw new ArgumentException("Path cannot be null.");
            if (path[path.Length - 1] != Path.DirectorySeparatorChar)
                fullPath = path + Path.DirectorySeparatorChar;
            else
                fullPath = path;
            _directories = new List<Tree>();
            _files = new List<FileModel>();
        }
        public string FullPath
        {
            get
            {
                return fullPath;
            }
        }

        public string DirectoryName
        {
            get
            {
                string[] temp = fullPath.Split(new char[] { Path.DirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);

                return temp[temp.Length - 1];
            }
        }

        public string treeString
        {
            get
            {
                return treeStructure.ToString();
            }
        }

        public List<Tree> Directories
        {
            get
            {
                return _directories;
            }
        }

        public List<FileModel> Files
        {
            get
            {
                return _files;
            }
        }

        public int GetFilesCount
        {
            get
            {
                return _files.Count;
            }
        }

        public int GetDirectoryCount
        {
            get
            {
                return _directories.Count;
            }
        }

        public void StartBuildTree()
        {
            BuildTree();
        }

        public void PrintTreeData()
        {
            InitializeData("", this);
            int measureName = MeasureName();
            string[] newLineChar = { Environment.NewLine, "\n" };
            string[] splitTree = treeStructure.ToString().Split(newLineChar, StringSplitOptions.RemoveEmptyEntries);
            string[] splitType = typeStructure.ToString().Split(newLineChar, StringSplitOptions.RemoveEmptyEntries);
            string[] splitTime = timeStructure.ToString().Split(newLineChar, StringSplitOptions.RemoveEmptyEntries);

            if (measureName <= 16)
            {
                ReplayCharInConsole("=", 54);
                Console.WriteLine();
                Console.Write("| Folder/File Name |    Type   |    Creation Time    |");
                Console.WriteLine();
                ReplayCharInConsole("=", 54);
                Console.WriteLine();
                for (int i = 0; i < splitTime.Length; i++)
                {
                    Console.Write("| " + splitTree[i]);
                    ReplayCharInConsole(" ", 16 - splitTree[i].Length);
                    Console.Write(" | ");
                    if (splitType[i] == "File")
                        Console.Write("  " + splitType[i] + "    | " + splitTime[i] + " |");
                    else
                        Console.Write(splitType[i] + " | " + splitTime[i] + " |");
                    Console.WriteLine();
                }

            }
            else
            {
                ReplayCharInConsole("=", measureName + 38);
                Console.WriteLine();
                Console.Write("|");
                ;
                ReplayCharInConsole(" ", Math.Ceiling(((double)measureName - 16) / 2));
                Console.Write(" Folder/File Name ");
                ReplayCharInConsole(" ", (measureName - 16) / 2);
                Console.Write("|    Type   |    Creation Time    |");
                Console.WriteLine();
                ReplayCharInConsole("=", measureName + 38);
                Console.WriteLine();
                for (int i = 0; i < splitTime.Length; i++)
                {
                    Console.Write("| " + splitTree[i]);
                    ReplayCharInConsole(" ", measureName - splitTree[i].Length);
                    Console.Write(" | ");
                    if (splitType[i] == "File")
                        Console.Write("  " + splitType[i] + "    | " + splitTime[i] + " |");
                    else
                        Console.Write(splitType[i] + " | " + splitTime[i] + " |");
                    Console.WriteLine();
                }
            }
        }

        private void InitializeData(string indent, Tree tree)
        {
            treeStructure.Append(indent + tree.DirectoryName);
            typeStructure.AppendLine("Directory");
            timeStructure.AppendLine((new DirectoryInfo(tree.fullPath).CreationTime.ToString()));
            treeStructure.AppendLine();
            indent += "   ";
            if (tree.Directories.Count != 0)
            {
                for (int i = 0; i < tree.Directories.Count; i++)
                {
                    InitializeData(indent, tree.Directories[i]);
                }
            }
            for (int j = 0; j < tree._files.Count; j++)
            {
                treeStructure.AppendLine(indent + tree._files[j].Name);
                typeStructure.AppendLine("File");
                timeStructure.AppendLine(tree._files[j].CreationTime.ToString());
            }
        }
        private void BuildTree(ref Tree tree)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(tree.fullPath);
                DirectoryInfo[] dirTable = dir.GetDirectories();
                FileInfo[] fileTable = dir.GetFiles();
                string[] files = FileService.GetFiles(tree.fullPath);
                foreach (string fi in files)
                {
                    tree._files.Add(FileService.FileMetadata(tree.fullPath + fi));
                }

                foreach (DirectoryInfo di in dirTable)
                {
                    Tree tempTree = new Tree(di.FullName);
                    tree.Directories.Add(tempTree);
                    BuildTree(ref tempTree);
                }
            }
            catch (UnauthorizedAccessException)
            {
                return;
            }
            catch (PathTooLongException)
            {
                return;
            }
        }
        private void BuildTree()
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(fullPath);
                DirectoryInfo[] dirTable = dir.GetDirectories();
                FileInfo[] fileTable = dir.GetFiles();

                foreach (DirectoryInfo di in dirTable)
                {

                    Tree tempTree = new Tree(di.FullName);
                    Directories.Add(tempTree);
                    BuildTree(ref tempTree);
                }
                string[] files = FileService.GetFiles(fullPath);
                foreach (string fi in files)
                {
                    _files.Add(FileService.FileMetadata(fullPath + fi));
                }
            }
            catch (UnauthorizedAccessException)
            {
                return;
            }
            catch (PathTooLongException)
            {
                return;
            }
        }
        private bool ValidatePath(string path)
        {
            // Walidacja ścieżki dla systemu Windows
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // Wyrażenie regularne uwzględnia tradycyjną ścieżkę (np. C:\) oraz przydział sieciowy lecz lokalizacja podawana jest z nazwą hosta, nie waliduje możliwości podania błędnego adresu IP (np. 256.256.256.256 będzie dla niego poprawnym adresem)
                Regex reg = new Regex(@"^(([a-zA-Z]:\\)|\\\\)([^\\\/:*?\<>|\r\n]+(\\)?)*$");
                return reg.IsMatch(path);
            }
            // Walidacja ścieżki dla systemu Linux
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Regex reg = new Regex(@"^(\/){1}([^\/\0]+(\/)?)*$");
                return reg.IsMatch(path);
            }
            // Walidacja ścieżki dla systemu OSX
            else
            {
                // Wyrażenie regularne uwzględnia typowy dla systemu Unix format ścieżki gdzie separatorem jest "/", nie uwzględnia tradycyjnej formy z separatorem ":" (wykorzystywanym do wersji OS X 8.1)
                Regex reg = new Regex(@"^(\/){1}([^\/\0:]+(\/)?)*$");
                return reg.IsMatch(path);
            }
        }

        private int MeasureName()
        {
            string[] newLineChar = { Environment.NewLine, "\n" };
            string[] temp = treeStructure.ToString().Split(newLineChar, StringSplitOptions.None);
            int measure = 0;
            foreach (string text in temp)
            {
                if (text.Length > measure)
                    measure = text.Length;
            }
            return measure;
        }

        private void ReplayCharInConsole(string character, double replay)
        {
            for (int i = 0; i < (int)replay; i++)
                Console.Write(character);
        }
    }
}
