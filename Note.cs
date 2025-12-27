using CSCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Note
{
    public event Action IsFinished = null;
    public event Action IsDispose = null;

    int sampleRate = 44100;

    double phase = 0;
    double frequency = 440;/*, targetFrequency = 440;*/
    float currentAmplitude = 0.5f, amplitude = 0.5f;/*, tempAmplitude = 0.5f;*/
    double currentDuration = 0.0f,  duration = 0.0f;

    ADSR adrs = new ADSR();

    public ADSR ADSR => adrs;

    //debug
    public double CurrentDuration => currentDuration;
    public double CurrentAmplitude => currentAmplitude;
    public int SampleRate {  get; set; }

    //public bool CanSeek => false;

    //public CSCore.WaveFormat WaveFormat => new CSCore.WaveFormat(sampleRate, 32, 1, AudioEncoding.IeeeFloat);

    //public long Position { get => 0; set { } }

    //public long Length => 0;

    public Note(double _frequency, float _amplitude, float _duration, ADSR _adrs)
    {
        frequency = _frequency;
        amplitude = _amplitude;
        duration = _duration;
        adrs = _adrs;
    }

    public void Start()
    {
        IsFinished += () => { adrs.currentState = ADSR_STATE.RELEASE_STATE; };
        adrs.OnReleaseFinished += Dispose;
        adrs.Init(sampleRate);
        adrs.Start();
    }

    public void Update()
    {
        //Console.WriteLine(currentAmplitude);
        currentDuration += Time.Deltatime;
        if(currentDuration > duration)
        {
            IsFinished?.Invoke();
        }
    }


    public double NextSample()
    {
        //logger.PrintLog(VerbosityType.Warning, $"CurrentAmplitude {CurrentAmplitude}");
        //logger.PrintLog(VerbosityType.Warning, $"ADRS {adrs}");

        currentAmplitude = adrs.Update(currentAmplitude,amplitude);
        //UpdateFrequency();
        float _sample = currentAmplitude * (float)Math.Sin(phase);

        phase += 2f * MathF.PI * frequency / sampleRate;
        phase = phase >= MathF.PI * 2f ? phase -= 2f * MathF.PI : phase;

        return _sample;
    }

    //public int Read(float[] _buffer, int _offset, int _count)
    //{
    //    for (int i = 0; i < _count; i++)
    //    {
    //        adrs.Update(ref currentAmplitude, amplitude);
    //        //UpdateFrequency();
    //        _buffer[_offset + i] = currentAmplitude * (float)Math.Sin(phase);
    //        phase += 2f * MathF.PI * frequency / WaveFormat.SampleRate;
    //        phase = phase >= MathF.PI * 2f ? phase -= 2f * MathF.PI : phase;
    //    }
    //    //Console.WriteLine("Target : " + targetFrequency.ToString());
    //    //Console.WriteLine("freq : " + frequency.ToString());
    //    return _count;

    //}

    public void Dispose()
    {
        //adrs = null;
        IsDispose?.Invoke();
    }
}

