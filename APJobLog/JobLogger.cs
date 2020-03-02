using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;

namespace APJobLog
{
    public class JobLogger
    {
        private static bool LogToFile;
        private static bool LogToConsole;
        private static bool LogToDataBase;
        private static bool LogMessages;
        private static bool LogWarnings;
        private static bool LogErrors;

        private static bool Initialized = false;

        private static System.Data.SqlClient.SqlConnection _connection = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

        public enum LogLevel
        {
            Message,
            Warning,
            Error
        }

        public static void InitLogger(bool logToFile, bool logToConsole, bool logToDatabase, bool logMessages, bool logWarning, bool logErrors)
        {
            if (Initialized)
            {
                throw new Exception("Logger already initialied");
            }

            LogErrors = logErrors;
            LogMessages = logMessages;
            LogWarnings = logWarning;
            LogToDataBase = logToDatabase;
            LogToFile = logToFile;
            LogToConsole = logToConsole;

            if (!LogToConsole && !LogToFile && !LogToDataBase)
            {
                throw new Exception("Invalid configuration");
            }

            if (!LogMessages && !LogWarnings && !LogErrors)
            {
                throw new Exception("Error or Warning or Message must be specified");
            }

            Initialized = true;
        }

        public void LogMessage(string message, LogLevel level)
        {
            message.Trim();

            if (message == null || message.Length == 0)
            {
                return;
            }
            if ((!LogMessages && level.Equals(LogLevel.Message)) ||
               (!LogWarnings && level.Equals(LogLevel.Warning)) ||
               (!LogErrors && level.Equals(LogLevel.Error)))
            {
                return;
            }

            if (LogToDataBase)
            {
                LogMessageInDataBase(message, level);
            }

            if (LogToFile)
            {
                LogMessageInFile(message, level);
            }

            if (LogToConsole)
            {
                LogMessageInConsole(message, level);
            }
        }

        private void LogMessageInFile(string message, LogLevel level)
        {
            string currentFileContent = string.Empty;

            if (System.IO.File.Exists(System.Configuration.ConfigurationManager.AppSettings["LogFileDirectory"] + "LogFile" + DateTime.Now.ToString("ddmmyyyy") + ".txt"))
            {
                currentFileContent = System.IO.File.ReadAllText(System.Configuration.ConfigurationManager.AppSettings["LogFileDirectory"] + "LogFile" + DateTime.Now.ToString("ddmmyyyy") + ".txt");
            }

            currentFileContent += string.Format("{0} [{1}] - {2} : {3}", Environment.NewLine, level.ToString(), DateTime.Now.ToShortDateString(), message);

            System.IO.File.WriteAllText(System.Configuration.ConfigurationManager.AppSettings["LogFileDirectory"] + "LogFile" + DateTime.Now.ToString("dd-mm-yyyy") + ".txt", currentFileContent);
        }

        private void LogMessageInDataBase(string message, LogLevel level)
        {
            _connection.Open();
            int levelId = 0;
            switch (level)
            {
                case LogLevel.Error:
                    levelId = 1;
                    break;
                case LogLevel.Warning:
                    levelId = 2;
                    break;
                case LogLevel.Message:
                    levelId = 3;
                    break;
            }

            string sqlCommandString = string.Format("Insert into Log Values('{0}',{1})", message, levelId);
            System.Data.SqlClient.SqlCommand command = new SqlCommand(sqlCommandString, _connection);
            command.ExecuteNonQuery();
            _connection.Close();
        }



        private void LogMessageInConsole(string message, LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogLevel.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogLevel.Message:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }

            Console.WriteLine(DateTime.Now.ToShortDateString() + message);
        }
    }


}
