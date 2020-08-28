//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Threading;
//using System.Threading.Tasks;
//using LoggerLib;
//using Xunit;

//namespace LoggerLibTests
//{
//	public class LoggerClass_OnThreads
//	{
//		private readonly string testingDirectory = Path.Combine(
//				Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
//				"TemporaryLogFolder");

//		//[Fact]
//		//public async void TestLogTaskAwaited()
//		//{
//		//	var logger = new Logger(testingDirectory);
//		//	logger.DeleteAllLogs();

//		//	await Task.Run(() => {
//		//		new Logger(testingDirectory).Info("1 " + Task.CurrentId);
//		//		new Logger(testingDirectory).Info("2 " + Task.CurrentId);
//		//		new Logger(testingDirectory).Info("3 " + Task.CurrentId);
//		//	});
//		//	await Task.Run(() => {
//		//		new Logger(testingDirectory).Info("4 " + Task.CurrentId);
//		//		new Logger(testingDirectory).Info("5 " + Task.CurrentId);
//		//		new Logger(testingDirectory).Info("6 " + Task.CurrentId);
//		//	});


//		//	StreamReader sr = new StreamReader(Path.Combine(new Logger(testingDirectory).LogDirectory, "app.log"));
//		//	List<string> lines = new List<string>();
//		//	string line;
//		//	while ((line = sr.ReadLine()) != null)
//		//	{
//		//		lines.Add(line);
//		//	}

//		//	Assert.Equal(6, lines.Count);
//		//}

//		[Fact]
//		public void TestLogThread()
//		{
//			new LoggerService(testingDirectory).DeleteAllLogs();

//			Thread t1 = new Thread(() => {
//				new LoggerService(testingDirectory).Info("1 " + Thread.CurrentThread.Name);
//				new LoggerService(testingDirectory).Info("2 " + Thread.CurrentThread.Name);
//				new LoggerService(testingDirectory).Info("3 " + Thread.CurrentThread.Name);
//			});

//			Thread t2 = new Thread(() => {
//				new LoggerService(testingDirectory).Info("4 " + Thread.CurrentThread.Name);
//				new LoggerService(testingDirectory).Info("5 " + Thread.CurrentThread.Name);
//				new LoggerService(testingDirectory).Info("6 " + Thread.CurrentThread.Name);
//			});

//			t1.Start();
//			t2.Start();

//			t1.Join();
//			t2.Join();

//			StreamReader sr = new StreamReader(Path.Combine(new LoggerService(testingDirectory).LogDirectory, "app.log"));
//			List<string> lines = new List<string>();
//			string line;
//			while ((line = sr.ReadLine()) != null)
//			{
//				lines.Add(line);
//			}

//			Assert.Equal(6, lines.Count);
//		}


//		[Fact]
//		public async void TestLogTaskMix()
//		{
//			new LoggerService(testingDirectory).DeleteAllLogs();

//			new LoggerService(testingDirectory).Debug("1" + Thread.CurrentThread.Name);

//			await new LoggerService(testingDirectory).DebugAsync("2" + Thread.CurrentThread.Name);

//			await Task.Run(() => {
//				new LoggerService(testingDirectory).Info("3 " + Task.CurrentId);
//			});

//			await Task.Run(() => {
//				new LoggerService(testingDirectory).Info("4 " + Task.CurrentId);
//			});

//			Thread t1 = new Thread(() => {
//				new LoggerService(testingDirectory).Info("5 " + Thread.CurrentThread.Name);
//			});

//			Thread t2 = new Thread(() => {
//				new LoggerService(testingDirectory).Info("6 " + Thread.CurrentThread.Name);
//			});

//			t1.Start();
//			t2.Start();

//			t1.Join();
//			t2.Join();

//			StreamReader sr = new StreamReader(Path.Combine(new LoggerService(testingDirectory).LogDirectory, "app.log"));
//			List<string> lines = new List<string>();
//			string line;
//			while ((line = sr.ReadLine()) != null)
//			{
//				lines.Add(line);
//			}

//			Assert.Equal(6, lines.Count);
//		}

//	}
//}
