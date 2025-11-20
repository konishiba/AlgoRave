using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class WaveProvider : WaveProvider32
{
    double phase = 0;
    public double frequency = 440;
    public float volume = 0.5f;
    List<string> patern = new List<string>();
    int currentIndexPatern = 0;

    double currentTime = 0.0f, cycleTime = 2.0f;

    public List<string>Patern { get { return patern; } set { patern = value; } } 

    public WaveProvider (double _frequency)
    {
        frequency = Math.Clamp(_frequency, 20, 18000);
    }

    public override int Read(float[] buffer, int offset, int count)
    {
        for (int i = 0; i < count; i++)
        {
            buffer[offset + i] = (float)Math.Sin(phase);
            phase += 2f * MathF.PI * frequency / WaveFormat.SampleRate;

            if (phase > MathF.PI * 2)
                phase -= MathF.PI * 2;
        }
        return count;
    }

    public void UpdateVolume(float _volume, ref WaveOutEvent _output)
    {
        _output.Volume = _volume / 1000;
    }

    public void Update()
    {
        UpdateTimer();
    }

    public void ReadPatern()
    {
        if (patern.Count < 1) return;
        NoteUtilites.NoteToFrequence(patern[currentIndexPatern], ref frequency);
        Console.WriteLine(patern[currentIndexPatern]);
    }

    void UpdateTimer()
    {
        currentTime += Time.Deltatime;
        if (currentTime > cycleTime)
        {
            IncrementIndexPatern();
            ReadPatern();
            currentTime = 0.0f;
        }
        //Console.WriteLine(currentTime);

    }

    void IncrementIndexPatern()
    {
        currentIndexPatern = currentIndexPatern + 1 > patern.Count - 1 ? 0 : currentIndexPatern + 1;
    }
}

