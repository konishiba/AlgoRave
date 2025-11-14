using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class WaveProvider : WaveProvider32
{
    float phase = 0;
    public float frequency = 440;

    public WaveProvider (float _frequency)
    {
        frequency = Math.Clamp(_frequency, 20, 18000);
    }

    public override int Read(float[] buffer, int offset, int count)
    {
        for (int i = 0; i < count; i++)
        {
            buffer[offset + i] = (float)Math.Sin(phase);
            phase += 2f * MathF.PI * frequency / WaveFormat.SampleRate;
        }
        return count;
    }
}

