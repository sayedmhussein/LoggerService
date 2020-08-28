using System;
using System.IO;
using Xunit;
using LoggerService;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LoggerService.Test
{
    public class LoggerConfigTesters
    {
        [Fact]
        public void SetDefaultDirectoryPath_CreateLogDirectoryIfNotExist()
        {
            string directoryName = "Log";

            LoggerService.SetDefaultDirectoryPath();

            Assert.True(Directory.Exists(directoryName));
        }

        [Fact]
        public void SetDirectoryPath_CreateLogDirectoryIfNotExist()
        {
            string directoryName = "Bla";

            LoggerService.SetDirectoryPath(directoryName);

            Assert.True(Directory.Exists(directoryName));
        }

        [Fact]
        public void SetDirectoryPath_WhenNullDirectoryString_ThrowArgumentNullException()
        {
            Action action = () => LoggerService.SetDirectoryPath(null);

            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void SetDirectoryPath_WhenEmptyDirectoryString_ThrowArgumentException()
        {
            Action action = () => LoggerService.SetDirectoryPath("");

            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void SetDirectoryPath_WhenInvalidDirectoryString_ThrowIOException()
        {
            Action action = () => LoggerService.SetDirectoryPath("/");
            LoggerService.Log("bla");

            Assert.Throws<IOException>(action);
        }

        [Fact]
        public void Flush_WhenCalledWithoutPendingLogs()
        {
            LoggerService.Flush();
        }

        [Fact]
        public void Flush_WhenCalledWithPendingLogs()
        {
            LoggerService.Log("test1", false);
            LoggerService.Log("test2", false);
            LoggerService.Flush();
        }

        [Fact]
        public void Flush_WhenCalledWithMixedPendingLogs()
        {
            LoggerService.Log("test1", true);
            LoggerService.Log("test2", false);
            LoggerService.Log("test3", true);
            LoggerService.Log("test4", true);
            LoggerService.Log("test5", false);
            LoggerService.Flush();
        }

        [Fact]
        async public void Flush_BulkTestByTasks()
        {
            List<Task> tasks = new List<Task>();

            for (int i = 0; i < 100; i++)
            {
                tasks.Add(Task.Run(() =>
                {
                    LoggerService.Log("I'm task id " + i.ToString(), false);
                }));
            }

            for (int i = 100; i <= 200; i++)
            {
                tasks.Add(Task.Run(() =>
                {
                    LoggerService.Log("I'm task id " + i.ToString());
                }));
            }

            for (int i = 200; i <= 300; i++)
            {
                tasks.Add(Task.Run(() =>
                {
                    LoggerService.Log("I'm task id " + i.ToString(), true);
                }));
            }

            await Task.WhenAll(tasks);

            await LoggerService.FlushAsync();
        }
        
    }
}
