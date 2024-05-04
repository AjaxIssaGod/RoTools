using APIService;
using RobloxObjects;
using RoTools;

class Program
{
    internal static Interpreter Interpreter = new Interpreter();

    public static async Task Main(string[] args)
    {
        while (true)
        {
            string? input = Console.ReadLine();
            await Interpreter.RcvCommand(input);
        }
    }
}
