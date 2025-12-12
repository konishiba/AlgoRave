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
    double frequency = 440;/*, targetFrequency = 440;*/
    float currentAmplitude = 0.5f, amplitude = 0.5f;/*, tempAmplitude = 0.5f;*/
    double currentDuration = 0.0f,  duration = 0.0f;

    ADSR adrs = new ADSR();

    public ADSR ADSR => adrs;

    //debug
    public double CurrentDuration => currentDuration;
    public double CurrentAmplitude => currentAmplitude;

    public Note(double _frequency, float _amplitude, float _duration, ADSR _adrs)
    {
        frequency = _frequency;
        amplitude = _amplitude;
        duration = _duration;
        adrs = _adrs;
        adrs.Init(WaveFormat.SampleRate);
    }

    public void Start()
    {
        IsFinished += () => { adrs.currentState = ADSR_STATE.RELEASE_STATE; };
        adrs.Start();
    }

    public void Update()
    {
        //Console.WriteLine(currentAmplitude);
        currentDuration += Time.Deltatime;
        if(currentDuration > duration)
        {
            //IsFinished?.Invoke();
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
            adrs.Update(ref currentAmplitude, amplitude);
            //UpdateFrequency();
            _buffer[_offset + i] = currentAmplitude * (float)Math.Sin(phase);
            phase += 2f * MathF.PI * frequency / WaveFormat.SampleRate;
            phase = phase >= MathF.PI * 2f ? phase -= 2f * MathF.PI : phase;
        }
        //Console.WriteLine("Target : " + targetFrequency.ToString());
        //Console.WriteLine("freq : " + frequency.ToString());
        return _count;

    }

    public void UpdateFrequency()
    {
        //frequency += (targetFrequency - frequency) * GetDeltaSampleRate();
    }
}

