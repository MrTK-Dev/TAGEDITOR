using ID3_Tag_Editor.Scripts.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ID3_Tag_Editor.Scripts.UI;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows;
using ID3_Tag_Editor.Scripts.IO;

namespace ID3_Tag_Editor.Scripts.User
{

    [System.Serializable]
    public class LogMessage
    {
        public string Message { get; set; }

        public _Debug.Level Level { get; set; }

        public DateTime DateTime { get; set; }

        public _Debug.Tags[] Tags { get; set; }
}

    class Logger
    {
        readonly List<LogMessage> logMessages = new List<LogMessage>();

        private string FormatMessage(LogMessage Message)
        {
            string newMessage = $"[{Message.DateTime}|{Message.Level}]: {Message.Message}";

            return newMessage;
        }

        public void AddMessage(string message, _Debug.Level level, _Debug.Tags[] tags, DateTime dateTime)
        {
            LogMessage logMessage = new LogMessage()
            {
                Message = message,
                Level = level,
                Tags = tags,
                DateTime = dateTime
            };

            logMessages.Add(logMessage);

            string newMessage = FormatMessage(logMessages[0]);

            Debug.WriteLine(newMessage);

            FileSystem.AppendToFile(newMessage, Paths.GetFullPath(_Debug.PathToLogs));

            //https://stackoverflow.com/questions/13644114/how-can-i-access-a-control-in-wpf-from-another-class-or-window
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(MainWindow))
                {
                    if ((window as MainWindow).TB_Logger.Text == "")
                        (window as MainWindow).TB_Logger.Text = newMessage;

                    else
                        (window as MainWindow).TB_Logger.Text += "\n" + newMessage;
                }
            }
        }

        void ClearLogs()
        {
            logMessages.Clear();
        }

        void ExportLogs()
        {
            string destination = UI.OpenStuff.Folders.GetPathFromDialog("Pick the destination of your log file.", Paths.Defaults.Documents);
        }
    }
}
