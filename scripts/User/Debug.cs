using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ID3_Tag_Editor.Scripts.User
{
    public static class _Debug
    {
        public static void Log(string message, Level level, bool isUser)
        {
            Log(message, level, new Tags[] { }, isUser);
        }

        public static void Log(string message, Level level, Tags[] tags, bool isUser)
        {
            Logger logger = new Logger();

            logger.AddMessage(message, level, tags, DateTime.Now);
        }

        public enum Level
        {
            INFO,
            WARNING,
            ERROR
        }

        public enum Tags
        {
            FILES,
            AUTOMATION,
            GUI
        }

        public static readonly string PathToLogs = @"User/Logs.log";
    }
}
