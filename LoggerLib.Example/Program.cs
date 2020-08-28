using System;

namespace LoggerLib.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(UserInterface.GetWelcomeMessage());

            while (true)
            {
                Console.Write(UserInterface.GetInstructionMessage());
                var input = Console.ReadLine().ToLower();

                if (input.Equals(UserInterface.GetExitArgument()))
                    break;
                else
                {
                    var ms = new ManageUserInput(input);
                    Console.WriteLine(ms.GetMessageToDisplay());
                }
            }

            Console.WriteLine(UserInterface.GetExitMessage());
            Console.WriteLine("\n\nPress enter to exit");
            Console.ReadLine();
        }
    }
}
