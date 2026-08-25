using System.Collections.Generic;

class Program
{
    static void Main()
    {
        HashSet<string> validCommands = new HashSet<string>(new[] { "echo", "exit", "type" });

        while (true)
        {

        Console.Write("$ ");
        string input = Console.ReadLine();

        if(input == "exit")
        {
            break;
        }
        else if(input.StartsWith("echo "))
        {
            Console.WriteLine(input[5..]);
        }
        else if(input.StartsWith("type "))
        {
            string filePath = input[5..];
            if(validCommands.Contains(filePath))
            {
                Console.WriteLine($"{filePath} is a shell builtin");
            }
            else
            {
                Console.WriteLine($"{filePath}: not found");
            }
        }
        else
        {
        Console.WriteLine($"{input}: command not found");
            

        }
        
        
    }
}
}
