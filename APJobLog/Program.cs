using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APJobLog
{
    class Program
    {
        
        public static void Main(string[] args)
        {

            bool logMessages = false;
            bool logErrors = false;
            bool logWarnings = true;

            bool logToFile = true;
            bool logToConsole = false;
            bool logToDatabase = false;

            JobLogger.InitLogger(logToFile, logToConsole, logToDatabase, logMessages, logWarnings, logErrors);
            JobLogger objJobLogger = new JobLogger();
            objJobLogger.LogMessage("Hola mundo",JobLogger.LogLevel.Warning);
            Console.ReadKey();
        }
    }
}

