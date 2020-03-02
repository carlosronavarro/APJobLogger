using System;
using APJobLog;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataTests
{
    [TestClass]
    public class JobLoggerTest
    {
        [TestMethod]
        public void LogMessageExceptionInvalidConfigurationTest()
        {
            bool logMessages = false;
            bool logErrors = false;
            bool logWarnings = true;

            bool logToFile = false;
            bool logToConsole = false;
            bool logToDatabase = false;

            try
            {
                JobLogger.InitLogger(logToFile, logToConsole, logToDatabase, logMessages, logWarnings, logErrors);
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.Message, "Invalid configuration");
            }
        }

        [TestMethod]
        public void LogMessageExceptionErrorWarningMessageMustSpecifiedTest()
        {
            bool logMessages = false;
            bool logErrors = false;
            bool logWarnings = false;

            bool logToFile = false;
            bool logToConsole = true;
            bool logToDatabase = false;

            try
            {
                JobLogger.InitLogger(logToFile, logToConsole, logToDatabase, logMessages, logWarnings, logErrors);
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.Message, "Error or Warning or Message must be specified");
            }
        }
    }
}
