using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


public enum ESampleRate
{
    LOW_QUALITY = 22050,
    MEDIAM_QUALITY = 44100,
    HIGH_QUALITY = 96000,
};

public enum EChannelsType
{
    MONO = 1,
    STEREO = 2,
};

public enum ENote
{
    C = 1635,
    D = 1835,
    E = 2060,
    F = 2183,
    G = 2450,
    A = 2750,
    B = 3087,
};

public enum ESemitones
{
    C = 1732,
    D = 1945,
    F = 2312,
    G = 2596,
    A = 2917
};


static class NoteUtilites
{
    static Regex regex = new Regex(@"^[A-G](#{0,1})(?:0|[1-8])$");

    static float NoteToFrequence(string _fullNote)
    {
        Match match = regex.Match(_fullNote);
        if (!match.Success) return 0.0f;
        string _note = match.Groups[1].Value;
        string _Sharpe = match.Groups[2].Value;
        int _octave = int.Parse(match.Groups[3].Value);
        return 0.0f; //Note * 2^_octave
    }

}

