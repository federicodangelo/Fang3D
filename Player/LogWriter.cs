using System.IO;
namespace Fang3D.Player
{
    public class LogWriter
    {
        static StreamWriter sw;

        static public void Init()
        {
            Console.ConsoleLineEvent += new Console.ConsoleLineDelegate(Console_ConsoleLineEvent);

            sw = new System.IO.StreamWriter(Path.Combine(Program.RunningPath, "fang3d.log"));
        }

        static void Console_ConsoleLineEvent(string text, Console.TextType textType)
        {
            switch (textType)
            {
                case Console.TextType.LOG:
                    sw.WriteLine(" - LOG: " + text);
                    break;

                case Console.TextType.WARNING:
                    sw.WriteLine(" - WARNING: " + text);
                    break;

                case Console.TextType.ERROR:
                    sw.WriteLine(" - ERROR: " + text);
                    break;
            }

            sw.Flush();
        }
    }
}
