using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using NAudio.Wave;

public class LiveContext
{
    public WaveProvider? wave = null;
    public WaveOutEvent output = new WaveOutEvent();

    public void Start()
    {
        if (wave == null) return;
        output.Init(wave);
        output.Play();
    }

    public void Refresh()
    {
        Console.WriteLine("Refresh");
        output.Stop();
        output.Dispose();
        output.Init(wave);
        output.Play();
    }
}
