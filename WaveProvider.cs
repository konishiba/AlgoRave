using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class WaveProvider : WaveProvider32
{
    double phase = 0;
    public double frequency = 440, targetFrequency = 440;
    public float volume = 0.5f, targetVolume = 0.5f, tempVolume = 0.5f;
    List<string> patern = new List<string>();
    int currentIndexPatern = 0;

    double currentTime = 0.0f, cycleTime = 2.0f;

    bool playSilence = false;

    public List<string>Patern { get { return patern; } set { patern = value; } } 

    public WaveProvider (double _frequency)
    {
        targetFrequency = Math.Clamp(_frequency, 20, 18000);
    }

    public override int Read(float[] buffer, int offset, int count)
    {
        for (int i = 0; i < count; i++)
        {
            UpdateFrequency();
            UpdateVolume();

            float _sin = (float)Math.Sin(phase);
            buffer[offset + i] = volume * _sin;
            phase += 2f * MathF.PI * frequency / WaveFormat.SampleRate;

            if (phase > MathF.PI * 2)
                phase -= MathF.PI * 2;
        }
        Console.WriteLine("Target : " + targetFrequency.ToString());
        Console.WriteLine("freq : " + frequency.ToString());
        return count;
    }

    public void UpdateFrequency()
    {
        frequency += (targetFrequency - frequency) * 0.001f;
    }
    public void UpdateVolume()
    {
        volume += (targetVolume - volume) * 0.001f;
    }

    public void SetVolume(float _volume, ref WaveOutEvent _output)
    {
        _output.Volume = _volume / 1000;
    }

    public void SetVolume(float _volume)
    {
        targetVolume = _volume;
    }

    public void Update()
    {
        UpdateTimer();
    }

    public void ReadPatern()
    {
        if (patern.Count < 1) return;
        string _note = patern[currentIndexPatern];
        if (_note == "~")
        {
            tempVolume = targetVolume;
            targetVolume = 0.0f;
            playSilence = true;
            return;
        }
        else if(_note != "~" && playSilence == true)
        {
            targetVolume = tempVolume;
            playSilence = false;
        }
            NoteUtilites.NoteToFrequence(patern[currentIndexPatern], ref targetFrequency);
        Console.WriteLine(patern[currentIndexPatern]);
    }

    void UpdateTimer()
    {
        currentTime += Time.Deltatime;
        if (currentTime > cycleTime / patern.Count)
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

