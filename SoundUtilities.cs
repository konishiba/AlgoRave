using NAudio.MediaFoundation;
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

static class Time
{
    static double deltatime = 0;
    static DateTime last = DateTime.UtcNow;

    public static double Deltatime => deltatime;

    public static void UpdateDeltime()
    {
        DateTime now = DateTime.UtcNow;
        deltatime = (now - last).TotalSeconds;
        last = now;
        // ... utiliser deltaSeconds
    }
}

static class NoteUtilites
{
    static Regex regex = new Regex(@"^([A-G])(#{0,1})(0|[1-8])$");

    public static double NoteToFrequence(string _fullNote)
    {
        Match _match = regex.Match(_fullNote);
        if (!_match.Success) return 0.0f;
        string _note = _match.Groups[1].Value;
        string _sharpe = _match.Groups[2].Value;
        string _value = _match.Groups[3].Value;
        int _octave = int.Parse(_value);

        double _frequence = NoteToFrequence(_note, _sharpe);

        return _frequence * MathF.Pow(2, _octave); //Note * 2^_octave
    }

    static ENote StringToENote(string note)
    {
        ENote _note = (ENote)Enum.Parse(typeof(ENote), note);
        return _note;
    }
    static ESemitones StringToESemitones(string note)
    {
        ESemitones _note = (ESemitones)Enum.Parse(typeof(ESemitones), note);
        return _note;
    }


    static double NoteToFrequence(string _note, string _sharp)
    {
        double _enumValue = 0;
        if(_sharp == "#")
        {
            _enumValue = (int)StringToESemitones(_note);
        }
        else
        {
            _enumValue = (int)StringToENote(_note);
        }

        return _enumValue / 100;
    }


}

