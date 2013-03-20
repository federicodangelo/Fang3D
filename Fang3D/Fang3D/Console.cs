using System;
using System.Collections.Generic;
using System.Text;

namespace Fang3D
{
    static public class Console
    {
        public enum TextType
        {
            LOG,
            WARNING,
            ERROR
        }

        public delegate void ConsoleLineDelegate(String text, TextType textType);
        
        static public event ConsoleLineDelegate ConsoleLineEvent;

        static public void WriteLog(String text)
        {
            if (ConsoleLineEvent != null)
                ConsoleLineEvent(text, TextType.LOG);
        }

        static public void WriteError(String text)
        {
            if (ConsoleLineEvent != null)
                ConsoleLineEvent(text, TextType.ERROR);
        }

        static public void WriteWarning(String text)
        {
            if (ConsoleLineEvent != null)
                ConsoleLineEvent(text, TextType.WARNING);
        }
    }
}
