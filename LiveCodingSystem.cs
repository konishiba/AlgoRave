using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


public class LiveCodingSystem
{
    public Action OnReloaded = null;

    LiveContext ctx = new LiveContext();
    string scriptPath = Path.GetFullPath("script.csx");

    public LiveCodingSystem()
    {
        Init();
    }

    public LiveCodingSystem(LiveContext _ctx)
    {
        ctx = _ctx;
        Init();
    }

    async void Init()
    {
        if (!File.Exists(scriptPath))
            File.WriteAllText(scriptPath, "Value = 20;");

        Console.WriteLine("Compilation initiale...");
        await ExecuteScript(scriptPath);

        WatchFile();
    }

    void WatchFile()
    {
        FileSystemWatcher _watcher = new FileSystemWatcher(
        Path.GetDirectoryName(scriptPath),
        Path.GetFileName(scriptPath));

        _watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size | NotifyFilters.FileName;

        //watcher.Changed += async (s, e) => await Reload(scriptPath, ctx);
        _watcher.Renamed += async (s, e) => await Reload(scriptPath);

        _watcher.EnableRaisingEvents = true;

        Console.WriteLine("\nProgramme en cours. Modifie script.csx.");
    }
    async Task Reload(string _scriptPath)
    {
        if (ctx == null) return;
        Thread.Sleep(80);
        Console.WriteLine("\n== Modification détectée ==");
        await ExecuteScript(_scriptPath);
        OnReloaded?.Invoke();

    }

    async Task ExecuteScript(string _path)
    {
        try
        {
            string _code = File.ReadAllText(_path);

            ScriptOptions _options = ScriptOptions.Default
                .WithImports("System")
                .WithReferences(
                    typeof(WaveProvider).Assembly, typeof(ESampleRate).Assembly, typeof(ESampleRate).Assembly
                );

            await CSharpScript.RunAsync(
                _code,
                globals: ctx,
                globalsType: typeof(LiveContext),
                options: _options
            );
        }
        catch (Exception _ex)
        {
            Console.WriteLine("Erreur script : " + _ex.Message);
        }
    }
    //public void Execute()
    //{
    //    WatchFile();
    //    while (true)
    //    {
    //        Console.WriteLine($"Affichage → Message: {ctx.Message} | Value: {ctx.Value}");
    //        Thread.Sleep(1000);
    //    }
    //}
}

