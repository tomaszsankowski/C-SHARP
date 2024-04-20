using System;
using System.IO;

class Program
{
    static DateTime oldestFileDate = DateTime.Now;
    static string oldestFileName = string.Empty;
    static void Main(string[] args)
    {
        if (args.Length > 0)
        {
            string directoryPath = args[0];
            if (Directory.Exists(directoryPath))
            {
                DisplayDirectory(directoryPath, 0);
                Console.WriteLine($"\nThe oldest file '{oldestFileName}' was created on: {oldestFileDate}");
            }
            else
            {
                Console.WriteLine($"Directory {directoryPath} does not exist.");
            }
        }
        else
        {
            Console.WriteLine("No directory argument provided.");
        }
    }

    private static void DisplayDirectory(string directory, int tabs)
    {
        var dir = new System.IO.DirectoryInfo(directory);
        
        string[] files = Directory.GetFiles(directory);
        string[] directories = Directory.GetDirectories(directory);
        
        Console.WriteLine(new string(' ', 3*tabs) + Path.GetFileName(directory) + " (" + (files.Length+directories.Length) + ") " + GetDosAttributes(dir));
        tabs++;

        foreach (string file in files)
        {
            DateTime creationTime = File.GetCreationTime(file);
            if (creationTime < oldestFileDate)
            {
                oldestFileDate = creationTime;
                oldestFileName = Path.GetFileName(file);
            }
            FileInfo fileInfo = new FileInfo(file);
            Console.WriteLine(new string(' ',3*tabs) + Path.GetFileName(file) + " " + fileInfo.Length + " bytes " + GetDosAttributes(fileInfo));
        }

        foreach (string subDirectory in directories)
        {
            DisplayDirectory(subDirectory, tabs);
        }
    }
    
    private static string GetDosAttributes(FileSystemInfo fileSystemInfo)
    {
        var attributes = fileSystemInfo.Attributes;
        return $"{(attributes.HasFlag(FileAttributes.ReadOnly) ? 'r' : '-')}" +
               $"{(attributes.HasFlag(FileAttributes.Archive) ? 'a' : '-')}" +
               $"{(attributes.HasFlag(FileAttributes.Hidden) ? 'h' : '-')}" +
               $"{(attributes.HasFlag(FileAttributes.System) ? 's' : '-')}"; 
    }
}