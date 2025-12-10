using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using NAudio.Wave;
using System;
using System.IO;
using System.Threading;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;


class Program
{
    static bool exit = false;
    static Note wave = null;
    static WaveOutEvent firstOutput = null;
    static void Main()
    {
        AppDomain.CurrentDomain.ProcessExit += Exit;
        Sound();
        //wave.Patern.Add("C#6");
        //wave.Patern.Add("~");
        //wave.Patern.Add("F2");
        //Console.WriteLine("Wave");
        while (exit == false)
        {
            //Time.UpdateDeltime();
            Console.WriteLine(Time.Deltatime);
            if (wave == null) continue;
            //wave.Update();
            //Console.WriteLine("Wave"); 
        }

        //LiveCoding();

        //Console.WriteLine(NoteUtilites.NoteToFrequence("C4"));

        //Sound();
    }


    static void Sound()
    {
        double _freq = 277.6;
        NoteUtilites.NoteToFrequence("C#6", ref _freq);
        wave = new Note(_freq, 1.0f, 2.0f, new ADSR(2.0f,1.0f, 0.7f, 5.0f));
        firstOutput = new WaveOutEvent();
        firstOutput.Init(wave);
        //firstOutput.Volume = 0.2f;
        firstOutput.Play(); 
        wave.ADSR.OnReleaseFinished += RemoveWAve;
        wave.Start();

        BufferedWaveProvider _bufferedWaveProvider = new BufferedWaveProvider(new WaveFormat());
        //_bufferedWaveProvider.

        // //wave = new WaveProvider(_freq);
        // WaveOutEvent _firstOutput = new WaveOutEvent();
        // _firstOutput.Init(wave);
        //// wave.SetAmplitude(0.2f);
        // _firstOutput.Play();


        // //TODO le mettre au moment ou l'on créé la note
        // wave.IsFinished += () => { _firstOutput.Stop(); _firstOutput.Dispose(); };

    }

    static void RemoveWAve()
    {
        firstOutput.Dispose();
        wave = null;
    }

    static void LiveCoding()
    {
        LiveContext _ctx = new LiveContext();

        LiveCodingSystem _liveCoding = new LiveCodingSystem(_ctx);
        _liveCoding.OnReloaded += _ctx.Refresh;
        _ctx.Start();
        while (exit == false)
        {
        }
        //_liveCoding.Execute();
        //while (exit == false)
        //{
        //    if (_ctx.MyObj != null)
        //        Console.WriteLine(_ctx.MyObj.ToString());
        //        //_ctx.MyObj.Update();

        //    else
        //        Console.WriteLine("Null");
        //    Thread.Sleep(1000);
        //}
    }

    static void Exit(object _sender, EventArgs _e)
    {
        exit = true;
    }
}
