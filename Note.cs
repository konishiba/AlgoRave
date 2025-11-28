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
    public float amplitude = 0.5f, targetAmplitude = 0.5f, tempAmplitude = 0.5f;
     
    public override int Read(float[] _buffer, int _offset, int _count)
    {
        for (int i = 0; i < _count; i++)
        {
            UpdateFrequency();
            UpdateAmplitude();

            float _sin = (float)Math.Sin(phase);
            _buffer[_offset + i] = amplitude * _sin;
            phase += 2f * MathF.PI * frequency / WaveFormat.SampleRate;

            if (phase > MathF.PI * 2)
                phase -= MathF.PI * 2;
        }
        Console.WriteLine("Target : " + targetFrequency.ToString());
        Console.WriteLine("freq : " + frequency.ToString());
        return _count;
    }

    public void UpdateFrequency()
    {
        frequency += (targetFrequency - frequency) * 0.001f;
    }
    public void UpdateAmplitude()
    {
        amplitude += (targetAmplitude - amplitude) * 0.001f;
    }
}

