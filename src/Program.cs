class Program
{
    static void Main()
    {

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
        else
        {
        Console.WriteLine($"{input}: command not found");
            

        }
        
        
    }
}
}
