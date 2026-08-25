using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        HashSet<string> validCommands = new HashSet<string>(new[] { "echo", "exit", "type" });

        while (true)
        {
            Console.Write("$ ");
            string input = Console.ReadLine();
            if (string.IsNullOrEmpty(input)) continue;

            if (input == "exit")
            {
                break;
            }
            else if (input.StartsWith("echo "))
            {
                Console.WriteLine(input[5..]);
            }
            else if (input.StartsWith("type "))
            {
                string target = input[5..];
                
                if (validCommands.Contains(target))
                {
                    Console.WriteLine($"{target} is a shell builtin");
                }
                else
                {
                    string execPath = FindInPath(target);
                    if (execPath != null)
                    {
                        Console.WriteLine($"{target} is {execPath}");
                    }
                    else
                    {
                        Console.WriteLine($"{target}: not found");
                    }
                }
            }
            else
            {
                Console.WriteLine($"{input}: command not found");
            }
        }
    }

    static string FindInPath(string command)
    {
        string pathEnv = Environment.GetEnvironmentVariable("PATH") ?? "";
        string[] directories = pathEnv.Split(':');

        foreach (string dir in directories)
        {
            string fullPath = Path.Combine(dir, command);
            if (File.Exists(fullPath))
            {
                UnixFileMode mode = File.GetUnixFileMode(fullPath);
                UnixFileMode execBits = UnixFileMode.UserExecute | UnixFileMode.GroupExecute | UnixFileMode.OtherExecute;
                
                if ((mode & execBits) != 0)
                {
                    return fullPath;
                }
            }
        }
        return null;
    }
}