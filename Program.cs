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

    static void Main()
    {
        //LiveCoding();
        Sound();
    }


    static void Sound()
    {
        WaveProvider _wave = new WaveProvider(440);
        WaveProvider _secondWave = new WaveProvider(800);
        WaveOutEvent _firstOutput = new WaveOutEvent();
        WaveOutEvent _secondOutput = new WaveOutEvent();
        _firstOutput.Init(_wave);
        _firstOutput.Play();

        _secondOutput.Init(_secondWave);
        _secondOutput.Play();


        Console.ReadLine();
    }

    static void LiveCoding()
    {
        LiveContext _ctx = new LiveContext();

        LiveCodingSystem _liveCoding = new LiveCodingSystem(_ctx);
        //_liveCoding.Execute();
        AppDomain.CurrentDomain.ProcessExit += Exit;
        while (exit == false)
        {
            if (_ctx.MyObj != null)
                Console.WriteLine(_ctx.MyObj.ToString());
                //_ctx.MyObj.Update();

            else
                Console.WriteLine("Null");
            Thread.Sleep(1000);
        }
    }

    static void Exit(object _sender, EventArgs _e)
    {
        exit = true;
    }
}
