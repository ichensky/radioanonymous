using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RadioAnonymous.Models;

namespace RadioAnonymous.ViewModels
{
    public class LogViewModel : BaseViewModel<LogViewModel>
    {
        #region Fields
        
        private LogMassage massage;

        private string description;

        private LogLevel level;

        private string logLable;

        public enum LogMassage
        {
            BadInternetConnection,
            AnotherMassage
        }

        public enum LogLevel
        {
            Off = 6,
            Fatal = 5,
            Error = 4,
            Warn = 3,
            Info = 2,
            Debug = 1,
            All = 0,
        }

        #endregion // Fields

        #region Properties
        
        public string Massage
        {
            get
            {
                if ((int)level >= 4)
                {
                    switch (massage)
                    {
                        case LogMassage.BadInternetConnection:
                            return "Check Internet connection. " + description;
                        case LogMassage.AnotherMassage:
                            return description;
                    }
                }

                return null;
            }
        }

        public string LogLable
        {
            get { return logLable; }
            set { logLable = value; RaisePropertyChanged(x => x.LogLable); }
        }


        #endregion // Properties

        #region Constructors

        protected LogViewModel() {
            //logLable = "log > ";
        }

        
        #endregion // Constructors

        private sealed class LogViewModelCreator
        {
            private static readonly LogViewModel instance = new LogViewModel();
            public static LogViewModel Instance { get { return instance; } }
        }

        #region Methods

        public static LogViewModel Instance
        {
            get { return LogViewModelCreator.Instance; }
        }

        public void Log(LogLevel logLevel, LogMassage logMessage)
        {
            this.level = logLevel;
            this.massage = logMessage;
            RaisePropertyChanged(x => x.Massage);
        }
        
        public void Log(LogLevel logLevel, LogMassage logMessage, string description)
        {
            this.description = description;
            Log(logLevel, logMessage);
        }

        #endregion // Methods

    }
}
