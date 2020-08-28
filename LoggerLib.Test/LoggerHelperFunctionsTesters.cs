using System;
using Xunit;
using LoggerService;
using System.IO;

namespace LoggerLib.Test
{
    public class LoggerHelperFunctionsTesters
    {
        [Fact]
        public void GetLogDirectory_MatchPassedInConstructor()
        {
            string path = "TestDir";

            LoggerService.SetDirectoryPath(path);

            Assert.Equal(path, LoggerService.GetLogDirectory());
        }

        [Fact]
        public void GetLogList_WhenLogTypeIsNull_GetAllDefault()
        {
            LoggerService.Log("Hello World");

            var list = LoggerService.GetLogList(LoggerService.LogType.Default);

            Assert.NotEmpty(list);
        }

        [Fact]
        public void DeleteLogsTester()
        {
            LoggerService.DeleteLogs(LoggerService.LogType.Default);

            Assert.Empty(LoggerService.GetLogList(LoggerService.LogType.Default));
        }
    }
}
