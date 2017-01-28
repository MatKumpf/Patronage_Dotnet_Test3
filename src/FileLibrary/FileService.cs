using System;
using System.IO;

namespace FileLibrary
{
    public class FileService
    {
        public static FileModel FileMetadata(string path)
        {
            FileModel file = new FileModel();
            FileInfo fi = null;
            try
            {
                fi = new FileInfo(path);
                // przypisanie do zmiennej temp wykonujemy z uwagi na pliki które mimo tego że są nieosiągalne (ale istnieją) to jest dla nich tworzony obiekt FileInfo,
                // dane których nie da się odczytać zawierają UnauthorizedAccessException
                var temp = fi.IsReadOnly;
            }
            catch (UnauthorizedAccessException)
            {
                throw new UnauthorizedAccessException("Access to file is denied.");
            }
            if (!fi.Exists)
                throw new FileNotFoundException("File does not exist.");
            file.Name = fi.Name;
            file.FullName = fi.FullName;
            file.CreationTime = fi.CreationTime;
            file.Extension = fi.Extension;
            file.Length = (ulong)fi.Length;

            return file;
        }

        public static string[] GetFiles(string path)
        {
            try
            {
                FileInfo[] fi = (new DirectoryInfo(path)).GetFiles();
                string[] files = new string[fi.Length];
                for (int i = 0; i < fi.Length; i++)
                    files[i] = fi[i].Name;
                return files;
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
        }
    }
}
