using System;
using SQLite;

namespace LoggerService
{
    [Table("logtable")]
    internal class LoggerModel
    {
        [PrimaryKey, AutoIncrement, Column("pk")]
        public long PrimaryKey { get; set; }

        [Ignore]
        public string TimestampString { get => Timestamp.ToString("yyyy-MM-dd hh:mm:ss K"); }

        [Column("ts")]
        public DateTime Timestamp { get => DateTime.Now; set { } }

        [Column("type")]
        public LoggerService.LogType TypeOfLog { get; set; }

        [Column("string")]
        public string Argument { get; set; }

        [Ignore]
        public bool MaskLogToFile { get; set; }

        [Ignore]
        public bool MaskLogToDb { get; set; }
    }
}
