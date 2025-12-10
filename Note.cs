using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


internal class Note : WaveProvider32
{
    public event Action IsFinished = null;

    double phase = 0;
    public double frequency = 440, targetFrequency = 440;
    public float currentAmplitude = 0.5f, amplitude = 0.5f;/*, tempAmplitude = 0.5f;*/
    float currentDuration = 0.0f,  duration = 0.0f;

    ADSR adrs = new ADSR();

    public ADSR ADSR => adrs;

    public Note(double _frequency, float _amplitude, float _duration, ADSR _adrs)
    {
        targetFrequency = _frequency;
        amplitude = _amplitude;
        duration = _duration;
        adrs = _adrs;
    }

    public void Start()
    {
        IsFinished += () => { adrs.currentState = ADSR_STATE.RELEASE_STATE; };
        adrs.Start();
    }

    public void Update()
    {
        adrs.Update(GetDeltaSampleRate(),ref currentAmplitude, amplitude);
        Console.WriteLine(currentAmplitude);
        currentDuration += (float)Time.Deltatime;
        if(currentDuration > duration)
        {
            IsFinished?.Invoke();
        }
    }

    float GetDeltaSampleRate()
    {
        return 1.0f / WaveFormat.SampleRate;
    }

    public override int Read(float[] _buffer, int _offset, int _count)
    {
        for (int i = 0; i < _count; i++)
        {
            //UpdateFrequency();
            _buffer[_offset + i] = currentAmplitude * (float)Math.Sin(phase);
            phase += 2f * MathF.PI * frequency / WaveFormat.SampleRate;
            if (phase >= MathF.PI * 2f)
                phase -= 2f * MathF.PI;
        }
        //Console.WriteLine("Target : " + targetFrequency.ToString());
        //Console.WriteLine("freq : " + frequency.ToString());
        return _count;

    }

    public void UpdateFrequency()
    {
        frequency += (targetFrequency - frequency) * GetDeltaSampleRate();
    }
}

