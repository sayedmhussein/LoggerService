using System;
using Xunit;
using LoggerService;
using System.IO;

namespace LoggerService.Test
{
    public class LoggerMainFunctionsTesters
    {
        [Fact]
        public void LogService_SetDefaultFolder()
        {
            string path = "bla";

            LoggerService.SetDirectoryPath(path);

            bool exist = Directory.Exists(path);

            Assert.True(exist);

            LoggerService.SetDefaultDirectoryPath();
        }

        [Fact]
        public void Log_SimpleLogToDefaultFolder()
        {
            LoggerService.Log("This is test log argument");
            LoggerService.Log("This is another test log argument");
            LoggerService.Log("This is one more test log argument");
            LoggerService.Flush();
        }

        [Fact]
        public void Log_SimpleDebugToDefaultFolder()
        {
            LoggerService.Debug("This is test log argument", true);
            LoggerService.Debug("This is another test log argument", false);
            LoggerService.Debug("This is one more test log argument");
            LoggerService.Flush();
        }
    }
}
