using System.Runtime.Serialization.Formatters.Binary;

namespace lab7;

internal static class Lab7
{
    static DateTime _oldestFileDate = DateTime.Now;
    static string _oldestFileName = string.Empty;
    static void Main(string[] args)
    {
        AppContext.SetSwitch("System.Runtime.Serialization.EnableUnsafeBinaryFormatterSerialization", true);
        if (args.Length > 0)
        {
            string directoryPath = args[0];
            if (Directory.Exists(directoryPath))
            {
                DisplayDirectory(directoryPath, 0);
                
                Console.WriteLine($"\nThe oldest file '{_oldestFileName}' was created on: {_oldestFileDate}\n");
                
                Serialize(GetFilesToCollection(directoryPath), "collection.bin");
                
                var collection = Deserialize("collection.bin");
                
                foreach (var item in collection)
                {
                    Console.WriteLine($"{item.Key} -> {item.Value}");
                }
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
        var dir = new DirectoryInfo(directory);
        
        string[] files = Directory.GetFiles(directory);
        string[] directories = Directory.GetDirectories(directory);
        
        Console.WriteLine(new string('\t', tabs) + Path.GetFileName(directory) + " (" + (files.Length+directories.Length) + ") " + GetDosAttributes(dir));
        tabs++;

        var filesAndDirectoriesList = new List<string>(files);
        filesAndDirectoriesList.AddRange(directories);
        
        filesAndDirectoriesList.Sort();
        
        foreach (string path in filesAndDirectoriesList)
        {
            if (File.Exists(path))
            {
                DateTime creationTime = File.GetCreationTime(path);
                if (creationTime < _oldestFileDate)
                {
                    _oldestFileDate = creationTime;
                    _oldestFileName = Path.GetFileName(path);
                }
                FileInfo fileInfo = new FileInfo(path);
                Console.WriteLine(new string('\t',tabs) + Path.GetFileName(path) + " " + fileInfo.Length + " bytes " + GetDosAttributes(fileInfo));
            }
            else if (Directory.Exists(path))
            {
                DisplayDirectory(path, tabs);
            }
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

    private static SortedDictionary<string, long> GetFilesToCollection(string root)
    {
        var sortedFiles = new SortedDictionary<string, long>(new DirectoriesComparer());

        if (Directory.Exists(root))
        {

            var directoryInfo = new DirectoryInfo(root);
        
            foreach (var file in directoryInfo.GetFiles())
            {
                sortedFiles.Add(file.Name, file.Length);
            }

            foreach (var directory in directoryInfo.GetDirectories())
            {
                sortedFiles.Add(directory.Name, directory.GetDirectories().Length + directory.GetFiles().Length);
            }

        }
        else
        {
            Console.WriteLine($"Directory {root} does not exist.");
        }
        return sortedFiles;
    }

    private static void Serialize(SortedDictionary<string, long> collection, string filePath)
    {
        using var stream = File.OpenWrite(filePath);
        var formatter = new BinaryFormatter();
        formatter.Serialize(stream, collection);
    }
    
    private static SortedDictionary<string, long> Deserialize(string filePath)
    {
        using var stream = File.OpenRead(filePath);
        var formatter = new BinaryFormatter();
        return (SortedDictionary<string, long>)formatter.Deserialize(stream);
    }
}

[Serializable]
class DirectoriesComparer : IComparer<string>
{
    public int Compare(string? x, string? y)
    {
        if (y != null && x != null)
        {
            int result = x.Length.CompareTo(y.Length);
            if (result == 0)
            {
                result = string.Compare(x, y, StringComparison.Ordinal);
            }

            return result;
        }
        
        throw new ArgumentNullException();
    }

}