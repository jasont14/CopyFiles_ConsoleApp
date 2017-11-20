using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SimpleFileCopy
{
    class LogOperations
    {
        private string _filePath;
        private List<string> ErrorMessages;
        private List<string> LogMessages;
        private StringBuilder SB;
        private StringBuilder SBError;

        public int ErrorMessageCount 
        { 
            get
            {
                return ErrorMessages.Count;
            }
        }

        public int LogMessageCount 
        {
            get
            {
                return LogMessages.Count;
            }
        }
        public LogOperations()
        {
            ErrorMessages = new List<string>();
            LogMessages = new List<string>();
            SB = new StringBuilder();
            SBError = new StringBuilder();
                        
            SB.AppendLine(DateTime.UtcNow.ToString());
            SBError.AppendLine(DateTime.UtcNow.ToString());

            _filePath = Path.GetFullPath("Log\\");

            if(!Directory.Exists(_filePath))
            {
                try
                {
                    Directory.CreateDirectory(_filePath);
                }
                catch (UnauthorizedAccessException ex)
                {
                    Console.WriteLine("When trying to create the Log Directory the following error message was thrown: " + ex.Message);
                }                
            }
        }

        public void WriteToLogFile(string LogMessage)
        {
            SB.AppendLine(LogMessage);
        }

        public void WriteToErrorLog(string ErrorMessage)
        {
            SBError.AppendLine(ErrorMessage);
        }

        public void WriteLogs()
        {
            using (StreamWriter writer = new StreamWriter(_filePath + "Log.txt", true))
            {
                writer.WriteLine(SB.ToString());               
            }

            using (StreamWriter writer = new StreamWriter(_filePath + "ErrorLog.txt", true))
            {
                writer.WriteLine(SBError.ToString());
            }
        }
    }
}
