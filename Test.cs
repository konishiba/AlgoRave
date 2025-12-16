using CSCore;
using System;

public class Test : ISampleSource
{
    private readonly int _sampleRate;
    private readonly float _frequency;
    private double _phase;

    public Test(int sampleRate = 44100, float frequency = 440f)
    {
        _sampleRate = sampleRate;
        _frequency = frequency;
    }

    public bool CanSeek => false;
    public WaveFormat WaveFormat => new WaveFormat(_sampleRate, 32, 1, AudioEncoding.IeeeFloat);
    public long Length => 0;
    public long Position { get => 0; set { } }

    public int Read(float[] buffer, int offset, int count)
    {
        for (int i = 0; i < count; i++)
        {
            buffer[offset + i] = (float)Math.Sin(_phase);
            _phase += 2 * Math.PI * _frequency / _sampleRate;

            if (_phase > Math.PI * 2)
                _phase -= (float)(Math.PI * 2);
        }

        return count;
    }

    public void Dispose() { }
}