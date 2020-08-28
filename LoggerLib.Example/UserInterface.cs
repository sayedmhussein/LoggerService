using System;
namespace LoggerLib.Example
{
    public static class UserInterface
    {
        static UserInterface()
        {
        }

        public static string GetWelcomeMessage()
        {
            return "Welcome to LoggerLib tester application.";
        }

        public static string GetExitMessage()
        {
            return "Thank you for using the tester.";
        }

        public static string GetInstructionMessage()
        {
            return "Thank you for using the tester.";
        }

        public static string GetExitArgument()
        {
            return "exit";
        }
    }
}
