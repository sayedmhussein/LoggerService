using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using SQLite;


namespace LoggerService
{
    public static class LoggerService
    {
        #region private attributes

        private static readonly Queue<LoggerModel> logs;

        private static readonly object locker = new object();
        private static readonly object fileLocker = new object();
        private static readonly object dbLocker = new object();

        private static string directoryPath;
        private static string dbPath;
        
        #endregion

        #region public config functions

        public enum LogType { Default, Info, Warn, Debug, Error, Fatal };

        static LoggerService()
        {
            if (string.IsNullOrEmpty(directoryPath))
                SetDefaultDirectoryPath();

            if (Directory.Exists(directoryPath) == false)
            {
                Directory.CreateDirectory(directoryPath);
            }

            if (logs == null)
                logs = new Queue<LoggerModel>();
        }

        /// <summary>
        /// Set the log directory path to default "{AppRoot}/Log/
        /// </summary>
        public static void SetDefaultDirectoryPath()
        {
            SetDirectoryPath("Log");
        }

        /// <summary>
        /// Set the directory path where the log files will be saved.
        /// </summary>
        /// <param name="path"></param>
        public static void SetDirectoryPath(string path)
        {
            Directory.CreateDirectory(path);
            string tempFilePath = Path.Combine(path, "temp.dat");

            try
            {
                File.Create(tempFilePath);
                File.Delete(tempFilePath);
                directoryPath = path;
                dbPath = Path.Combine(path, "log.db3");
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Used to write and flush the logs if not flushed.
        /// </summary>
        public static void Flush()
        {
            lock (locker)
            {
                if (logs.Count > 0)
                    InsertToDb(logs);

                while (logs.Count > 0)
                {
                    var log = logs.Peek();

                    if (log.MaskLogToFile == false)
                    {
                        AppendToFile(log);
                    }

                    logs.Dequeue();
                }
            }
        }

        /// <summary>
        /// Used to write and flush the logs if not flushed.
        /// </summary>
        public static async Task FlushAsync()
        {
            await Task.Run(() => Flush());
        }

        #endregion

        #region public main functions

        public static void Log(string logString, bool flush = true)
        {
            Log(LogType.Default, logString, flush);
        }

        public static void Log(LogType logType, string logString, bool flush = true)
        {
            var log = new LoggerModel()
            {
                TypeOfLog = logType,
                Argument = logString
            };

            lock (locker)
            {
                logs.Enqueue(log);
            }
            
            if (flush)
            {
                Flush();
            }
        }

        public static void Info(string logString, bool flush = true)
        {
            Log(LogType.Info, logString, flush);
        }

        public static void Warn(string logString, bool flush = true)
        {
            Log(LogType.Warn, logString, flush);
        }

        public static void Debug(string logString, bool flush = true)
        {
            Log(LogType.Debug, logString, flush);
        }

        public static void Error(string logString, bool flush = true)
        {
            Log(LogType.Error, logString, flush);
        }

        public static void Fatal(string logString, bool flush = true)
        {
            Log(LogType.Fatal, logString, flush);
        }

        #endregion

        #region public helper functions

        public static string GetLogDirectory()
        {
            return directoryPath;
        }

        public static List<(DateTime TimeStamp, string LineString)> GetLogList(LogType logType)
        {
            lock (dbLocker)
            {
                using SQLiteConnection conn = new SQLiteConnection(dbPath);
                conn.CreateTable<LoggerModel>();
                var list = conn.Table<LoggerModel>().Where(x => x.TypeOfLog == logType);

                var logList = new List<(DateTime, string)>();

                foreach (var item in list.ToList())
                {
                    logList.Add((item.Timestamp, item.Argument));
                }

                return logList;
            }
        }

        public static void DeleteLogs(LogType logType)
        {
            lock (dbLocker)
            {
                using SQLiteConnection conn = new SQLiteConnection(dbPath);
                conn.CreateTable<LoggerModel>();

                var list = conn.Table<LoggerModel>().Where(x => x.TypeOfLog == logType);
                foreach (var item in list.ToList())
                {
                    conn.Execute("DELETE FROM logtable WHERE type = " + (int)item.TypeOfLog);
                }
            }

            lock (fileLocker)
            {
                File.Delete(GetFilePath(logType));
            }
        }

        #endregion

        #region static private functions

        private static bool AppendToFile(LoggerModel log)
        {
            string line = log.Timestamp.ToString("yyyy-MM-dd hh:mm:ss K") +
                " -> " + log.Argument;

            string filePath = GetFilePath(log.TypeOfLog);

            try
            {
                lock (fileLocker)
                {
                    using StreamWriter sr = new StreamWriter(filePath, true);
                    sr.WriteLine(line);
                    sr.Flush();
                    return true;
                }
            }
            catch (Exception e)
            {
                throw e.GetBaseException();
            }
        }

        private static bool InsertToDb(Queue<LoggerModel> logList)
        {
            lock (dbLocker)
            {
                using SQLiteConnection conn = new SQLiteConnection(dbPath);
                conn.CreateTable<LoggerModel>();
                var count = conn.InsertAll(logList);
                if (count == 1)
                    return true;
                else
                    return false;
            }
        }

        private static string GetFilePath(LogType logType)
        {
            return Path.Combine(directoryPath, logType.ToString().ToLower() + ".log");
        }

        #endregion

    }
}
