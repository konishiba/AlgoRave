using Microsoft.VisualBasic;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

public enum VerbosityType
{
        VeryVerbose,    // Prints a verbose message to a log file (if veryVerboseLogging is enabled).
        Verbose,        // Prints a verbose message to a log file (if verboseLogging is enabled).
        Log,            // Prints a message to a log file (does not print to console).
        Display,        // Prints a message to console (and log file).
        Warning,        // Prints a warning to console (and log file).
        Error,          // Prints an error to console (and log file).
        Fatal,          // Always prints a fatal error to console (and log file) and throw an exception.

        COUNT
};

public class VerbosityData
{
    Color color = null;
    string prefix = "Unknown", text = "Unknown", debug = "Unknown";
    bool useDebug = false;

    public VerbosityData()
    {

    }

    public VerbosityData(VerbosityType _type, string _text, string _debug, bool _useDebug)
    {
        ComputeUseDebug(_type);
        ComputeColor(_type);
        prefix = Enum.GetName<VerbosityType>(_type);
        text = _text;
        debug = _debug;
        useDebug = _useDebug;
    }

    public string RetrieveFullText(bool _useColor,bool _useTime = true)
    {
        string _fullText = null;

        if (_useTime)
        {
            _fullText += "[" + Time.GetCurrentRealTime() + "]";
        }

        _fullText += " " + prefix + ": " + text;

        if (useDebug)
        {
          _fullText += " " + debug;
        }

        return _useColor ? color.ToString() + _fullText + Color.White.ToString() : _fullText;
    }

    void ComputeUseDebug(VerbosityType _type)
    {
        useDebug = new SortedSet<VerbosityType>{ VerbosityType.Error}.Contains(_type);
    }

    void ComputeColor(VerbosityType _type)
    {
        if (_type >= VerbosityType.COUNT) return;

        List<Color> _verbosityColors = new List<Color>
        {
            Color.Green,         //VERY VERBOSE
            Color.Blue,          //VERBOSE
            Color.White,         //LOG
            Color.Cyan,          //DISPLAY
            Color.Yellow,        //WARNING
            Color.Magenta,       //ERROR
            Color.Red,           //FATAL
        };

        color = _verbosityColors[Enum.GetValues<VerbosityType>().ToList().IndexOf(_type)];
    }
}

public static class logger
{
    static bool running = false;

    static readonly Queue<string> logQueue = new();
    static readonly Queue<string> consoleQueue = new();

    static readonly object queueMutex = new();
    static readonly AutoResetEvent cv = new(false);

    static Thread logThread;

    public static bool verboseLogging = false;
    public static bool veryVerboseLogging = false;

    static readonly string logsDir = "logs";
    static readonly string logsPath = Path.Combine(logsDir, "log.txt");

    public static void Init()
    {
        Reset();
        running = true;
        logThread = new Thread(LoggingThread);
        logThread.Start();
    }

    public static void Shutdown()
    {
        running = false;
        cv.Set();

        if (logThread != null && logThread.IsAlive)
            logThread.Join();
    }

    public static void Reset()
    {
        Directory.CreateDirectory(logsDir);
        File.WriteAllText(logsPath, string.Empty);
    }

    static void LoggingThread()
    {
        Directory.CreateDirectory(logsDir);

        using StreamWriter file = new(logsPath, append: true);

        while (running || logQueue.Count > 0 || consoleQueue.Count > 0)
        {
            cv.WaitOne();

            lock (queueMutex)
            {
                while (logQueue.Count > 0 || consoleQueue.Count > 0)
                {
                    if (consoleQueue.Count > 0)
                    {
                        string consoleText = consoleQueue.Dequeue();
                        Monitor.Exit(queueMutex);
                        Console.WriteLine(consoleText);
                        Monitor.Enter(queueMutex);
                    }

                    if (logQueue.Count > 0)
                    {
                        string logText = logQueue.Dequeue();
                        Monitor.Exit(queueMutex);
                        file.WriteLine(logText);
                        file.Flush();
                        Monitor.Enter(queueMutex);
                    }
                }
            }
        }
    }

    static void EnqueueLog(string text)
    {
        lock (queueMutex)
            logQueue.Enqueue(text);

        cv.Set();
    }

    static void EnqueueConsole(string text)
    {
        lock (queueMutex)
            consoleQueue.Enqueue(text);

        cv.Set();
    }

    public static bool CanPrintInLog(VerbosityType type)
    {
        if (type >= VerbosityType.Log) return true;

        if (verboseLogging)
        {
            if (type >= VerbosityType.Verbose) return true;
            if (veryVerboseLogging && type >= VerbosityType.VeryVerbose)
                return true;
        }

        return false;
    }

    public static bool CanPrintInConsole(VerbosityType type)
    {
        return type > VerbosityType.Log;
    }

    public static string GetDebugInfo([CallerFilePath] string file = "",
                                      [CallerMemberName] string member = "",
                                      [CallerLineNumber] int line = 0)
    {
        return $"(File: {file} | Func: {member} | Line: {line})";
    }

    public static void PrintLog(VerbosityType type, string text, string debug = "")
    {
        if (!running)
            Console.WriteLine("You printed a log but the Logger is not running (call Logger.Init())");

        if (CanPrintInLog(type))
        {
            bool _useDebug = debug != "";
            VerbosityData v = new(type, text, debug, _useDebug);
            EnqueueLog(v.RetrieveFullText(false));

            if (CanPrintInConsole(type))
                EnqueueConsole(v.RetrieveFullText(true));
        }

        if (type == VerbosityType.Fatal)
            throw new Exception("Fatal exception occurred");
    }

    public static void PrintLog(VerbosityType type, object obj, string debug = "")
    {
        if (obj == null)
        {
            PrintLog(VerbosityType.Error, "You tried to print a null IPrintable pointer");
            return;
        }

        PrintLog(type, obj.ToString(), debug);
    }
}

