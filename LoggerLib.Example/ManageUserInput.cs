using System;
using LoggerLib;

namespace LoggerLib.Example
{
    public class ManageUserInput
    {
        private readonly string messageToDisplay;

        public ManageUserInput(string input)
        {
            switch (input)
            {
                case "":
                    break;
                default:
                    break;
            }
        }

        public string GetMessageToDisplay()
        {
            return messageToDisplay;
        }
    }
}
