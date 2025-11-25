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
    static WaveProvider wave = null;
    static void Main()
    {
        AppDomain.CurrentDomain.ProcessExit += Exit;
        Sound();
        wave.Patern.Add("C#6");
        wave.Patern.Add("~");
        wave.Patern.Add("F2");
        Console.WriteLine("Wave");

        while (exit == false)
        {
            Time.UpdateDeltime();
            wave.Update();
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
        wave = new WaveProvider(_freq);
        WaveOutEvent _firstOutput = new WaveOutEvent();
        _firstOutput.Init(wave);
        wave.SetVolume(0.2f);
        _firstOutput.Play();
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
