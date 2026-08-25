using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

class Program
{
    static void Main()
    {
        HashSet<string> validCommands = new HashSet<string>(new[] { "echo", "exit", "type", "pwd", "cd"});

        while (true)
        {
            Console.Write("$ ");
            string input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input)) continue;

            string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string command = parts[0];
            string[] args = parts[1..];

            if (command == "exit")
            {
                break;
            }
            else if(command == "cd")
            {
                if(Directory.Exists(input[3..]))
                {
                    Directory.SetCurrentDirectory(input[3..]);
                }
                else
                {
                    Console.WriteLine($"cd: {input[3..]}: No such file or directory");
                }
                 
            }
            else if(command == "pwd")
            {
                
                Console.WriteLine(Directory.GetCurrentDirectory());
            }
            else if (command == "echo")
            {
                Console.WriteLine(string.Join(" ", args));
            }
            else if (command == "type")
            {
                if (args.Length > 0)
                {
                    string target = args[0];
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
            }
            else
            {
                string execPath = FindInPath(command);
                if (execPath != null)
                {
                    // Pass 'command' instead of 'execPath' to preserve Arg #0 as the original command name
                    ExecuteProgram(command, args);
                }
                else
                {
                    Console.WriteLine($"{command}: command not found");
                }
            }
        }
    }

    static string FindInPath(string command)
    {
        if (command.Contains('/'))
        {
            return (File.Exists(command) && IsExecutable(command)) ? command : null;
        }

        string pathEnv = Environment.GetEnvironmentVariable("PATH") ?? "";
        string[] directories = pathEnv.Split(':');

        foreach (string dir in directories)
        {
            string fullPath = Path.Combine(dir, command);
            if (File.Exists(fullPath) && IsExecutable(fullPath))
            {
                return fullPath;
            }
        }
        return null;
    }

    static bool IsExecutable(string filePath)
    {
        try
        {
            UnixFileMode mode = File.GetUnixFileMode(filePath);
            UnixFileMode execBits = UnixFileMode.UserExecute | UnixFileMode.GroupExecute | UnixFileMode.OtherExecute;
            return (mode & execBits) != 0;
        }
        catch
        {
            return false;
        }
    }

    static void ExecuteProgram(string command, string[] args)
    {
        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = command,
            UseShellExecute = false
        };

        foreach (string arg in args)
        {
            startInfo.ArgumentList.Add(arg);
        }

        using Process process = Process.Start(startInfo);
        process?.WaitForExit();
    }
}